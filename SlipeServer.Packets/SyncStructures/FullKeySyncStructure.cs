﻿using SlipeServer.Packets.Builder;
using SlipeServer.Packets.Reader;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Xml;

namespace SlipeServer.Packets.Structures
{
    public class FullKeySyncStructure : ISyncStructure
    {
        public bool LeftShoulder1 { get; set; }
        public bool RightShoulder1 { get; set; }
        public bool ButtonSquare { get; set; }
        public bool ButtonCross { get; set; }
        public bool ButtonCircle { get; set; }
        public bool ButtonTriangle { get; set; }
        public bool ShockButton { get; set; }
        public bool PedWalk { get; set; }

        public byte ButtonSquareByte { get; set; }
        public byte ButtonCrossByte { get; set; }

        public Vector2 LeftStick { get; set; }


        public FullKeySyncStructure()
        {

        }

        public FullKeySyncStructure(
            Vector2 leftStick,
            bool leftShoulder1,
            bool leftShoulder2,
            bool buttonSquare,
            bool buttonCross,
            bool buttonCircle,
            bool buttonTriangle,
            bool shockButton,
            bool pedWalk,
            byte buttonSquareByte,
            byte buttonCrossByte
        )
        {
            this.LeftStick = leftStick;
            this.LeftShoulder1 = leftShoulder1;
            this.RightShoulder1 = leftShoulder2;
            this.ButtonSquare = buttonSquare;
            this.ButtonCross = buttonCross;
            this.ButtonCircle = buttonCircle;
            this.ButtonTriangle = buttonTriangle;
            this.ShockButton = shockButton;
            this.PedWalk = pedWalk;
            this.ButtonSquareByte = buttonSquareByte;
            this.ButtonCrossByte = buttonCrossByte;
        }

        public void Read(PacketReader reader)
        {
            this.PedWalk = reader.GetBit();
            this.ShockButton = reader.GetBit();
            this.ButtonTriangle = reader.GetBit();
            this.ButtonCircle = reader.GetBit();
            this.ButtonCross = reader.GetBit();
            this.ButtonSquare = reader.GetBit();
            this.RightShoulder1 = reader.GetBit();
            this.LeftShoulder1 = reader.GetBit();

            this.ButtonSquareByte = reader.GetBit() ? reader.GetByte() : (byte)0;
            this.ButtonCrossByte = reader.GetBit() ? reader.GetByte() : (byte)0;

            this.LeftStick = new Vector2(
                (float)(reader.GetByte() * 128.0f / 127.0f),
                (float)(reader.GetByte() * 128.0f / 127.0f)
            );

        }

        public void Write(PacketBuilder builder)
        {
            builder.Write(new bool[] {
                this.PedWalk,
                this.ShockButton,
                this.ButtonTriangle,
                this.ButtonCircle,
                this.ButtonCross,
                this.ButtonSquare,
                this.RightShoulder1,
                this.LeftShoulder1,
            });


            if (this.ButtonSquareByte >= 1 && this.ButtonSquareByte <= 254)
            {
                builder.Write(true);
                builder.Write(this.ButtonSquareByte);
            }
            else
                builder.Write(false);

            if (this.ButtonCrossByte >= 1 && this.ButtonCrossByte <= 254)
            {
                builder.Write(true);
                builder.Write(this.ButtonCrossByte);
            }
            else
                builder.Write(false);

            builder.Write(new byte[]
            {
                (byte)((float)this.LeftStick.X * 127.0f / 128.0f),
                (byte)((float)this.LeftStick.Y * 127.0f / 128.0f),
            });
        }
    }
}
