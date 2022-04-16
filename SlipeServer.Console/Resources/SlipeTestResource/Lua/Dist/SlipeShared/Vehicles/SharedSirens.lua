-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaMtaDefinitions = SlipeLua.MtaDefinitions
local SystemNumerics = System.Numerics
local DictStringSingle = System.Dictionary(System.String, System.Single)
local SlipeLuaSharedUtilities
local SlipeLuaSharedVehicles
local ArraySiren
System.import(function (out)
  SlipeLuaSharedUtilities = SlipeLua.Shared.Utilities
  SlipeLuaSharedVehicles = SlipeLua.Shared.Vehicles
  ArraySiren = System.Array(SlipeLuaSharedVehicles.Siren)
end)
System.namespace("SlipeLua.Shared.Vehicles", function (namespace)
  --/ <summary>
  --/ Represents the set of all sirens on a vehicle
  --/ </summary>
  namespace.class("SharedSirens", function (namespace)
    local getType, getVisibleFromAllDirections, getCheckLineOfSight, getUseRandomiser, getSilent, getOn, setOn, getAll, 
    UpdateParams, __ctor__
    __ctor__ = function (this, vehicle)
      this.vehicle = vehicle
      UpdateParams(this)
    end
    getType = function (this)
      return this.type
    end
    getVisibleFromAllDirections = function (this)
      return this.visibleFromAllDirection
    end
    getCheckLineOfSight = function (this)
      return this.checkLOS
    end
    getUseRandomiser = function (this)
      return this.useRandomiser
    end
    getSilent = function (this)
      return this.silent
    end
    getOn = function (this)
      return SlipeLuaMtaDefinitions.MtaShared.GetVehicleSirensOn(this.vehicle:getMTAElement())
    end
    setOn = function (this, value)
      -- This is due to an MTA bug not turning on silent sirens that are off
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleSirensOn(this.vehicle:getMTAElement(), false)
      SlipeLuaMtaDefinitions.MtaShared.SetVehicleSirensOn(this.vehicle:getMTAElement(), value)
    end
    getAll = function (this)
      local ar = SlipeLuaMtaDefinitions.MtaShared.GetArrayFromTable(SlipeLuaMtaDefinitions.MtaShared.GetVehicleSirens(this.vehicle:getMTAElement()), "dynamic", T)
      local result = ArraySiren(#ar)
      for i = 0, #ar - 1 do
        local d = System.cast(DictStringSingle, SlipeLuaMtaDefinitions.MtaShared.GetDictionaryFromTable(ar:get(i), "System.String", "System.Single"))
        result:set(i, SlipeLuaSharedVehicles.Siren(this.vehicle, i + 1, SystemNumerics.Vector3(d:get("x"), d:get("y"), d:get("z")), System.new(SlipeLuaSharedUtilities.Color, 3, System.ToByte(d:get("Red")), System.ToByte(d:get("Green")), System.ToByte(d:get("Blue")), System.ToByte(d:get("Alpha"))), d:get("Min_Alpha"), false))
      end
      return result
    end
    UpdateParams = function (this)
      local d = SlipeLuaMtaDefinitions.MtaShared.GetDictionaryFromTable(SlipeLuaMtaDefinitions.MtaShared.GetVehicleSirenParams(this.vehicle:getMTAElement()), "System.String", "dynamic")
      this.type = System.cast(System.Int32, d:get("SirenType"))
      local flags = SlipeLuaMtaDefinitions.MtaShared.GetDictionaryFromTable(d:get("Flags"), "System.String", "System.Boolean")
      this.visibleFromAllDirection = flags:get("360")
      this.checkLOS = flags:get("DoLOSCheck")
      this.useRandomiser = flags:get("UseRandomiser")
      this.silent = flags:get("Silent")
    end
    return {
      type = 0,
      getType = getType,
      visibleFromAllDirection = false,
      getVisibleFromAllDirections = getVisibleFromAllDirections,
      checkLOS = false,
      getCheckLineOfSight = getCheckLineOfSight,
      useRandomiser = false,
      getUseRandomiser = getUseRandomiser,
      silent = false,
      getSilent = getSilent,
      getOn = getOn,
      setOn = setOn,
      getAll = getAll,
      UpdateParams = UpdateParams,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          fields = {
            { "checkLOS", 0x3, System.Boolean },
            { "silent", 0x3, System.Boolean },
            { "type", 0x3, System.Int32 },
            { "useRandomiser", 0x3, System.Boolean },
            { "vehicle", 0x3, out.SlipeLua.Shared.Vehicles.SharedVehicle },
            { "visibleFromAllDirection", 0x3, System.Boolean }
          },
          properties = {
            { "All", 0x206, System.Array(out.SlipeLua.Shared.Vehicles.Siren), getAll },
            { "CheckLineOfSight", 0x206, System.Boolean, getCheckLineOfSight },
            { "On", 0x106, System.Boolean, getOn, setOn },
            { "Silent", 0x206, System.Boolean, getSilent },
            { "Type", 0x206, System.Int32, getType },
            { "UseRandomiser", 0x206, System.Boolean, getUseRandomiser },
            { "VisibleFromAllDirections", 0x206, System.Boolean, getVisibleFromAllDirections }
          },
          methods = {
            { ".ctor", 0x106, nil, out.SlipeLua.Shared.Vehicles.SharedVehicle },
            { "UpdateParams", 0x3, UpdateParams }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
