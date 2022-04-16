-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaSharedIO = SlipeLua.Shared.IO
local SystemNumerics = System.Numerics
System.namespace("SlipeLua.Client.Gui.Events", function (namespace)
  namespace.class("OnMouseDownEventArgs", function (namespace)
    local getMouseButton, getScreenPosition, __ctor__
    __ctor__ = function (this, mouseButton, x, y)
      this.ScreenPosition = System.default(SystemNumerics.Vector2)
      setMouseButton(this, System.cast(System.Int32, System.Enum.Parse(System.typeof(SlipeLuaSharedIO.MouseButton), System.cast(System.String, mouseButton), true)))
      setScreenPosition(this, SystemNumerics.Vector2(System.cast(System.Single, x), System.cast(System.Single, y)))
    end
    getMouseButton = System.property("MouseButton", true)
    getScreenPosition = System.property("ScreenPosition", true)
    return {
      MouseButton = 0,
      getMouseButton = getMouseButton,
      getScreenPosition = getScreenPosition,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          properties = {
            { "MouseButton", 0x206, System.Int32, getMouseButton },
            { "ScreenPosition", 0x206, System.Numerics.Vector2, getScreenPosition }
          },
          methods = {
            { ".ctor", 0x304, nil, System.Object, System.Object, System.Object }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
