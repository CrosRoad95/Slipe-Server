﻿using SlipeServer.Server.Enums;
using System;

namespace SlipeServer.Server.Elements.Events
{
    public class WeaponReceivedEventArgs : EventArgs
    {
        public Ped Ped { get; set; }
        public WeaponId WeaponId { get; }
        public ushort AmmoCount { get; }
        public bool SetAsCurrent { get; }

        public WeaponReceivedEventArgs(Ped ped, WeaponId weaponId, ushort ammoCount, bool setAsCurrent)
        {
            this.Ped = ped;
            this.WeaponId = weaponId;
            this.AmmoCount = ammoCount;
            this.SetAsCurrent = setAsCurrent;
        }
    }
}
