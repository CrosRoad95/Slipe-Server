﻿using SlipeServer.Packets.Definitions.Map;
using SlipeServer.Packets.Definitions.Map.Structs;
using SlipeServer.Packets.Structs;
using SlipeServer.Server.ElementConcepts;
using SlipeServer.Server.Elements;
using SlipeServer.Server.Enums;
using SlipeServer.Server.Extensions;
using SlipeServer.Server.Services;
using System;
using System.Linq;
using System.Numerics;

namespace SlipeServer.Server.Behaviour
{
    public class MapInfoBehaviour
    {
        private readonly GameWorld gameWorld;
        private readonly WeaponConfigurationService weaponConfigurationService;

        public MapInfoBehaviour(MtaServer server, GameWorld gameWorld, WeaponConfigurationService weaponConfigurationService)
        {
            this.gameWorld = gameWorld;
            this.weaponConfigurationService = weaponConfigurationService;

            server.PlayerJoined += HandlePlayerJoin;
        }

        private void HandlePlayerJoin(Player player)
        {
            new MapInfoPacket()
            {
                Weather = this.gameWorld.PreviousWeather,
                WeatherBlendingTo = this.gameWorld.Weather,
                BlendedWeatherHour = this.gameWorld.WeatherBlendStopHour,
                SkyGradient = this.gameWorld.GetSkyGradient(),
                HeatHaze = default,
                Time = this.gameWorld.Time,
                MinuteDuration = this.gameWorld.MinuteDuration,
                Flags = (default, default, this.gameWorld.CloudsEnabled),
                Gravity = this.gameWorld.Gravity,
                GameSpeed = this.gameWorld.GameSpeed,
                WaveHeight = default,
                SeaLevel = default,
                NonSeaLevel = default,
                OutsideWorldSeaLevel = default,
                FpsLimit = default,
                GarageStates = Enum.GetValues<GarageLocation>().Select(x => this.gameWorld.IsGarageOpen(x)).ToArray(),
                BugsEnabled = (
                    this.gameWorld.IsGlitchEnabled(GlitchType.GLITCH_CLOSEDAMAGE),
                    this.gameWorld.IsGlitchEnabled(GlitchType.GLITCH_QUICKRELOAD),
                    this.gameWorld.IsGlitchEnabled(GlitchType.GLITCH_FASTFIRE),
                    this.gameWorld.IsGlitchEnabled(GlitchType.GLITCH_FASTMOVE),
                    this.gameWorld.IsGlitchEnabled(GlitchType.GLITCH_CROUCHBUG),
                    this.gameWorld.IsGlitchEnabled(GlitchType.GLITCH_HITANIM),
                    this.gameWorld.IsGlitchEnabled(GlitchType.GLITCH_FASTSPRINT),
                    this.gameWorld.IsGlitchEnabled(GlitchType.GLITCH_BADDRIVEBYHITBOX),
                    this.gameWorld.IsGlitchEnabled(GlitchType.GLITCH_QUICKSTAND)
                ),
                MaximumJetpackHeight = default,
                WaterColor = default,
                AreInteriorSoundsEnabled = default,
                RainLevel = default,
                MoonSize = default,
                SunSize = this.gameWorld.SunSize,
                SunColor = this.gameWorld.GetSunColor(),
                WindVelocity = this.gameWorld.WindVelocity,
                FarClipDistance = this.gameWorld.FarClipDistance,
                FogDistance = this.gameWorld.FogDistance,
                AircraftMaxHeight = this.gameWorld.AircraftMaxHeight,
                AircraftMaxVelocity = this.gameWorld.AircraftMaxVelocity,
                WeaponProperties = Enum.GetValues<WeaponId>().Select(x => new MapInfoWeaponProperty()
                {
                    WeaponType = (byte)x,
                    EnabledWhenUsingJetpack = false,
                    WeaponConfigurations = Enum.GetValues<WeaponSkillLevel>().Select(skill =>
                    {
                        //return new() { WeaponType = (byte)x };
                        return MapWeaponConfiguration(this.weaponConfigurationService.GetWeaponConfiguration(x, skill));
                    }).ToArray()
                }).ToArray(),
                RemovedWorldModels = Array.Empty<(ushort model, float radius, Vector3 position, byte interior)>(),
                OcclusionsEnabled = this.gameWorld.OcclusionsEnabled,
            }.SendTo(player);
        }

        private MapInfoWeaponConfiguration MapWeaponConfiguration(WeaponConfiguration weaponConfiguration)
        {
            return new MapInfoWeaponConfiguration()
            {
                WeaponType = (int)weaponConfiguration.WeaponType,
                TargetRange = weaponConfiguration.TargetRange,
                WeaponRange = weaponConfiguration.WeaponRange,
                Flags = weaponConfiguration.Flags,
                MaximumClipAmmo = weaponConfiguration.MaximumClipAmmo,
                Damage = weaponConfiguration.Damage,
                Accuracy = weaponConfiguration.Accuracy,
                MoveSpeed = weaponConfiguration.MoveSpeed,
                AnimationLoopStart = weaponConfiguration.AnimationLoopStart,
                AnimationLoopStop = weaponConfiguration.AnimationLoopStop,
                AnimationLoopBulletFire = weaponConfiguration.AnimationLoopBulletFire,
                Animation2LoopStart = weaponConfiguration.Animation2LoopStart,
                Animation2LoopStop = weaponConfiguration.Animation2LoopStop,
                Animation2LoopBulletFire = weaponConfiguration.Animation2LoopBulletFire,
                AnimationBreakoutTime = weaponConfiguration.AnimationBreakoutTime
            };
        }
    }
}
