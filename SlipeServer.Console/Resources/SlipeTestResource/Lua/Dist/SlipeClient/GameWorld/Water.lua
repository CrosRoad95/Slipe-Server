-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaMtaDefinitions = SlipeLua.MtaDefinitions
local SlipeLuaSharedGameWorld = SlipeLua.Shared.GameWorld
System.namespace("SlipeLua.Client.GameWorld", function (namespace)
  --/ <summary>
  --/ Class used to create bodies of water on the map
  --/ </summary>
  namespace.class("Water", function (namespace)
    local getLevel1, setLevel1, __ctor1__, __ctor2__, __ctor3__
    __ctor1__ = function (this, element)
      SlipeLuaSharedGameWorld.SharedWater.__ctor__[1](this, element)
    end
    __ctor2__ = function (this, corner1, corner2, corner3, corner4, shallow)
      SlipeLuaSharedGameWorld.SharedWater.__ctor__[2](this, corner1, corner2, corner3, corner4, shallow)
    end
    __ctor3__ = function (this, corner1, corner2, corner3, shallow)
      SlipeLuaSharedGameWorld.SharedWater.__ctor__[3](this, corner1, corner2, corner3, shallow)
    end
    getLevel1 = function (this)
      return SlipeLuaMtaDefinitions.MtaClient.GetWaterLevel1(this.element)
    end
    setLevel1 = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetWaterLevel(this.element, value)
    end
    return {
      base = function (out)
        return {
          out.SlipeLua.Shared.GameWorld.SharedWater
        }
      end,
      getLevel1 = getLevel1,
      setLevel1 = setLevel1,
      __ctor__ = {
        __ctor1__,
        __ctor2__,
        __ctor3__
      },
      __metadata__ = function (out)
        return {
          properties = {
            { "Level", 0x106, System.Single, getLevel1, setLevel1 }
          },
          methods = {
            { ".ctor", 0x106, __ctor1__, out.SlipeLua.MtaDefinitions.MtaElement },
            { ".ctor", 0x506, __ctor2__, System.Numerics.Vector3, System.Numerics.Vector3, System.Numerics.Vector3, System.Numerics.Vector3, System.Boolean },
            { ".ctor", 0x406, __ctor3__, System.Numerics.Vector3, System.Numerics.Vector3, System.Numerics.Vector3, System.Boolean }
          },
          class = { 0x6, System.new(out.SlipeLua.Shared.Elements.DefaultElementClassAttribute, 2, 34 --[[ElementType.Water]]) }
        }
      end
    }
  end)
end)
