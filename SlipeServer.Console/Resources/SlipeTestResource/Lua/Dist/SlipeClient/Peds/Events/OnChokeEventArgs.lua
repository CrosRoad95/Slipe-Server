-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaSharedElements = SlipeLua.Shared.Elements
local SlipeLuaSharedWeapons = SlipeLua.Shared.Weapons
local SlipeLuaClientPeds
System.import(function (out)
  SlipeLuaClientPeds = SlipeLua.Client.Peds
end)
System.namespace("SlipeLua.Client.Peds.Events", function (namespace)
  namespace.class("OnChokeEventArgs", function (namespace)
    local getWeaponModel, getResponsiblePed, __ctor__
    __ctor__ = function (this, model, ped)
      setWeaponModel(this, SlipeLuaSharedWeapons.SharedWeaponModel(System.cast(System.Int32, model)))
      setResponsiblePed(this, SlipeLuaSharedElements.ElementManager.getInstance():GetElement(ped, SlipeLuaClientPeds.Ped))
    end
    getWeaponModel = System.property("WeaponModel", true)
    getResponsiblePed = System.property("ResponsiblePed", true)
    return {
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          properties = {
            { "ResponsiblePed", 0x201, out.SlipeLua.Client.Peds.Ped, getResponsiblePed },
            { "WeaponModel", 0x201, out.SlipeLua.Shared.Weapons.SharedWeaponModel, getWeaponModel }
          },
          methods = {
            { ".ctor", 0x204, nil, System.Object, out.SlipeLua.MtaDefinitions.MtaElement }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
