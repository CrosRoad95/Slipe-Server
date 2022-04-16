-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaSharedElements = SlipeLua.Shared.Elements
local SlipeLuaClientPeds
System.import(function (out)
  SlipeLuaClientPeds = SlipeLua.Client.Peds
end)
System.namespace("SlipeLua.Client.Peds.Events", function (namespace)
  namespace.class("OnStealthKillEventArgs", function (namespace)
    local getVictim, __ctor__
    __ctor__ = function (this, element)
      setVictim(this, SlipeLuaSharedElements.ElementManager.getInstance():GetElement(element, SlipeLuaClientPeds.Ped))
    end
    getVictim = System.property("Victim", true)
    return {
      getVictim = getVictim,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          properties = {
            { "Victim", 0x206, out.SlipeLua.Client.Peds.Ped, getVictim }
          },
          methods = {
            { ".ctor", 0x104, nil, out.SlipeLua.MtaDefinitions.MtaElement }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
