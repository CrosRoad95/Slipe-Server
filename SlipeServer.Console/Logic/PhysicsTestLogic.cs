﻿using BepuPhysics.Collidables;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using SlipeServer.Physics.Entities;
using SlipeServer.Physics.Services;
using SlipeServer.Physics.Worlds;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Extensions;
using SlipeServer.Server.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SlipeServer.Console.Logic
{
    public class PhysicsTestLogic
    {
        private readonly MtaServer server;
        private readonly ILogger logger;
        private readonly PhysicsWorld physicsWorld;

        private StaticPhysicsElement? ufoInnMesh1;
        private StaticPhysicsElement? ufoInnMesh2;
        private StaticPhysicsElement? army;
        private ConvexPhysicsMesh cylinder;
        private ConvexPhysicsMesh ball;

        public PhysicsTestLogic(MtaServer server, PhysicsService physicsService, CommandService commandService, ILogger logger)
        {
            this.server = server;
            this.logger = logger;

            string? gtaDirectory = GetGtasaDirectory();

            this.physicsWorld = physicsService.CreateEmptyPhysicsWorld(new Vector3(0, 0, -1f));
            //this.physicsWorld = physicsService.CreatePhysicsWorldFromGtaDirectory(gtaDirectory ?? "gtasa", "gta.dat", builderAction: (builder) =>
            //{
            //    builder.SetGravity(Vector3.UnitZ * -1.0f);
            //});

            server.PlayerJoined += HandlePlayerJoin;
            commandService.AddCommand("ray").Triggered += HandleRayCommand;
            commandService.AddCommand("rayme").Triggered += HandleRayMeCommand;
            commandService.AddCommand("ball").Triggered += HandleBallCommand;
            commandService.AddCommand("startsim").Triggered += HandleStartSimCommand;
            commandService.AddCommand("stopsim").Triggered += HandleStopSimCommand;

            Init();
            GenerateRaycastedImage(new Vector3(50, 0, 3));
        }

        private void HandlePlayerJoin(Player player)
        {
            var playerElement = this.physicsWorld.AddKinematicBody(this.cylinder, player.Position, player.Rotation.ToQuaternion());
            playerElement.CoupleWith(player, Vector3.Zero, new Vector3(0, 90, 0));

            player.Disconnected += (_, _) => this.physicsWorld.Destroy(playerElement);
        }

        private void Init()
        {
            var img = this.physicsWorld.LoadImg(Path.Join(GetGtasaDirectory(), @"models\gta3.img"));
            var ufoInnMeshes = this.physicsWorld.CreateMesh(img, "countn2_20.col", "des_ufoinn");
            this.ufoInnMesh1 = (StaticPhysicsElement)this.physicsWorld.AddStatic(ufoInnMeshes.Item1!, Vector3.Zero, Quaternion.Identity);
            this.ufoInnMesh2 = (StaticPhysicsElement)this.physicsWorld.AddStatic(ufoInnMeshes.Item2!, Vector3.Zero, Quaternion.Identity);

            var inn = new WorldObject(Server.Enums.ObjectModel.Desufoinn, new Vector3(50, 0, 4.5f))
            {
                Rotation = new Vector3(0, 0, 90)
            }.AssociateWith(this.server);
            this.ufoInnMesh1.CoupleWith(inn);
            this.ufoInnMesh2.CoupleWith(inn);

            var armyMesh = this.physicsWorld.CreateMesh(img, "army.dff");
            var armyRotation = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), -0.5f * MathF.PI);
            this.army = (StaticPhysicsElement)this.physicsWorld.AddStatic(armyMesh, new Vector3(54, -22.5f, 1), armyRotation);

            this.cylinder = this.physicsWorld.CreateCylinder(0.35f, 1.8f);
            this.ball = this.physicsWorld.CreateSphere(0.25f);
        }

        private void HandleRayCommand(object? sender, Server.Events.CommandTriggeredEventArgs e)
        {
            GenerateRaycastedImage(new Vector3(50, 0, 3));
        }

        private void HandleRayMeCommand(object? sender, Server.Events.CommandTriggeredEventArgs e)
        {
            GenerateRaycastedImage(e.Player.Position);
        }

        private void HandleBallCommand(object? sender, Server.Events.CommandTriggeredEventArgs e)
        {
            var physicsBall = this.physicsWorld.AddDynamicBody(this.ball, e.Player.Position, Quaternion.Identity, 1);
            var ball = new WorldObject(2114, e.Player.Position + Vector3.UnitZ * 2).AssociateWith(this.server);
            physicsBall.CoupleWith(ball);
        }

        private void HandleStartSimCommand(object? sender, Server.Events.CommandTriggeredEventArgs e)
        {
            this.physicsWorld.Start(5);
        }

        private void HandleStopSimCommand(object? sender, Server.Events.CommandTriggeredEventArgs e)
        {
            this.physicsWorld.Stop();
        }

        private void GenerateRaycastedImage(Vector3 position)
        {
            static IEnumerable<Color> GetColors()
            {
                yield return Color.Red;
                yield return Color.Green;
                yield return Color.Blue;

                var random = new Random();
                while (true)
                {
                    yield return Color.FromArgb(255, (byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255));
                }
            }

            var width = 1200;
            var height = 1200;
            var depth = 50f;
            var pixelSize = 0.025f;

            var direction = new Vector3(0, 1, 0);

            var colors = GetColors().GetEnumerator();
            var colorPerCollidable = new Dictionary<CollidableReference, Color>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Bitmap output = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var from = position + new Vector3(x * pixelSize - 0.5f * width * pixelSize, -25, y * pixelSize - 0.5f * width * pixelSize);
                    var hit = this.physicsWorld.RayCast(from, direction, depth);
                    if (hit.HasValue)
                    {
                        var intensity = 255 - (byte)(hit.Value.distance / depth * 255);
                        if (!colorPerCollidable.ContainsKey(hit.Value.Collidable))
                        {
                            colors.MoveNext();
                            colorPerCollidable[hit.Value.Collidable] = colors.Current;
                        }
                        var color = Color.FromArgb(intensity, colorPerCollidable[hit.Value.Collidable]);
                        output.SetPixel(x, height - y - 1, color);
                    }
                }
            }
            stopwatch.Stop();
            var time = stopwatch.Elapsed;
            this.logger.LogInformation($"Raycast image generated in {time.TotalMilliseconds}ms");

            output.Save("rayresult.png", ImageFormat.Png);
        }

        private string? GetGtasaDirectory()
        {
            string? gtaDirectory = Environment.GetEnvironmentVariable("Slipe.GtaSAPath");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && gtaDirectory == null)
            {
                using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Multi Theft Auto: San Andreas All\Common");
                gtaDirectory = key?.GetValue("GTA:SA Path")?.ToString();
            }

            return gtaDirectory;
        }
    }
}
