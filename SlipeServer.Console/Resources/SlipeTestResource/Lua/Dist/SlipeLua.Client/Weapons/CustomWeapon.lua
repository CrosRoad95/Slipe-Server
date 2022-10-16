-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaMtaDefinitions = SlipeLua.MtaDefinitions
local SlipeLuaSharedElements = SlipeLua.Shared.Elements
local SlipeLuaSharedWeapons = SlipeLua.Shared.Weapons
System.namespace("SlipeLua.Client.Weapons", function (namespace)
  --/ <summary>
  --/ Represents a custom weapon that can be placed in the world
  --/ </summary>
  namespace.class("CustomWeapon", function (namespace)
    local getAmmo, setAmmo, getAmmoInClip, setAmmoInClip, getFiringRate, setFiringRate, getState, setState, 
    Fire, SetTarget, SetTarget1, SetTarget2, SetTarget3, SetTarget4, ResetFiringRate, addOnFire, 
    removeOnFire, __ctor1__, __ctor2__
    __ctor1__ = function (this, element)
      SlipeLuaSharedElements.PhysicalElement.__ctor__(this, element)
    end
    __ctor2__ = function (this, model, position)
      __ctor1__(this, SlipeLuaMtaDefinitions.MtaClient.CreateWeapon(model:getName(), position.X, position.Y, position.Z))
    end
    getAmmo = function (this)
      return SlipeLuaMtaDefinitions.MtaClient.GetWeaponAmmo(this.element)
    end
    setAmmo = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetWeaponAmmo(this.element, value, - 1, - 1)
    end
    getAmmoInClip = function (this)
      return SlipeLuaMtaDefinitions.MtaClient.GetWeaponClipAmmo(this.element)
    end
    setAmmoInClip = function (this, value)
      SlipeLuaMtaDefinitions.MtaClient.SetWeaponClipAmmo(this.element, value)
    end
    getFiringRate = function (this)
      return SlipeLuaMtaDefinitions.MtaClient.GetWeaponFiringRate(this.element)
    end
    setFiringRate = function (this, value)
      SlipeLuaMtaDefinitions.MtaClient.SetWeaponFiringRate(this.element, value)
    end
    getState = function (this)
      return System.cast(System.Int32, System.Enum.Parse(System.typeof(SlipeLuaSharedWeapons.WeaponState), SlipeLuaMtaDefinitions.MtaClient.GetWeaponState(this.element), true))
    end
    setState = function (this, value)
      SlipeLuaMtaDefinitions.MtaClient.SetWeaponState(this.element, value:EnumToString(SlipeLuaSharedWeapons.WeaponState):ToLower())
    end
    Fire = function (this)
      return SlipeLuaMtaDefinitions.MtaClient.FireWeapon(this.element)
    end
    SetTarget = function (this, vehicle, tire)
      return SlipeLuaMtaDefinitions.MtaClient.SetWeaponTarget(this.element, vehicle:getMTAElement(), tire)
    end
    SetTarget1 = function (this, physicalElement)
      return SlipeLuaMtaDefinitions.MtaClient.SetWeaponTarget(this.element, physicalElement:getMTAElement(), 255)
    end
    SetTarget2 = function (this, ped, bone)
      return SlipeLuaMtaDefinitions.MtaClient.SetWeaponTarget(this.element, ped:getMTAElement(), bone)
    end
    SetTarget3 = function (this, position)
      return SlipeLuaMtaDefinitions.MtaClient.SetWeaponTarget(this.element, position.X, position.Y, position.Z)
    end
    SetTarget4 = function (this)
      return SlipeLuaMtaDefinitions.MtaClient.SetWeaponTarget(this.element)
    end
    ResetFiringRate = function (this)
      return SlipeLuaMtaDefinitions.MtaClient.ResetWeaponFiringRate(this.element)
    end
    addOnFire, removeOnFire = System.event("OnFire")
    return {
      base = function (out)
        return {
          out.SlipeLua.Shared.Elements.PhysicalElement
        }
      end,
      getAmmo = getAmmo,
      setAmmo = setAmmo,
      getAmmoInClip = getAmmoInClip,
      setAmmoInClip = setAmmoInClip,
      getFiringRate = getFiringRate,
      setFiringRate = setFiringRate,
      getState = getState,
      setState = setState,
      Fire = Fire,
      SetTarget = SetTarget,
      SetTarget1 = SetTarget1,
      SetTarget2 = SetTarget2,
      SetTarget3 = SetTarget3,
      SetTarget4 = SetTarget4,
      ResetFiringRate = ResetFiringRate,
      addOnFire = addOnFire,
      removeOnFire = removeOnFire,
      __ctor__ = {
        __ctor1__,
        __ctor2__
      },
      __metadata__ = function (out)
        return {
          properties = {
            { "Ammo", 0x106, System.Int32, getAmmo, setAmmo },
            { "AmmoInClip", 0x106, System.Int32, getAmmoInClip, setAmmoInClip },
            { "FiringRate", 0x106, System.Int32, getFiringRate, setFiringRate },
            { "State", 0x106, System.Int32, getState, setState }
          },
          methods = {
            { ".ctor", 0x106, __ctor1__, out.SlipeLua.MtaDefinitions.MtaElement },
            { ".ctor", 0x206, __ctor2__, out.SlipeLua.Shared.Weapons.SharedWeaponModel, System.Numerics.Vector3 },
            { "Fire", 0x86, Fire, System.Boolean },
            { "ResetFiringRate", 0x86, ResetFiringRate, System.Boolean },
            { "SetTarget", 0x286, SetTarget, out.SlipeLua.Client.Vehicles.BaseVehicle, System.Int32, System.Boolean },
            { "SetTarget", 0x186, SetTarget1, out.SlipeLua.Shared.Elements.PhysicalElement, System.Boolean },
            { "SetTarget", 0x286, SetTarget2, out.SlipeLua.Client.Peds.Ped, System.Int32, System.Boolean },
            { "SetTarget", 0x186, SetTarget3, System.Numerics.Vector3, System.Boolean },
            { "SetTarget", 0x86, SetTarget4, System.Boolean }
          },
          class = { 0x6, System.new(out.SlipeLua.Shared.Elements.DefaultElementClassAttribute, 2, 29 --[[ElementType.Weapon]]) }
        }
      end
    }
  end)
end)