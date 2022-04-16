-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaSharedIO = SlipeLua.Shared.IO
local SystemNumerics = System.Numerics
System.namespace("SlipeLua.Client.Gui.Events", function (namespace)
  namespace.class("OnDoubleClickEventArgs", function (namespace)
    local getMouseButton, getMouseButtonState, getScreenPosition, __ctor__
    __ctor__ = function (this, mouseButton, buttonState, x, y)
      this.ScreenPosition = System.default(SystemNumerics.Vector2)
      setMouseButton(this, System.cast(System.Int32, System.Enum.Parse(System.typeof(SlipeLuaSharedIO.MouseButton), System.cast(System.String, mouseButton), true)))
      setMouseButtonState(this, System.cast(System.Int32, System.Enum.Parse(System.typeof(SlipeLuaSharedIO.MouseButtonState), System.cast(System.String, buttonState), true)))
      setScreenPosition(this, SystemNumerics.Vector2(System.cast(System.Single, x), System.cast(System.Single, y)))
    end
    getMouseButton = System.property("MouseButton", true)
    getMouseButtonState = System.property("MouseButtonState", true)
    getScreenPosition = System.property("ScreenPosition", true)
    return {
      MouseButton = 0,
      getMouseButton = getMouseButton,
      MouseButtonState = 0,
      getMouseButtonState = getMouseButtonState,
      getScreenPosition = getScreenPosition,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          properties = {
            { "MouseButton", 0x206, System.Int32, getMouseButton },
            { "MouseButtonState", 0x206, System.Int32, getMouseButtonState },
            { "ScreenPosition", 0x206, System.Numerics.Vector2, getScreenPosition }
          },
          methods = {
            { ".ctor", 0x404, nil, System.Object, System.Object, System.Object, System.Object }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
