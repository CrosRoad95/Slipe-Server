﻿using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Server.Concepts;
using SlipeServer.Server.Elements.Enums;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.Extensions;
using SlipeServer.Server.PacketHandling.Factories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlipeServer.Server.Elements;

/// <summary>
/// The base class for any and all elements
/// </summary>
public class Element
{
    public virtual ElementType ElementType => ElementType.Unknown;

    private Element? parent;
    public Element? Parent
    {
        get => this.parent;
        set
        {
            this.parent = value;

            if (this.parent != null)
                this.parent.RemoveChild(this);

            if (value != null)
                value.AddChild(this);
        }
    }

    private readonly List<Element> children;
    public IReadOnlyCollection<Element> Children => this.children.AsReadOnly();

    private uint id;
    public uint Id
    {
        get => this.id;
        set
        {
            var args = new ElementChangedEventArgs<uint>(this, this.id, value, this.IsSync);
            this.id = value;
            IdChanged?.Invoke(this, args);
        }
    }

    private readonly object timeContextLock = new();
    public byte TimeContext { get; private set; }


    private string name = "";
    public string Name
    {
        get => this.name;
        set
        {
            var args = new ElementChangedEventArgs<string>(this, this.Name, value, this.IsSync);
            this.name = value;
            NameChanged?.Invoke(this, args);
        }
    }


    protected Vector3 position;
    public Vector3 Position
    {
        get => this.position;
        set
        {
            var args = new ElementChangedEventArgs<Vector3>(this, this.Position, value, this.IsSync);
            this.position = value;
            PositionChanged?.Invoke(this, args);

            foreach (var attachment in this.attachedElements)
                attachment.UpdateAttachedElement();
        }
    }

    public Vector3 Right => Vector3.Transform(Vector3.UnitX, this.rotation.ToQuaternion());
    public Vector3 Up => Vector3.Transform(Vector3.UnitZ, this.rotation.ToQuaternion());
    public Vector3 Forward => Vector3.Transform(Vector3.UnitY, this.rotation.ToQuaternion());

    protected Vector3 rotation;
    public Vector3 Rotation
    {
        get => this.rotation;
        set
        {
            var args = new ElementChangedEventArgs<Vector3>(this, this.Rotation, value, this.IsSync);
            this.rotation = value;
            RotationChanged?.Invoke(this, args);

            foreach (var attachment in this.attachedElements)
                attachment.UpdateAttachedElement();
        }
    }

    protected Vector3 velocity;
    public Vector3 Velocity
    {
        get => this.velocity;
        set
        {
            var args = new ElementChangedEventArgs<Vector3>(this, this.Velocity, value, this.IsSync);
            this.velocity = value;
            VelocityChanged?.Invoke(this, args);
        }
    }

    protected Vector3 turnVelocity;
    public Vector3 TurnVelocity
    {
        get => this.turnVelocity;
        set
        {
            var args = new ElementChangedEventArgs<Vector3>(this, this.TurnVelocity, value, this.IsSync);
            this.turnVelocity = value;
            TurnVelocityChanged?.Invoke(this, args);
        }
    }

    protected byte interior;
    public byte Interior
    {
        get => this.interior;
        set
        {
            var args = new ElementChangedEventArgs<byte>(this, this.Interior, value, this.IsSync);
            this.interior = value;
            InteriorChanged?.Invoke(this, args);
        }
    }

    protected ushort dimension;
    public ushort Dimension
    {
        get => this.dimension;
        set
        {
            var args = new ElementChangedEventArgs<ushort>(this, this.Dimension, value, this.IsSync);
            this.dimension = value;
            DimensionChanged?.Invoke(this, args);
        }
    }

    protected byte alpha = 255;
    public byte Alpha
    {
        get => this.alpha;
        set
        {
            var args = new ElementChangedEventArgs<byte>(this, this.Alpha, value, this.IsSync);
            this.alpha = value;
            AlphaChanged?.Invoke(this, args);
        }
    }


    protected bool areCollisionsEnabled = true;
    public bool AreCollisionsEnabled
    {
        get => this.areCollisionsEnabled;
        set
        {
            var args = new ElementChangedEventArgs<bool>(this, this.areCollisionsEnabled, value, this.IsSync);
            this.areCollisionsEnabled = value;
            CollisionEnabledhanged?.Invoke(this, args);
        }
    }

    protected bool isCallPropagationEnabled = false;
    public bool IsCallPropagationEnabled
    {
        get => this.isCallPropagationEnabled;
        set
        {
            var args = new ElementChangedEventArgs<bool>(this, this.isCallPropagationEnabled, value, this.IsSync);
            this.isCallPropagationEnabled = value;
            CallPropagationChanged?.Invoke(this, args);
        }
    }

    protected bool isFrozen = false;
    public bool IsFrozen
    {
        get => this.isFrozen;
        set
        {
            var args = new ElementChangedEventArgs<bool>(this, this.isFrozen, value, this.IsSync);
            this.isFrozen = value;
            FrozenChanged?.Invoke(this, args);
        }
    }

    private AsyncLocal<ElementUpdateContext> updateContext = new();
    public ElementUpdateContext UpdateContext
    {
        get => this.updateContext?.Value ?? ElementUpdateContext.Default;
        protected set
        {
            this.updateContext ??= new AsyncLocal<ElementUpdateContext>();
            this.updateContext.Value = value;
        }
    }

    public bool IsSync
    {
        get => this.UpdateContext.HasFlag(ElementUpdateContext.Sync);
        set => this.UpdateContext = ElementUpdateContext.Sync;
    }

    private readonly HashSet<Player> subscribers;
    public IEnumerable<Player> Subscribers => this.subscribers;

    private Dictionary<string, ElementData> ElementData { get; set; }
    public ConcurrentDictionary<Player, ConcurrentDictionary<string, bool>> ElementDataSubscriptions { get; set; }

    public ElementAttachment? Attachment { get; private set; }

    private readonly List<ElementAttachment> attachedElements;
    public IReadOnlyCollection<ElementAttachment> AttachedElements => this.attachedElements.AsReadOnly();
    public bool IsDestroyed { get; set; }

    private readonly object destroyLock = new();

    public bool ExistsForAllPlayers { get; set; } = true;

    public Element()
    {
        this.children = new();
        this.subscribers = new();
        this.attachedElements = new();
        this.TimeContext = 1;

        this.ElementData = new();
        this.ElementDataSubscriptions = new();
    }

    public Element(Element parent) : this()
    {
        this.Parent = parent;
    }

    public void AddSubscriber(Player player)
    {
        if (this.subscribers.Contains(player))
            return;

        this.subscribers.Add(player);
        player.SubscribeTo(this);
    }

    public void RemoveSubscriber(Player player)
    {
        if (!this.subscribers.Contains(player))
            return;

        this.subscribers.Remove(player);
        player.UnsubscribeFrom(this);
    }


    public byte GetAndIncrementTimeContext()
    {
        lock (this.timeContextLock)
        {
            if (++this.TimeContext == 0)
            {
                this.TimeContext++;
            }
            return this.TimeContext;
        }
    }

    public bool CanUpdateSync(byte remoteContext)
    {
        return (this.TimeContext == remoteContext || remoteContext == 0 || this.TimeContext == 0);
    }

    public bool Destroy()
    {
        lock (this.destroyLock)
        {
            if (this.IsDestroyed)
                return false;

            this.IsDestroyed = true;
            this.Destroyed?.Invoke(this);
            return true;
        }
    }

    public void RunWithContext(Action action, ElementUpdateContext context)
    {
        var oldValue = this.UpdateContext;
        this.UpdateContext = context;
        action();
        this.UpdateContext = oldValue;
    }

    public async Task RunWithContext(Func<Task> action, ElementUpdateContext context)
    {
        var oldValue = this.UpdateContext;
        this.UpdateContext = context;
        await action();
        this.UpdateContext = oldValue;
    }

    public void RunAsSync(Action action, bool value = true)
        => RunWithContext(action, value ? ElementUpdateContext.Sync : ElementUpdateContext.NoRelay);

    public Task RunAsSync(Func<Task> action, bool value = true)
        => RunWithContext(action, value ? ElementUpdateContext.Sync : ElementUpdateContext.NoRelay);

    public Element AssociateWith(MtaServer server)
    {
        return server.AssociateElement(this);
    }

    public void AddChild(Element element)
    {
        this.children.Add(element);
        element.Destroyed += (element) => RemoveChild(element);
    }

    public void RemoveChild(Element element)
    {
        this.children.Remove(element);
    }

    public bool IsChildOf(Element element)
    {
        return element != null && (this.parent == element || (this.parent != null && this.parent.IsChildOf(element)));
    }

    public void SetData(string key, LuaValue value, DataSyncType syncType = DataSyncType.Local)
    {
        LuaValue? oldValue = this.GetData(key);

        if (value.IsNil)
            this.ElementData.Remove(key);
        else
            this.ElementData[key] = new ElementData(key, value, syncType);

        this.DataChanged?.Invoke(this, new ElementDataChangedArgs(key, value, oldValue, syncType));
    }

    public LuaValue? GetData(string dataName, bool inherit = false)
    {
        if (this.ElementData.TryGetValue(dataName, out var value))
            return value.Value;

        if (inherit)
            return this.parent?.GetData(dataName, inherit);

        return null;
    }

    public void SubscribeToData(Player player, string key)
    {
        if (!this.ElementDataSubscriptions.ContainsKey(player))
        {
            this.ElementDataSubscriptions[player] = new();
            player.Destroyed += HandleElementDataSubscriberDestruction;
        }
        this.ElementDataSubscriptions[player].TryAdd(key, true);

    }

    private void HandleElementDataSubscriberDestruction(Element element)
    {
        if (element is Player player)
            UnsubscribeFromAllData(player);
    }

    public void UnsubscribeFromData(Player player, string key)
    {
        if (this.ElementDataSubscriptions.TryGetValue(player, out var keys))
        {
            keys.Remove(key, out var _);
            if (keys.IsEmpty)
                UnsubscribeFromAllData(player);
        }
    }

    public void UnsubscribeFromAllData(Player player)
    {
        player.Destroyed -= HandleElementDataSubscriberDestruction;
        this.ElementDataSubscriptions.Remove(player, out var keys);
    }

    public bool IsPlayerSubscribedToData(Player player, string key)
    {
        if (this.ElementDataSubscriptions.TryGetValue(player, out var keys))
            return keys.ContainsKey(key);

        return false;
    }

    public IEnumerable<Player> GetPlayersSubcribedToData(string key)
    {
        return this.ElementDataSubscriptions.Keys
            .Where(x => this.ElementDataSubscriptions[x].ContainsKey(key));
    }

    internal void AddElementAttachment(ElementAttachment attachment) => this.attachedElements.Add(attachment);
    internal void RemoveElementAttachment(ElementAttachment attachment) => this.attachedElements.Remove(attachment);

    /// <summary>
    /// Attaches the element to a target element
    /// </summary>
    /// <param name="element">Target element to attach to</param>
    /// <param name="positionOffset">position offset between the element, and the target element</param>
    /// <param name="rotationOffset">rotation offset between the element, and the target element</param>
    /// <returns></returns>
    /// <exception cref="Exception">Throws an exception when the target element does not have an ID</exception>
    public ElementAttachment AttachTo(Element element, Vector3? positionOffset = null, Vector3? rotationOffset = null)
    {
        if (element.Id == 0)
            throw new Exception(string.Format("Can not attach {0} to {1} because {1} has no id", this, element, element));

        var position = positionOffset ?? Vector3.Zero;
        var rotation = rotationOffset ?? Vector3.Zero;

        if (this.Attachment != null)
            DetachFrom(this.Attachment.Target);

        var attachment = new ElementAttachment(this, element, position, rotation);
        this.Attachment = attachment;
        element.AddElementAttachment(attachment);
        attachment.UpdateAttachedElement();

        attachment.PositionOffsetChanged += HandleAttachmentPositionOffsetChange;
        attachment.RotationOffsetChanged += HandleAttachmentRotationOffsetChange;

        this.Attached?.Invoke(this, new ElementAttachedEventArgs(this, element, position, rotation));

        return attachment;
    }

    private void HandleAttachmentPositionOffsetChange(Vector3 newPosition)
    {
        this.AttachedOffsetChanged?.Invoke(this, new ElementAttachOffsetsChangedArgs(this, this.Attachment!.Target, newPosition, this.Attachment.RotationOffset));
    }

    private void HandleAttachmentRotationOffsetChange(Vector3 newRotation)
    {
        this.AttachedOffsetChanged?.Invoke(this, new ElementAttachOffsetsChangedArgs(this, this.Attachment!.Target, this.Attachment.PositionOffset, newRotation));
    }

    /// <summary>
    /// Detaches the element from a target element
    /// </summary>
    /// <param name="element">Target element to detach from</param>
    public virtual void DetachFrom(Element? element = null)
    {
        if (this.Attachment != null && (element == null || this.Attachment.Target == element))
        {
            this.Detached?.Invoke(this, new ElementDetachedEventArgs(this, this.Attachment.Target));
            (element ?? this.Attachment.Target).RemoveElementAttachment(this.Attachment);

            this.Attachment.PositionOffsetChanged -= HandleAttachmentPositionOffsetChange;
            this.Attachment.RotationOffsetChanged -= HandleAttachmentRotationOffsetChange;

            this.Attachment = null;
        }
    }

    /// <summary>
    /// Sends packets to create an elementto a set of players
    /// Do note that the element will be required to have an id assigned for this to work properly
    /// </summary>
    public void CreateFor(IEnumerable<Player> players)
        => AddEntityPacketFactory.CreateAddEntityPacket(new Element[] { this }).SendTo(players);

    /// <summary>
    /// Sends packets to create an elementto a set of players
    /// Do note that the element will be required to have an id assigned for this to work properly
    /// </summary>
    public void CreateFor(Player player)
        => this.CreateFor(new Player[] { player });

    /// <summary>
    /// Sends packets to destroy an elementto a set of players
    /// Do note that the element will be required to have an id assigned for this to work properly
    /// </summary>
    public void DestroyFor(IEnumerable<Player> players)
        => RemoveEntityPacketFactory.CreateRemoveEntityPacket(new Element[] { this }).SendTo(players);

    /// <summary>
    /// Sends packets to destroy an elementto a set of players
    /// Do note that the element will be required to have an id assigned for this to work properly
    /// </summary>
    public void DestroyFor(Player player)
        => this.DestroyFor(new Player[] { player });

    public event ElementChangedEventHandler<Vector3>? PositionChanged;
    public event ElementChangedEventHandler<Vector3>? RotationChanged;
    public event ElementChangedEventHandler<Vector3>? VelocityChanged;
    public event ElementChangedEventHandler<Vector3>? TurnVelocityChanged;
    public event ElementChangedEventHandler<byte>? InteriorChanged;
    public event ElementChangedEventHandler<ushort>? DimensionChanged;
    public event ElementChangedEventHandler<byte>? AlphaChanged;
    public event ElementChangedEventHandler<string>? NameChanged;
    public event ElementChangedEventHandler<bool>? CallPropagationChanged;
    public event ElementChangedEventHandler<bool>? CollisionEnabledhanged;
    public event ElementChangedEventHandler<bool>? FrozenChanged;
    public event ElementChangedEventHandler<Element, uint>? IdChanged;
    public event ElementEventHandler<Element, ElementDataChangedArgs>? DataChanged;
    public event ElementEventHandler<Element, ElementAttachedEventArgs>? Attached;
    public event ElementEventHandler<Element, ElementDetachedEventArgs>? Detached;
    public event ElementEventHandler<Element, ElementAttachOffsetsChangedArgs>? AttachedOffsetChanged;
    public event Action<Element>? Destroyed;

    /// <summary>
    /// Returns a Lua value for the element, this is used for any lua event communication.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator LuaValue(Element value) => new(value.Id);
}
