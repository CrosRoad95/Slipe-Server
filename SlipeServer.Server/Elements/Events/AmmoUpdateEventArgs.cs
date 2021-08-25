using SlipeServer.Server.Enums;
using System;

namespace SlipeServer.Server.Elements.Events
{
    public class AmmoUpdateEventArgs : ElementEventArgs<Ped>
    {
        public WeaponId WeaponId { get; }
        public ushort AmmoCount { get; }
        public ushort? AmmoInClipCount { get; }

        public AmmoUpdateEventArgs(Ped ped, WeaponId weaponId, ushort ammoCount, ushort? ammoInClipCount) : base(ped)
        {
            this.WeaponId = weaponId;
            this.AmmoCount = ammoCount;
            this.AmmoInClipCount = ammoInClipCount;
        }
    }
}
