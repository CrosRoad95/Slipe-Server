-- Generated by CSharp.lua Compiler
local System = System
local SystemNumerics = System.Numerics
System.namespace("SlipeLua.Client.Elements.Events", function (namespace)
  namespace.class("OnWorldSoundEventArgs", function (namespace)
    local getGroup, getIndex, getPosition, __ctor__
    __ctor__ = function (this, group, index, x, y, z)
      this.Position = System.default(SystemNumerics.Vector3)
      setGroup(this, System.cast(System.Int32, group))
      setIndex(this, System.cast(System.Int32, index))
      setPosition(this, SystemNumerics.Vector3(System.cast(System.Single, x), System.cast(System.Single, y), System.cast(System.Single, z)))
    end
    getGroup = System.property("Group", true)
    getIndex = System.property("Index", true)
    getPosition = System.property("Position", true)
    return {
      Group = 0,
      getGroup = getGroup,
      Index = 0,
      getIndex = getIndex,
      getPosition = getPosition,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          properties = {
            { "Group", 0x206, System.Int32, getGroup },
            { "Index", 0x206, System.Int32, getIndex },
            { "Position", 0x206, System.Numerics.Vector3, getPosition }
          },
          methods = {
            { ".ctor", 0x504, nil, System.Object, System.Object, System.Object, System.Object, System.Object }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
