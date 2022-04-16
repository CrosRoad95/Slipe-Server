-- Generated by CSharp.lua Compiler
local System = System
System.namespace("SlipeLua.Client.Browsers.Events", function (namespace)
  namespace.class("OnResourceBlockedEventArgs", function (namespace)
    local getUrl, getDomain, getReason, __ctor__
    __ctor__ = function (this, url, domain, reason)
      setUrl(this, System.cast(System.String, url))
      setDomain(this, System.cast(System.String, domain))
      setReason(this, System.cast(System.Int32, reason))
    end
    getUrl = System.property("Url", true)
    getDomain = System.property("Domain", true)
    getReason = System.property("Reason", true)
    return {
      getUrl = getUrl,
      getDomain = getDomain,
      Reason = 0,
      getReason = getReason,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          properties = {
            { "Domain", 0x206, System.String, getDomain },
            { "Reason", 0x206, System.Int32, getReason },
            { "Url", 0x206, System.String, getUrl }
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
