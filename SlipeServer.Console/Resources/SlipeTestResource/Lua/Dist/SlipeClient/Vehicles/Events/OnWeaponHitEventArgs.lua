-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaSharedElements = SlipeLua.Shared.Elements
local SystemNumerics = System.Numerics
System.namespace("SlipeLua.Client.Vehicles.Events", function (namespace)
  namespace.class("OnWeaponHitEventArgs", function (namespace)
    local getWeaponType, getHitElement, getPosition, getMaterial, getModel, __ctor__
    __ctor__ = function (this, weaponType, hitElement, x, y, z, hitModel, materialId)
      this.Position = System.default(SystemNumerics.Vector3)
      setWeaponType(this, System.cast(System.Int32, weaponType))
      setHitElement(this, SlipeLuaSharedElements.ElementManager.getInstance():GetElement(hitElement, SlipeLuaSharedElements.PhysicalElement))
      setPosition(this, SystemNumerics.Vector3(System.cast(System.Single, x), System.cast(System.Single, y), System.cast(System.Single, z)))
      setMaterial(this, System.cast(System.Int32, materialId))
      setModel(this, System.cast(System.Int32, hitModel))
    end
    getWeaponType = System.property("WeaponType", true)
    getHitElement = System.property("HitElement", true)
    getPosition = System.property("Position", true)
    getMaterial = System.property("Material", true)
    getModel = System.property("Model", true)
    return {
      WeaponType = 0,
      getWeaponType = getWeaponType,
      getHitElement = getHitElement,
      getPosition = getPosition,
      Material = 0,
      getMaterial = getMaterial,
      Model = 0,
      getModel = getModel,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          properties = {
            { "HitElement", 0x206, out.SlipeLua.Shared.Elements.PhysicalElement, getHitElement },
            { "Material", 0x206, System.Int32, getMaterial },
            { "Model", 0x206, System.Int32, getModel },
            { "Position", 0x206, System.Numerics.Vector3, getPosition },
            { "WeaponType", 0x206, System.Int32, getWeaponType }
          },
          methods = {
            { ".ctor", 0x704, nil, System.Object, System.Object, System.Object, System.Object, System.Object, System.Object, System.Object }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
