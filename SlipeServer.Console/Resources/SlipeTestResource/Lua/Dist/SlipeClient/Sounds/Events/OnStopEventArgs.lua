-- Generated by CSharp.lua Compiler
local System = System
System.namespace("SlipeLua.Client.Sounds.Events", function (namespace)
  namespace.class("OnStopEventArgs", function (namespace)
    local getReason, __ctor__
    __ctor__ = function (this, r)
      setReason(this, System.cast(System.String, r))
    end
    getReason = System.property("Reason", true)
    return {
      getReason = getReason,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          properties = {
            { "Reason", 0x206, System.String, getReason }
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
