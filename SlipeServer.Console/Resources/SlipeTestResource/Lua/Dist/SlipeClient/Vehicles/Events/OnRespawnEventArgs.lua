-- Generated by CSharp.lua Compiler
local System = System
System.namespace("SlipeLua.Client.Vehicles.Events", function (namespace)
  namespace.class("OnRespawnEventArgs", function (namespace)
    local getExploded, __ctor__
    __ctor__ = function (this, b)
      setExploded(this, System.cast(System.Boolean, b))
    end
    getExploded = System.property("Exploded", true)
    return {
      Exploded = false,
      getExploded = getExploded,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          properties = {
            { "Exploded", 0x206, System.Boolean, getExploded }
          },
          methods = {
            { ".ctor", 0x104, nil, System.Object }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
