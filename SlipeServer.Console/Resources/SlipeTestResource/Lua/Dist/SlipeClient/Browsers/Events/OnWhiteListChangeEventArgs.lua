-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaMtaDefinitions = SlipeLua.MtaDefinitions
System.namespace("SlipeLua.Client.Browsers.Events", function (namespace)
  namespace.class("OnWhiteListChangeEventArgs", function (namespace)
    local getChangedDomains, __ctor__
    __ctor__ = function (this, list)
      setChangedDomains(this, SlipeLuaMtaDefinitions.MtaShared.GetArrayFromTable(list, "System.String", T))
    end
    getChangedDomains = System.property("ChangedDomains", true)
    return {
      getChangedDomains = getChangedDomains,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          properties = {
            { "ChangedDomains", 0x206, System.Array(System.String), getChangedDomains }
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
