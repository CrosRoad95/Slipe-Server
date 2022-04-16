-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaClientGui
System.import(function (out)
  SlipeLuaClientGui = SlipeLua.Client.Gui
end)
System.namespace("SlipeLua.Client.Gui", function (namespace)
  --/ <summary>
  --/ Represents an editable Gui element
  --/ </summary>
  namespace.class("EditableGuiElement", function (namespace)
    local addOnChanged, removeOnChanged, __ctor__
    __ctor__ = function (this, element)
      SlipeLuaClientGui.GuiElement.__ctor__(this, element)
    end
    addOnChanged, removeOnChanged = System.event("OnChanged")
    return {
      base = function (out)
        return {
          out.SlipeLua.Client.Gui.GuiElement
        }
      end,
      addOnChanged = addOnChanged,
      removeOnChanged = removeOnChanged,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          methods = {
            { ".ctor", 0x106, nil, out.SlipeLua.MtaDefinitions.MtaElement }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
