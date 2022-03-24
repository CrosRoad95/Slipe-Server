﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SlipeServer.Packets.Definitions.Lua
{
    [DebuggerDisplay("LuaValue")]
    public class LuaValue
    {
        public LuaType LuaType { get; set; }

        public bool? BoolValue { get; }
        public string? StringValue { get; }
        public float? FloatValue { get; }
        public double? DoubleValue { get; }
        public int? IntegerValue { get; }
        public uint? ElementId { get; }
        public Dictionary<LuaValue, LuaValue>? TableValue { get; }
        public bool IsNil { get; }


        public LuaValue()
        {
            this.IsNil = true;
        }

        public LuaValue(bool? value)
        {
            this.LuaType = LuaType.Boolean;
            this.BoolValue = value;
            this.IsNil = value == null;
        }

        public LuaValue(string? value)
        {
            this.LuaType = LuaType.String;
            this.StringValue = value;
            this.IsNil = value == null;
        }

        public LuaValue(float? value)
        {
            this.LuaType = LuaType.Number;
            this.FloatValue = value;
            this.IsNil = value == null;
        }

        public LuaValue(double? value)
        {
            this.LuaType = LuaType.Number;
            this.DoubleValue = value;
            this.IsNil = value == null;
        }

        public LuaValue(int? value)
        {
            this.LuaType = LuaType.Number;
            this.IntegerValue = value;
            this.IsNil = value == null;
        }

        public LuaValue(uint? value)
        {
            this.LuaType = LuaType.Userdata;
            this.ElementId = value;
            this.IsNil = value == null;
        }

        public LuaValue(Dictionary<LuaValue, LuaValue>? value)
        {
            this.LuaType = LuaType.Table;
            this.TableValue = value;
            this.IsNil = value == null;
        }

        public LuaValue(IEnumerable<LuaValue>? value)
        {
            this.LuaType = LuaType.Table;

            if (value != null)
            {
                this.TableValue = new Dictionary<LuaValue, LuaValue>();
                int i = 1;
                foreach (var arrayValue in value)
                    this.TableValue[i++] = arrayValue;
            }

            this.IsNil = value == null;
        }

        public override string ToString()
        {
            if (this.TableValue != null)
                return $"{{{string.Join(", ", this.TableValue.Select(kvPair => $"{kvPair.Key}: {kvPair.Value}"))}}}";

            return
                this.IntegerValue?.ToString() ??
                this.DoubleValue?.ToString() ??
                this.FloatValue?.ToString() ??
                this.BoolValue?.ToString() ??
                this.ElementId?.ToString() ??
                this.StringValue?.ToString() ??
                "nil";
        }

        public static implicit operator LuaValue(uint value) => new(value);
        public static implicit operator LuaValue(string value) => new(value);
        public static implicit operator LuaValue(bool value) => new(value);
        public static implicit operator LuaValue(int value) => new(value);
        public static implicit operator LuaValue(float value) => new(value);
        public static implicit operator LuaValue(double value) => new(value);
        public static implicit operator LuaValue(Dictionary<LuaValue, LuaValue> value) => new(value);
        public static implicit operator LuaValue(LuaValue[] value) => new(value);
    }
}
