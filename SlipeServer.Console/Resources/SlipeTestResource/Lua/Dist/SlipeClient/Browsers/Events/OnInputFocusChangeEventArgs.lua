-- Generated by CSharp.lua Compiler
local System = System
System.namespace("SlipeLua.Client.Browsers.Events", function (namespace)
  namespace.class("OnInputFocusChangeEventArgs", function (namespace)
    local getDidGainFocus, __ctor__
    __ctor__ = function (this, gainedFocus)
      setDidGainFocus(this, System.cast(System.Boolean, gainedFocus))
    end
    getDidGainFocus = System.property("DidGainFocus", true)
    return {
      DidGainFocus = false,
      getDidGainFocus = getDidGainFocus,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          properties = {
            { "DidGainFocus", 0x206, System.Boolean, getDidGainFocus }
          },
          methods = {
            { ".ctor", 0x104, nil, System.Object }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
