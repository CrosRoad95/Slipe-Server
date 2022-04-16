-- Generated by CSharp.lua Compiler
local System = System
System.namespace("SlipeLua.Shared.Peds", function (namespace)
  --/ <summary>
  --/ Represents different HUD components
  --/ </summary>
  namespace.enum("HudComponent", function ()
    return {
      all = 0,
      ammo = 1,
      area_name = 2,
      armour = 3,
      breath = 4,
      clock = 5,
      health = 6,
      money = 7,
      radar = 8,
      vehicle_name = 9,
      weapon = 10,
      radio = 11,
      wanted = 12,
      crosshair = 13,
      __metadata__ = function (out)
        return {
          fields = {
            { "all", 0xE, System.Int32 },
            { "ammo", 0xE, System.Int32 },
            { "area_name", 0xE, System.Int32 },
            { "armour", 0xE, System.Int32 },
            { "breath", 0xE, System.Int32 },
            { "clock", 0xE, System.Int32 },
            { "crosshair", 0xE, System.Int32 },
            { "health", 0xE, System.Int32 },
            { "money", 0xE, System.Int32 },
            { "radar", 0xE, System.Int32 },
            { "radio", 0xE, System.Int32 },
            { "vehicle_name", 0xE, System.Int32 },
            { "wanted", 0xE, System.Int32 },
            { "weapon", 0xE, System.Int32 }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
