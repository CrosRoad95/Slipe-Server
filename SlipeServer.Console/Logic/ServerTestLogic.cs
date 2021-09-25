using Microsoft.Extensions.Logging;
using SlipeServer.Packets.Definitions.Lua;
using SlipeServer.Packets.Lua.Camera;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Elements.ColShapes;
using SlipeServer.Server.Elements.Enums;
using SlipeServer.Server.Elements.Events;
using SlipeServer.Server.Elements.Structs;
using SlipeServer.Server.Enums;
using SlipeServer.Server.Repositories;
using SlipeServer.Server.Resources;
using SlipeServer.Server.Resources.Providers;
using SlipeServer.Server.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace SlipeServer.Console.Logic
{
    public class ServerTestLogic
    {
        private readonly MtaServer server;
        private readonly IElementRepository elementRepository;
        private readonly RootElement root;
        private readonly GameWorld worldService;
        private readonly DebugLog debugLog;
        private readonly ILogger logger;
        private readonly ChatBox chatBox;
        private readonly ClientConsole console;
        private readonly LuaEventService luaService;
        private readonly ExplosionService explosionService;
        private readonly FireService fireService;
        private readonly TextItemService textItemService;
        private readonly IResourceProvider resourceProvider;
        private Resource? testResource;
        private Resource? secondTestResource;

        private readonly Random random = new();
        private RadarArea? RadarArea { get; set; }
        private Marker? Marker { get; set; }
        private Blip? BlipA { get; set; }
        private Blip? BlipB { get; set; }
        private WorldObject? WorldObject { get; set; }
        private Vehicle? Vehicle { get; set; }
        private Vehicle? Aircraft { get; set; }
        private Vehicle? Taxi { get; set; }
        private Vehicle? Rhino { get; set; }
        private Ped? Ped { get; set; }
        private readonly Team slipeDevsTeam;

        public ServerTestLogic(
            MtaServer server,
            IElementRepository elementRepository,
            RootElement root,
            GameWorld world,
            DebugLog debugLog,
            ILogger logger,
            ChatBox chatBox,
            ClientConsole console,
            LuaEventService luaService,
            ExplosionService explosionService,
            FireService fireService,
            TextItemService textItemService,
            IResourceProvider resourceProvider
        )
        {
            this.server = server;
            this.elementRepository = elementRepository;
            this.root = root;
            this.worldService = world;
            this.debugLog = debugLog;
            this.logger = logger;
            this.chatBox = chatBox;
            this.console = console;
            this.luaService = luaService;
            this.explosionService = explosionService;
            this.fireService = fireService;
            this.textItemService = textItemService;
            this.resourceProvider = resourceProvider;
            this.SetupTestLogic();
            this.slipeDevsTeam = new Team("Slipe devs", Color.FromArgb(255, 255, 81, 81));
        }

        private void SetupTestLogic()
        {
            SetupTestElements();

            this.luaService.AddEventHandler("Slipe.Test.Event", (e) => this.TriggerTestEvent(e.Player));

            this.worldService.SetWeather(Weather.ExtraSunnyDesert);
            this.worldService.CloudsEnabled = false;
            this.worldService.SetTime(13, 37);
            this.worldService.MinuteDuration = 60_000;
            this.worldService.SetSkyGradient(Color.Aqua, Color.Teal);
            this.worldService.SetGlitchEnabled(GlitchType.GLITCH_FASTSPRINT, true);

            this.server.PlayerJoined += OnPlayerJoin;
        }

        private void SetupTestElements()
        {
            this.testResource = this.resourceProvider.GetResource("TestResource");
            this.secondTestResource = this.resourceProvider.GetResource("SecondTestResource");

            new WorldObject(321, new Vector3(5, 0, 3)).AssociateWith(this.server);
            new Water(new Vector3[]
            {
                new Vector3(-6, 0, 4), new Vector3(-3, 0, 4),
                new Vector3(-6, 3, 4), new Vector3(-3, 3, 4)
            }).AssociateWith(this.server);
            new WorldObject(321, new Vector3(5, 0, 3)).AssociateWith(this.server);
            this.BlipA = new Blip(new Vector3(20, 0, 0), BlipIcon.Marker, 50).AssociateWith(this.server);
            this.BlipB = new Blip(new Vector3(15, 0, 0), BlipIcon.Marker, 50).AssociateWith(this.server);
            this.RadarArea = new RadarArea(new Vector2(0, 0), new Vector2(200, 200), Color.FromArgb(100, Color.Aqua)).AssociateWith(this.server);

            this.Marker = new Marker(new Vector3(5, 0, 2), MarkerType.Cylinder)
            {
                Color = Color.FromArgb(100, Color.Cyan)
            }.AssociateWith(this.server);

            new Pickup(new Vector3(0, -5, 3), PickupType.Health, 20)
            {
                RespawnTime = 5000
            }.AssociateWith(this.server);
            new Pickup(new Vector3(3, -5, 3), PickupType.Armor, 20)
            {
                RespawnTime = 5000
            }.AssociateWith(this.server);
            new Pickup(new Vector3(5, -5, 3), WeaponType.WEAPONTYPE_AK47, 100)
            {
                RespawnTime = 5000
            }.AssociateWith(this.server);

            var values = Enum.GetValues(typeof(PedModel));
            PedModel randomPedModel = (PedModel)values.GetValue(new Random().Next(values.Length))!;
            this.Ped = new Ped(randomPedModel, new Vector3(10, 0, 3)).AssociateWith(this.server);

            this.WorldObject = new WorldObject(ObjectModel.Drugred, new Vector3(15, 0, 3)).AssociateWith(this.server);

            new WeaponObject(355, new Vector3(10, 10, 5))
            {
                TargetType = WeaponTargetType.Fixed,
                TargetPosition = new Vector3(10, 10, 5)
            }.AssociateWith(this.server);
            var vehicle = new Vehicle(602, new Vector3(-10, 5, 3)).AssociateWith(this.server);
            this.Aircraft = new Vehicle(520, new Vector3(10, 5, 3)).AssociateWith(this.server);
            this.Vehicle = new Vehicle(530, new Vector3(20, 5, 3)).AssociateWith(this.server);
            this.Taxi = new Vehicle((ushort)VehicleModel.Taxi, new Vector3(20, -5, 3)).AssociateWith(this.server);
            this.Rhino = new Vehicle((ushort)VehicleModel.Rhino, new Vector3(20, -25, 3)).AssociateWith(this.server);
            var forklift2 = new Vehicle(530, new Vector3(22, 5, 3)).AssociateWith(this.server);
            var firetruck = new Vehicle(407, new Vector3(30, 5, 3)).AssociateWith(this.server);
            var firetruck2 = new Vehicle(407, new Vector3(35, 5, 3)).AssociateWith(this.server);

            var polygon1 = new CollisionPolygon(new Vector3(0, -25, 0), new Vector2[] { new Vector2(-25, -25), new Vector2(-25, -50), new Vector2(-50, -25) }).AssociateWith(this.server);
            var polygon2 = new CollisionPolygon(new Vector3(0, 25, 0), new Vector2[] { new Vector2(25, 25), new Vector2(25, 50), new Vector2(50, 25) }).AssociateWith(this.server);

            vehicle.PedEntered += async (sender, eventArgs) =>
            {
                if (eventArgs.Seat == 1)
                {
                    await Task.Delay(500);
                    eventArgs.Vehicle.RemovePassenger(eventArgs.Ped);
                }
            };

            vehicle.PedLeft += async (sender, eventArgs) =>
            {
                if (eventArgs.Seat == 0)
                {
                    await Task.Delay(1500);
                    vehicle?.Respawn();
                    this.logger.LogInformation("Vehicle has been respawned.");
                }
            };

            var circle = new CollisionCircle(new Vector2(0, 25), 3).AssociateWith(this.server);
            var sphere = new CollisionSphere(new Vector3(0, 25, 0), 3).AssociateWith(this.server);
            var tube = new CollisionTube(new Vector3(0, 25, 0), 3, 3).AssociateWith(this.server);
            var polygon = new CollisionPolygon(new Vector3(0, -25, 0), new Vector2[] { new Vector2(-25, -25), new Vector2(-25, -50), new Vector2(-50, -25) }).AssociateWith(this.server);
            var rectangle = new CollisionRectangle(new Vector2(50, 20), new Vector2(2, 2)).AssociateWith(this.server);
            var cuboid = new CollisionCuboid(new Vector3(30, 20, 4), new Vector3(2, 2, 2)).AssociateWith(this.server);
            Task.Run(async () =>
            {
                int i = 0;
                while (true)
                {
                    await Task.Delay(1000);
                    this.WorldObject.Model = (ushort)ObjectModel.Drugblue;
                    this.Vehicle.Model = (ushort)VehicleModel.Bobcat;
                    this.Ped.Model = (ushort)this.random.Next(20, 25);
                    this.Taxi.IsTaxiLightOn = !this.Taxi.IsTaxiLightOn;
                    await Task.Delay(1000);
                    this.WorldObject.Model = (ushort)ObjectModel.Drugred;
                    this.Vehicle.Model = (ushort)VehicleModel.BMX;
                    this.Ped.Model = (ushort)this.random.Next(20, 25);
                    this.Taxi.IsTaxiLightOn = !this.Taxi.IsTaxiLightOn;
                    this.Taxi.PlateText = $"i {i++}";
                }
            });

            var shape = new CollisionCircle(new Vector2(0, 25), 3).AssociateWith(this.server);

            circle.RadiusChanged += async (Element sender, ElementChangedEventArgs<float> args) =>
            {
                await Task.Delay(100);
                if (circle.Radius < 20)
                    circle.Radius += .03f;
            };

            sphere.RadiusChanged += async (Element sender, ElementChangedEventArgs<float> args) =>
            {
                await Task.Delay(100);
                if (sphere.Radius < 20)
                    sphere.Radius += .03f;
            };

            tube.RadiusChanged += async (Element sender, ElementChangedEventArgs<float> args) =>
            {
                await Task.Delay(100);
                if (tube.Radius < 20)
                    tube.Radius += .03f;
            };

            tube.HeightChanged += async (Element sender, ElementChangedEventArgs<float> args) =>
            {
                await Task.Delay(100);
                if (tube.Height < 20)
                    tube.Height += .03f;
            };

            polygon.HeightChanged += async (Element sender, ElementChangedEventArgs<Vector2> args) =>
            {
                await Task.Delay(100);
                if (polygon.Height.X > -3)
                    polygon.Height = new Vector2(polygon.Height.X - .03f, polygon.Height.Y - .03f);
            };

            polygon.PointPositionChanged += async (Element sender, CollisionPolygonPointPositionChangedArgs args) =>
            {
                await Task.Delay(100);
                if (args.Position.X < 0.0f)
                    args.Polygon.SetPointPosition(args.Index, new Vector2(args.Position.X + 0.03f, args.Position.Y));
            };

            rectangle.DimensionsChanged += async (Element sender, ElementChangedEventArgs<Vector2> args) =>
            {
                await Task.Delay(100);
                if (args.NewValue.Y < 10.0f)
                    rectangle.Dimensions = args.OldValue + new Vector2(0.03f, 0.03f);
            };

            cuboid.DimensionsChanged += async (Element sender, ElementChangedEventArgs<Vector3> args) =>
            {
                await Task.Delay(100);
                if (args.NewValue.Y < 10.0f)
                    cuboid.Dimensions = args.OldValue + new Vector3(0.03f, 0.03f, 0.03f);
            };

            Task.Run(async () =>
            {
                await Task.Delay(5000);
                for (int i = 0; i < 5; i++)
                {
                    await Task.Delay(2000);
                    polygon.AddPoint(new Vector2(this.random.Next(-20, 20), this.random.Next(-20, 20)));
                }
                for (int i = 0; i < 5; i++)
                {
                    await Task.Delay(2000);
                    polygon.RemovePoint(0);
                }
            });

            circle.Radius = 3;
            sphere.Radius = 3;
            tube.Radius = 3;
            tube.Height = 3;
            polygon.Height = new Vector2(10, 15);
            polygon.SetPointPosition(0, new Vector2(-25, -25));
            rectangle.Dimensions = new Vector2(2, 2);
            cuboid.Dimensions = new Vector3(2, 2, 2);
        }

        private void OnPlayerJoin(Player player)
        {
            var client = player.Client;

            this.chatBox.Output($"{player.Name} ({client.Version}) ({client.Serial}) has joined the server!");

            player.Spawn(new Vector3(0, 0, 3), 0, 7, 0, 0);
            player.Health = 50;
            player.Alpha = 100;
            player.Camera.Target = player;
            player.Camera.Fade(CameraFade.In);

            this.chatBox.OutputTo(player, "Hello world");
            this.chatBox.ClearFor(player);
            this.chatBox.OutputTo(player, "Hello World Again");

            this.console.OutputTo(player, "Hello Console World");

            this.debugLog.SetVisibleTo(player, true);
            this.debugLog.OutputTo(player, "Test debug message", DebugLevel.Custom, Color.Red);
            this.debugLog.OutputTo(player, "Test debug message 2", DebugLevel.Information);

            player.ShowHudComponent(HudComponent.Money, false);
            player.SetFpsLimit(60);
            player.PlaySound(1);
            player.WantedLevel = 4;
            //player.ForceMapVisible(true);
            //player.ToggleAllControls(false, true, true);

            player.Kicked += (player, args) =>
            {
                this.logger.LogWarning($"{player.Name} has been kicked, reason: {args.Reason}");
            };

            player.Wasted += async (o, args) =>
            {
                await Task.Delay(500);
                player.Camera.Fade(CameraFade.Out, 1.75f);
                await Task.Delay(2000);
                player.Camera.Fade(CameraFade.In, 0);
                player.Spawn(new Vector3(0, 0, 3), 0, 7, 0, 0);
            };

            player.ScreenshotTaken += HandlePlayerScreenshot;

            player.Weapons.Add(new Weapon(WeaponId.Ak47, 500));
            player.Weapons.Add(new Weapon(WeaponId.Tec9, 500));
            player.Weapons.Add(new Weapon(WeaponId.Sniper, 500));
            player.Weapons.Add(new Weapon(WeaponId.Deagle, 500));
            player.Weapons.Add(new Weapon(WeaponId.Golfclub, 1));
            player.Weapons.Add(new Weapon(WeaponId.Satchel, 25));
            player.Weapons.Remove(WeaponId.Tec9);
            player.Weapons.Remove(WeaponId.Sniper);
            player.Weapons.First(weapon => weapon.Type == WeaponId.Deagle).Ammo -= 200;
            player.Weapons.First(weapon => weapon.Type == WeaponId.Ak47).Ammo = 750;
            player.Weapons.First(weapon => weapon.Type == WeaponId.Ak47).AmmoInClip = 25;

            this.testResource?.StartFor(player);
            this.secondTestResource?.StartFor(player);

            this.HandlePlayerSubscriptions(player);
            this.HandlePlayerCommands(player);

            player.TeamChanged += (thePlayer, args) =>
            {
                this.logger.LogDebug($"{thePlayer.Name} Joined {thePlayer.Team?.TeamName} team!");
            };
            player.TargetChanged += (thePlayer, args) =>
            {
                if(args.NewValue != null && args.NewValue is Vehicle vehicle)
                {
                    if(vehicle.Model == (ushort)VehicleModel.Rhino)
                        this.logger.LogDebug($"{thePlayer.Name} Changed target rhino");
                }
            };

            player.Team = this.slipeDevsTeam;
        }

        private void HandlePlayerSubscriptions(Player player)
        {
            var otherPlayers = this.elementRepository.GetByType<Player>(ElementType.Player).Where(x => x != player);
            foreach (var otherPlayer in otherPlayers)
            {
                otherPlayer.SubscribeTo(player);
                player.SubscribeTo(otherPlayer);
            }


            player.CommandEntered += (o, args) =>
            {
                Player? otherPlayer;
                switch (args.Command)
                {
                    case "sub":
                        otherPlayer = this.elementRepository
                            .GetByType<Player>(ElementType.Player)
                            .SingleOrDefault(x => x.Name == args.Arguments[0]);

                        if (otherPlayer != null)
                            player.SubscribeTo(otherPlayer);
                        break;
                    case "unsub":
                        otherPlayer = this.elementRepository
                            .GetByType<Player>(ElementType.Player)
                            .SingleOrDefault(x => x.Name == args.Arguments[0]);

                        if (otherPlayer != null)
                            player.UnsubscribeFrom(otherPlayer);
                        break;
                }
            };
        }

        private void HandlePlayerCommands(Player player)
        {
            player.CommandEntered += (o, args) =>
            {
                if (args.Command == "radararea")
                {
                    this.RadarArea!.Color = Color.FromArgb(this.random.Next(0, 255), this.random.Next(0, 255), this.random.Next(0, 255), this.random.Next(0, 255));
                    this.RadarArea.Size = new Vector2(this.random.Next(100, 200), this.random.Next(100, 200));
                    this.RadarArea.IsFlashing = this.random.Next(2) == 1;
                    this.chatBox.OutputTo(player, "You have randomized radar area!", Color.YellowGreen);
                }
            };

            bool flip = false;
            player.CommandEntered += (o, args) => { if (args.Command == "kill") player.Kill(); };
            player.CommandEntered += (o, args) => { if (args.Command == "spawn") player.Spawn(new Vector3(20, 0, 3), 0, 9, 0, 0); };
            player.CommandEntered += (o, args) =>
            {
                if (args.Command == "night")
                    this.worldService.SetTime(0, 0);

                if (args.Command == "day")
                    this.worldService.SetTime(13, 37);

                if (args.Command == "blip")
                {
                    var values = Enum.GetValues(typeof(BlipIcon));
                    BlipIcon randomBlipIcon = (BlipIcon)values.GetValue(this.random.Next(values.Length))!;

                    this.BlipB!.Icon = randomBlipIcon;
                    this.BlipA!.Color = Color.FromArgb(this.random.Next(0, 255), this.random.Next(0, 255), this.random.Next(0, 255), this.random.Next(0, 255));
                    this.BlipA.Size = (byte)this.random.Next(1, 4);
                    this.BlipA.VisibleDistance = (ushort)this.random.Next(30, 100);
                    flip = !flip;
                    if (flip)
                    {
                        this.BlipA.Ordering = 1;
                        this.BlipB.Ordering = 2;
                    } else
                    {
                        this.BlipA.Ordering = 2;
                        this.BlipB.Ordering = 1;
                    }
                }
                if (args.Command == "boom")
                    this.explosionService.CreateExplosion(player.Position, ExplosionType.Tiny);

                if (args.Command == "m4")
                    player.CurrentWeapon = new Weapon(WeaponId.M4, 500);

                if (args.Command == "assault")
                    player.CurrentWeaponSlot = WeaponSlot.AssaultRifles;

                if (args.Command == "rocket")
                    player.CurrentWeapon = new Weapon(WeaponId.RocketLauncher, 500);

                if (args.Command == "shootrocket")
                {
                    var position = player.Position + new Vector3(0, 0, 0.7f);
                    this.worldService.CreateProjectile(position, player.Rotation, player);
                }

                if (args.Command == "fire")
                    this.fireService.CreateFire(player.Position);

                if (args.Command == "ts")
                    player.TakeScreenshot(256, 256, "lowqualitytag", 30);

                if (args.Command == "tshq")
                    player.TakeScreenshot(960, 540, "highqualitytag", 70);

                if (args.Command == "ping")
                    this.chatBox.OutputTo(player, $"Your ping is {player.Client.Ping}", Color.YellowGreen);


                if (args.Command == "kickme")
                    player.Kick("You has been kicked by slipe");

                if (args.Command == "playerlist")
                {
                    var players = this.elementRepository.GetByType<Player>(ElementType.Player);
                    foreach (var remotePlayer in players)
                        this.chatBox.OutputTo(player, remotePlayer.Name);

                    var text = string.Join('\n', players.Select(x => x.Name));
                    var textItem = this.textItemService.CreateTextItemFor(player, text, Vector2.Zero, 5);
                    Task.Run(async () =>
                    {
                        await Task.Delay(5000);
                        this.textItemService.DeleteTextItemFor(player, textItem);
                    });
                }


                if (args.Command == "increment")
                    player.GetAndIncrementTimeContext();

                if (args.Command == "resendmodpackets")
                    player.ResendModPackets();

                if (args.Command == "ac")
                    player.ResendPlayerACInfo();

                if (args.Command == "setmaxplayers")
                {
                    if (args.Arguments.Length > 0)
                    {
                        if (ushort.TryParse(args.Arguments[0], out ushort slots))
                        {
                            this.server.SetMaxPlayers(slots);
                            this.logger.LogInformation($"Slots has been changed to: {slots}");
                        }
                    }
                }

                if (args.Command == "vehicle")
                {
                    if (args.Arguments.Length > 0)
                    {
                        if (ushort.TryParse(args.Arguments[0], out ushort model))
                        {
                            var vehicle = (new Vehicle(model, player.Position)).AssociateWith(this.server);
                            player.WarpIntoVehicle(vehicle);
                        }
                    }
                }

                if (args.Command == "changeskin")
                {
                    if (args.Arguments.Length > 0)
                    {
                        if (ushort.TryParse(args.Arguments[0], out ushort model))
                        {
                            player.Model = model;
                        }
                    } else
                    {
                        player.Model = (ushort)this.random.Next(20, 25);
                    }

                    if (args.Command == "togglecontrol")
                        player.Controls.JumpEnabled = !player.Controls.JumpEnabled;
                }
                if (args.Command == "jp" || args.Command == "jetpack")
                    player.HasJetpack = !player.HasJetpack;

                if (args.Command == "landinggear")
                    this.Aircraft!.IsLandingGearDown = !this.Aircraft!.IsLandingGearDown;

                if (args.Command == "turret")
                {
                    Task.Run(async () =>
                    {
                        while (true)
                        {
                            await Task.Delay(30);
                            this.Rhino!.TurretRotation = new Vector2(-MathF.Atan2(this.Rhino.Position.X - player.Position.X, this.Rhino.Position.Y - player.Position.Y) + MathF.PI, 0);
                        }
                    });
                }

                if (args.Command == "marker")
                {
                    var typeValues = Enum.GetValues(typeof(MarkerType));
                    MarkerType? randomMarkerType = (MarkerType?)typeValues.GetValue(this.random.Next(typeValues.Length));
                    var iconValues = Enum.GetValues(typeof(MarkerIcon));
                    MarkerIcon? randomMarkerIcon = (MarkerIcon?)iconValues.GetValue(this.random.Next(iconValues.Length));

                    this.Marker!.Color = Color.FromArgb(this.random.Next(0, 255), this.random.Next(0, 255), this.random.Next(0, 255), this.random.Next(0, 255));
                    this.Marker!.Size = this.random.Next(1, 10) / 10.0f + 1.0f;
                    if (randomMarkerType.HasValue)
                        this.Marker!.MarkerType = randomMarkerType.Value;
                    if (randomMarkerIcon.HasValue)
                        this.Marker!.MarkerIcon = randomMarkerIcon.Value;

                    this.Marker!.TargetPosition = new Vector3(this.random.Next(0, 20) - 5, this.random.Next(0, 20), this.random.Next(-50, 50));
                    this.chatBox.OutputTo(player, "You have randomized marker!", Color.YellowGreen);
                }
              
                if (args.Command == "camerainterior")
                {
                    if (args.Arguments.Length > 0)
                    {
                        if (byte.TryParse(args.Arguments[0], out byte interior))
                        {
                            player.Camera.Interior = interior;
                            this.logger.LogInformation($"Camera interior changed to: {interior}");
                        }
                    }
                }

                if (args.Command == "watercolor")
                {
                    this.worldService.SetWaterColor(Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
                    this.chatBox.OutputTo(player, "You have randomized water color!", Color.YellowGreen);
                }
            };

            player.AcInfoReceived += (o, args) =>
            {
                this.logger.LogInformation($"ACInfo for {player.Name} detectedACList:{string.Join(",", args.DetectedACList)} d3d9Size: {args.D3D9Size} d3d9SHA256: {args.D3D9SHA256}");
            };

            player.DiagnosticInfoReceived += (o, args) =>
            {
                this.logger.LogInformation($"DIAGNOSTIC: {player.Name} #{args.Level} {args.Message}");
            };

            player.ModInfoReceived += (o, args) =>
            {
                this.logger.LogInformation($"Player: {player.Name} ModInfo:");
                foreach (var item in args.ModInfoItems)
                {
                    this.logger.LogInformation($"\t{item.Name} - md5: {item.LongMd5}");
                }
            };

            player.NetworkStatusReceived += (o, args) =>
            {
                switch (args.PlayerNetworkStatus)
                {
                    case Packets.Enums.PlayerNetworkStatusType.InterruptionBegan:
                        this.logger.LogInformation($"(packets from {o.Name}) interruption began {args.Ticks} ticks ago");
                        break;
                    case Packets.Enums.PlayerNetworkStatusType.InterruptionEnd:
                        this.logger.LogInformation($"(packets from {o.Name}) interruption began {args.Ticks} ticks ago and has just ended");
                        break;
                }
            };
        }

        private void HandlePlayerScreenshot(object? o, Server.Elements.Events.ScreenshotEventArgs e)
        {
            if (e.Stream != null)
            {
                using FileStream file = new FileStream($"screenshot_${e.Tag}.jpg", FileMode.Create, FileAccess.Write);
                e.Stream.CopyTo(file);
            } else
            {
                Player? player = (Player?)o;
                this.logger.LogWarning($"Failed to take a screenshot ({e.Tag}) of player: {player?.Name}, reason: {e.ErrorMessage}");
            }
        }

        private void TriggerTestEvent(Player player)
        {
            var table = new LuaValue(new Dictionary<LuaValue, LuaValue>()
            {
                ["x"] = 5.5f,
                ["y"] = "string",
                ["z"] = new LuaValue(new Dictionary<LuaValue, LuaValue>() { }),
                ["w"] = false
            });
            table.TableValue?.Add("self", table);

            this.luaService.TriggerEvent(player, "Slipe.Test.ClientEvent", this.root, "String value", true, 23, table);
        }
    }
}
