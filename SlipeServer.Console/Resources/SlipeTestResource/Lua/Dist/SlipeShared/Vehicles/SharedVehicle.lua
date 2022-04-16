-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaMtaDefinitions = SlipeLua.MtaDefinitions
local DictInt32Int32 = System.Dictionary(System.Int32, System.Int32)
local SlipeLuaSharedElements
local SlipeLuaSharedUtilities
local SlipeLuaSharedVehicles
local ArrayColor
local ArrayInt32
System.import(function (out)
  SlipeLuaSharedElements = SlipeLua.Shared.Elements
  SlipeLuaSharedUtilities = SlipeLua.Shared.Utilities
  SlipeLuaSharedVehicles = SlipeLua.Shared.Vehicles
  ArrayColor = System.Array(SlipeLuaSharedUtilities.Color)
  ArrayInt32 = System.Array(System.Int32)
end)
System.namespace("SlipeLua.Shared.Vehicles", function (namespace)
  --/ <summary>
  --/ Represents a vehicle in the GTA world
  --/ </summary>
  namespace.class("SharedVehicle", function (namespace)
    local getPrimaryColor, setPrimaryColor, getSecondaryColor, setSecondaryColor, getColors, setColors, getHeadLightColor, setHeadLightColor, 
    getName, getMaxPassengers, getEngineRunning, setEngineRunning, getHandling, getOverrideLights, setOverrideLights, getPaintjob, 
    setPaintjob, getPlateText, setPlateText, getSirens, getVehicleType, getUpgrades, getIsBlown, getDamageProof, 
    setDamageProof, getFuelTankExplodable, setFuelTankExplodable, getLocked, setLocked, getIsOnGround, setDoorsUndamagable, getVariant, 
    getWheelState, setWheelState, getVehicleTowedByThis, Fix, DetachTowedVehicle, DetachAnyTowedVehicle, AddUpgrade, GetCompatibleUpgrades, 
    GetCompatibleUpgrades1, GetUpgradeOnSlot, RemoveUpgrade, GetDoorOpenRatio, SetDoorOpenRatio, GetDoorState, SetDoorState, GetLightState, 
    SetLightState, GetPanelDamage, SetPanelDamage, class, __ctor__
    __ctor__ = function (this, element)
      SlipeLuaSharedElements.PhysicalElement.__ctor__(this, element)
    end
    getPrimaryColor = function (this)
      local r = SlipeLuaMtaDefinitions.MtaShared.GetVehicleColor(this.element, true)
      return System.new(SlipeLuaSharedUtilities.Color, 4, System.toByte(r[1]), System.toByte(r[2]), System.toByte(r[3]))
    end
    setPrimaryColor = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleColor(this.element, value:getR(), value:getG(), value:getB(), getSecondaryColor(this):getR(), getSecondaryColor(this):getG(), getSecondaryColor(this):getB(), 0, 0, 0, 0, 0, 0)
    end
    getSecondaryColor = function (this)
      local r = SlipeLuaMtaDefinitions.MtaShared.GetVehicleColor(this.element, true)
      return System.new(SlipeLuaSharedUtilities.Color, 4, System.toByte(r[4]), System.toByte(r[5]), System.toByte(r[6]))
    end
    setSecondaryColor = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleColor(this.element, getPrimaryColor(this):getR(), getPrimaryColor(this):getG(), getPrimaryColor(this):getB(), value:getR(), value:getG(), value:getB(), 0, 0, 0, 0, 0, 0)
    end
    getColors = function (this)
      local r = SlipeLuaMtaDefinitions.MtaShared.GetVehicleColor(this.element, true)
      local c = ArrayColor(4)
      c:set(0, System.new(SlipeLuaSharedUtilities.Color, 4, System.toByte(r[1]), System.toByte(r[2]), System.toByte(r[3])))
      c:set(1, System.new(SlipeLuaSharedUtilities.Color, 4, System.toByte(r[4]), System.toByte(r[5]), System.toByte(r[6])))
      c:set(2, System.new(SlipeLuaSharedUtilities.Color, 4, System.toByte(r[7]), System.toByte(r:getRest()[1]), System.toByte(r:getRest()[2])))
      c:set(3, System.new(SlipeLuaSharedUtilities.Color, 4, System.toByte(r:getRest()[3]), System.toByte(r:getRest()[4]), System.toByte(r:getRest()[5])))
      return c
    end
    setColors = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleColor(this.element, value:get(0):getR(), value:get(0):getG(), value:get(0):getB(), value:get(1):getR(), value:get(1):getG(), value:get(1):getB(), value:get(2):getR(), value:get(2):getG(), value:get(2):getB(), value:get(3):getR(), value:get(3):getG(), value:get(3):getB())
    end
    getHeadLightColor = function (this)
      local r = SlipeLuaMtaDefinitions.MtaShared.GetVehicleHeadLightColor(this.element)
      return System.new(SlipeLuaSharedUtilities.Color, 4, System.toByte(r[1]), System.toByte(r[2]), System.toByte(r[3]))
    end
    setHeadLightColor = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleHeadLightColor(this.element, value:getR(), value:getG(), value:getB())
    end
    getName = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehicleName(this.element)
    end
    getMaxPassengers = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehicleMaxPassengers(this.element)
    end
    getEngineRunning = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehicleEngineState(this.element)
    end
    setEngineRunning = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleEngineState(this.element, value)
    end
    getHandling = function (this)
      if this.handling == nil then
        this.handling = System.new(SlipeLuaSharedVehicles.Handling, 2, this)
      end
      return this.handling
    end
    getOverrideLights = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehicleOverrideLights(this.element)
    end
    setOverrideLights = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleOverrideLights(this.element, value)
    end
    getPaintjob = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehiclePaintjob(this.element)
    end
    setPaintjob = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehiclePaintjob(this.element, value)
    end
    getPlateText = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehiclePlateText(this.element)
    end
    setPlateText = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehiclePlateText(this.element, value)
    end
    getSirens = function (this)
      if this.sirens == nil then
        this.sirens = SlipeLuaSharedVehicles.SharedSirens(this)
      end
      return this.sirens
    end
    getVehicleType = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehicleType(this.element)
    end
    getUpgrades = function (this)
      local d = System.cast(DictInt32Int32, SlipeLuaMtaDefinitions.MtaShared.GetDictionaryFromTable(SlipeLuaMtaDefinitions.MtaShared.GetVehicleUpgrades(this.element), "System.Int32", "System.Int32"))
      local r = DictInt32Int32()
      for _, upgrade in System.each(d) do
        r:AddKeyValue(upgrade[1], upgrade[2])
      end
      return r
    end
    getIsBlown = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.IsVehicleBlown(this.element)
    end
    getDamageProof = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.IsVehicleDamageProof(this.element)
    end
    setDamageProof = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleDamageProof(this.element, value)
    end
    getFuelTankExplodable = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.IsVehicleFuelTankExplodable(this.element)
    end
    setFuelTankExplodable = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleFuelTankExplodable(this.element, value)
    end
    getLocked = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.IsVehicleLocked(this.element)
    end
    setLocked = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleLocked(this.element, value)
    end
    getIsOnGround = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.IsVehicleOnGround(this.element)
    end
    setDoorsUndamagable = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleDoorsUndamageable(this.element, value)
    end
    getVariant = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehicleVariant(this.element)
    end
    getWheelState = function (this)
      local states = SlipeLuaMtaDefinitions.MtaShared.GetVehicleWheelStates(this.element)
      return System.Tuple(states[1], states[2], states[3], states[4])
    end
    setWheelState = function (this, value)
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleWheelStates(this.element, value[1], value[2], value[3], value[4])
    end
    getVehicleTowedByThis = function (this)
      return SlipeLuaSharedElements.ElementManager.getInstance():GetElement(SlipeLuaMtaDefinitions.MtaShared.GetVehicleTowedByVehicle(this.element), class)
    end
    Fix = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.FixVehicle(this.element)
    end
    DetachTowedVehicle = function (this, attachedVehicle)
      return SlipeLuaMtaDefinitions.MtaShared.DetachTrailerFromVehicle(this.element, attachedVehicle:getMTAElement())
    end
    DetachAnyTowedVehicle = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.DetachTrailerFromVehicle(this.element)
    end
    AddUpgrade = function (this, upgrade)
      return SlipeLuaMtaDefinitions.MtaShared.AddVehicleUpgrade(this.element, upgrade)
    end
    GetCompatibleUpgrades = function (this, slot)
      local upInts = SlipeLuaMtaDefinitions.MtaShared.GetArrayFromTable(SlipeLuaMtaDefinitions.MtaShared.GetVehicleCompatibleUpgrades(this.element, slot), "System.Int32", T)
      local upgrades = ArrayInt32(#upInts)
      for i = 0, #upInts - 1 do
        upgrades:set(i, upInts:get(i))
      end
      return upgrades
    end
    GetCompatibleUpgrades1 = function (this)
      local upInts = SlipeLuaMtaDefinitions.MtaShared.GetArrayFromTable(SlipeLuaMtaDefinitions.MtaShared.GetVehicleCompatibleUpgrades(this.element, - 1), "System.Int32", T)
      local upgrades = ArrayInt32(#upInts)
      for i = 0, #upInts - 1 do
        upgrades:set(i, upInts:get(i))
      end
      return upgrades
    end
    GetUpgradeOnSlot = function (this, slot)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehicleUpgradeOnSlot(this.element, slot)
    end
    RemoveUpgrade = function (this, upgrade)
      return SlipeLuaMtaDefinitions.MtaShared.RemoveVehicleUpgrade(this.element, upgrade)
    end
    GetDoorOpenRatio = function (this, door)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehicleDoorOpenRatio(this.element, door)
    end
    SetDoorOpenRatio = function (this, door, ratio, time)
      return SlipeLuaMtaDefinitions.MtaShared.SetVehicleDoorOpenRatio(this.element, door, ratio, time)
    end
    GetDoorState = function (this, door)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehicleDoorState(this.element, door)
    end
    SetDoorState = function (this, door, state)
      return SlipeLuaMtaDefinitions.MtaShared.SetVehicleDoorState(this.element, door, state)
    end
    GetLightState = function (this, light)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehicleLightState(this.element, light)
    end
    SetLightState = function (this, light, state)
      return SlipeLuaMtaDefinitions.MtaShared.SetVehicleLightState(this.element, light, state)
    end
    GetPanelDamage = function (this, panel)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehiclePanelState(this.element, panel)
    end
    SetPanelDamage = function (this, panel, damage)
      return SlipeLuaMtaDefinitions.MtaShared.SetVehiclePanelState(this.element, panel, damage)
    end
    class = {
      base = function (out)
        return {
          out.SlipeLua.Shared.Elements.PhysicalElement
        }
      end,
      getPrimaryColor = getPrimaryColor,
      setPrimaryColor = setPrimaryColor,
      getSecondaryColor = getSecondaryColor,
      setSecondaryColor = setSecondaryColor,
      getColors = getColors,
      setColors = setColors,
      getHeadLightColor = getHeadLightColor,
      setHeadLightColor = setHeadLightColor,
      getName = getName,
      getMaxPassengers = getMaxPassengers,
      getEngineRunning = getEngineRunning,
      setEngineRunning = setEngineRunning,
      getHandling = getHandling,
      getOverrideLights = getOverrideLights,
      setOverrideLights = setOverrideLights,
      getPaintjob = getPaintjob,
      setPaintjob = setPaintjob,
      getPlateText = getPlateText,
      setPlateText = setPlateText,
      getSirens = getSirens,
      getVehicleType = getVehicleType,
      getUpgrades = getUpgrades,
      getIsBlown = getIsBlown,
      getDamageProof = getDamageProof,
      setDamageProof = setDamageProof,
      getFuelTankExplodable = getFuelTankExplodable,
      setFuelTankExplodable = setFuelTankExplodable,
      getLocked = getLocked,
      setLocked = setLocked,
      getIsOnGround = getIsOnGround,
      setDoorsUndamagable = setDoorsUndamagable,
      getVariant = getVariant,
      getWheelState = getWheelState,
      setWheelState = setWheelState,
      getVehicleTowedByThis = getVehicleTowedByThis,
      Fix = Fix,
      DetachTowedVehicle = DetachTowedVehicle,
      DetachAnyTowedVehicle = DetachAnyTowedVehicle,
      AddUpgrade = AddUpgrade,
      GetCompatibleUpgrades = GetCompatibleUpgrades,
      GetCompatibleUpgrades1 = GetCompatibleUpgrades1,
      GetUpgradeOnSlot = GetUpgradeOnSlot,
      RemoveUpgrade = RemoveUpgrade,
      GetDoorOpenRatio = GetDoorOpenRatio,
      SetDoorOpenRatio = SetDoorOpenRatio,
      GetDoorState = GetDoorState,
      SetDoorState = SetDoorState,
      GetLightState = GetLightState,
      SetLightState = SetLightState,
      GetPanelDamage = GetPanelDamage,
      SetPanelDamage = SetPanelDamage,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          fields = {
            { "handling", 0x1, out.SlipeLua.Shared.Vehicles.Handling },
            { "sirens", 0x1, out.SlipeLua.Shared.Vehicles.SharedSirens }
          },
          properties = {
            { "Colors", 0x106, System.Array(out.SlipeLua.Shared.Utilities.Color), getColors, setColors },
            { "DamageProof", 0x106, System.Boolean, getDamageProof, setDamageProof },
            { "DoorsUndamagable", 0x306, System.Boolean, setDoorsUndamagable },
            { "EngineRunning", 0x106, System.Boolean, getEngineRunning, setEngineRunning },
            { "FuelTankExplodable", 0x106, System.Boolean, getFuelTankExplodable, setFuelTankExplodable },
            { "Handling", 0x206, out.SlipeLua.Shared.Vehicles.Handling, getHandling },
            { "HeadLightColor", 0x106, out.SlipeLua.Shared.Utilities.Color, getHeadLightColor, setHeadLightColor },
            { "IsBlown", 0x206, System.Boolean, getIsBlown },
            { "IsOnGround", 0x206, System.Boolean, getIsOnGround },
            { "Locked", 0x106, System.Boolean, getLocked, setLocked },
            { "MaxPassengers", 0x206, System.Int32, getMaxPassengers },
            { "Name", 0x206, System.String, getName },
            { "OverrideLights", 0x106, System.Int32, getOverrideLights, setOverrideLights },
            { "Paintjob", 0x106, System.Int32, getPaintjob, setPaintjob },
            { "PlateText", 0x106, System.String, getPlateText, setPlateText },
            { "PrimaryColor", 0x106, out.SlipeLua.Shared.Utilities.Color, getPrimaryColor, setPrimaryColor },
            { "SecondaryColor", 0x106, out.SlipeLua.Shared.Utilities.Color, getSecondaryColor, setSecondaryColor },
            { "Sirens", 0x206, out.SlipeLua.Shared.Vehicles.SharedSirens, getSirens },
            { "Upgrades", 0x206, System.Dictionary(System.Int32, System.Int32), getUpgrades },
            { "Variant", 0x206, System.Tuple, getVariant },
            { "VehicleTowedByThis", 0x206, class, getVehicleTowedByThis },
            { "VehicleType", 0x206, System.String, getVehicleType },
            { "WheelState", 0x106, System.Tuple, getWheelState, setWheelState }
          },
          methods = {
            { ".ctor", 0x106, nil, out.SlipeLua.MtaDefinitions.MtaElement },
            { "AddUpgrade", 0x186, AddUpgrade, System.Int32, System.Boolean },
            { "DetachAnyTowedVehicle", 0x86, DetachAnyTowedVehicle, System.Boolean },
            { "DetachTowedVehicle", 0x186, DetachTowedVehicle, class, System.Boolean },
            { "Fix", 0x86, Fix, System.Boolean },
            { "GetCompatibleUpgrades", 0x186, GetCompatibleUpgrades, System.Int32, System.Array(System.Int32) },
            { "GetCompatibleUpgrades", 0x86, GetCompatibleUpgrades1, System.Array(System.Int32) },
            { "GetDoorOpenRatio", 0x186, GetDoorOpenRatio, System.Int32, System.Single },
            { "GetDoorState", 0x186, GetDoorState, System.Int32, System.Int32 },
            { "GetLightState", 0x186, GetLightState, System.Int32, System.Int32 },
            { "GetPanelDamage", 0x186, GetPanelDamage, System.Int32, System.Int32 },
            { "GetUpgradeOnSlot", 0x186, GetUpgradeOnSlot, System.Int32, System.Int32 },
            { "RemoveUpgrade", 0x186, RemoveUpgrade, System.Int32, System.Boolean },
            { "SetDoorOpenRatio", 0x386, SetDoorOpenRatio, System.Int32, System.Single, System.Int32, System.Boolean },
            { "SetDoorState", 0x286, SetDoorState, System.Int32, System.Int32, System.Boolean },
            { "SetLightState", 0x286, SetLightState, System.Int32, System.Int32, System.Boolean },
            { "SetPanelDamage", 0x286, SetPanelDamage, System.Int32, System.Int32, System.Boolean }
          },
          class = { 0x6 }
        }
      end
    }
    return class
  end)
end)
