-- Generated by CSharp.lua Compiler
local System = System
System.namespace("SlipeLua.Client.Peds.Events", function (namespace)
  namespace.class("OnRadioSwitchEventArgs", function (namespace)
    local getRadioStation, __ctor__
    __ctor__ = function (this, station)
      setRadioStation(this, System.cast(System.Int32, station))
    end
    getRadioStation = System.property("RadioStation", true)
    return {
      RadioStation = 0,
      getRadioStation = getRadioStation,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          properties = {
            { "RadioStation", 0x206, System.Int32, getRadioStation }
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
