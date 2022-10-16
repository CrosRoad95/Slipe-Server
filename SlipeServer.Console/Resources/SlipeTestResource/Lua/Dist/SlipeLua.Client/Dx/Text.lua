-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaMtaDefinitions = SlipeLua.MtaDefinitions
local SlipeLuaSharedUtilities = SlipeLua.Shared.Utilities
local SystemNumerics = System.Numerics
local SlipeLuaClientDx
System.import(function (out)
  SlipeLuaClientDx = SlipeLua.Client.Dx
end)
System.namespace("SlipeLua.Client.Dx", function (namespace)
  --/ <summary>
  --/ Represents a drawable text line
  --/ </summary>
  namespace.class("Text", function (namespace)
    local getContent, setContent, getBottomRight, setBottomRight, getScale, setScale, getCustomFont, setCustomFont, 
    getStandardFont, setStandardFont, getHorizontalAlignment, setHorizontalAlignment, getVerticalAlignment, setVerticalAlignment, getRotation, setRotation, 
    getRotationOrigin, setRotationOrigin, getSubPixelPositioning, setSubPixelPositioning, getClip, setClip, getWordBreak, setWordBreak, 
    getColorCoded, setColorCoded, Draw, GetTextWidth, internal, __ctor1__, __ctor2__, __ctor3__, 
    __ctor4__, __ctor5__, __ctor6__
    internal = function (this)
      this.BottomRight = System.default(SystemNumerics.Vector2)
      this.Scale = System.default(SystemNumerics.Vector2)
      this.RotationOrigin = System.default(SystemNumerics.Vector2)
    end
    __ctor1__ = function (this, text, position, bottomRight, color, scale, font, horizontalAlign, verticalAlign, fRotation, fRotationCenter, clip, wordBreak, postGUI, colorCoded, subPixelPositioning)
      internal(this)
      SlipeLuaClientDx.Dx2DObject.__ctor__(this)
      this.Content = text
      this:setPosition(position:__clone__())
      this.BottomRight = bottomRight:__clone__()
      this.Color = color
      this.Scale = scale:__clone__()
      setCustomFont(this, font)
      this.HorizontalAlignment = horizontalAlign
      this.VerticalAlignment = verticalAlign
      this.Rotation = fRotation
      this.RotationOrigin = fRotationCenter:__clone__()
      this.SubPixelPositioning = subPixelPositioning
      this.PostGUI = postGUI
      this.Clip = clip
      this.WordBreak = wordBreak
      this.ColorCoded = colorCoded
      this.useCustomFont = true
    end
    __ctor2__ = function (this, text, position, bottomRight, color, scale, font, horizontalAlign, verticalAlign, fRotation)
      __ctor1__(this, text, position:__clone__(), bottomRight:__clone__(), color, scale:__clone__(), font, horizontalAlign, verticalAlign, fRotation, SystemNumerics.Vector2.getZero(), false, false, false, false, false)
    end
    __ctor3__ = function (this, text, position, bottomRight, color, scale, font, horizontalAlign, verticalAlign, fRotation, fRotationCenter, clip, wordBreak, postGUI, colorCoded, subPixelPositioning)
      internal(this)
      SlipeLuaClientDx.Dx2DObject.__ctor__(this)
      this.Content = text
      this:setPosition(position:__clone__())
      this.Color = color
      this.Scale = scale:__clone__()
      setStandardFont(this, font)
      this.HorizontalAlignment = horizontalAlign
      this.VerticalAlignment = verticalAlign
      this.Rotation = fRotation
      this.RotationOrigin = fRotationCenter:__clone__()
      this.SubPixelPositioning = subPixelPositioning
      this.PostGUI = postGUI
      this.Clip = clip
      this.WordBreak = wordBreak
      this.ColorCoded = colorCoded
      this.useCustomFont = false
      this.BottomRight = bottomRight:__clone__()
    end
    __ctor4__ = function (this, text, position, bottomRight, color, scale, font, horizontalAlign, verticalAlign, fRotation)
      __ctor3__(this, text, position:__clone__(), bottomRight:__clone__(), color, scale:__clone__(), font, horizontalAlign, verticalAlign, fRotation, SystemNumerics.Vector2.getZero(), false, false, false, false, false)
    end
    __ctor5__ = function (this, text, position, bottomRight, color)
      __ctor4__(this, text, position:__clone__(), bottomRight:__clone__(), color, SystemNumerics.Vector2.getOne(), 0 --[[StandardFont.Default]], 0, 0, 0)
    end
    __ctor6__ = function (this, text, position)
      __ctor5__(this, text, position:__clone__(), SystemNumerics.Vector2.getZero(), SlipeLuaSharedUtilities.Color.getWhite())
    end
    getContent, setContent = System.property("Content")
    getBottomRight, setBottomRight = System.property("BottomRight")
    getScale, setScale = System.property("Scale")
    getCustomFont = function (this)
      return this.customFont
    end
    setCustomFont = function (this, value)
      this.customFont = value
      this.useCustomFont = true
    end
    getStandardFont = function (this)
      return this.standardFont
    end
    setStandardFont = function (this, value)
      this.standardFont = value
      this.useCustomFont = false
    end
    getHorizontalAlignment, setHorizontalAlignment = System.property("HorizontalAlignment")
    getVerticalAlignment, setVerticalAlignment = System.property("VerticalAlignment")
    getRotation, setRotation = System.property("Rotation")
    getRotationOrigin, setRotationOrigin = System.property("RotationOrigin")
    getSubPixelPositioning, setSubPixelPositioning = System.property("SubPixelPositioning")
    getClip, setClip = System.property("Clip")
    getWordBreak, setWordBreak = System.property("WordBreak")
    getColorCoded, setColorCoded = System.property("ColorCoded")
    Draw = function (this, source, eventArgs)
      if this.useCustomFont then
        return SlipeLuaMtaDefinitions.MtaClient.DxDrawText(this.Content, this:getPosition().X, this:getPosition().Y, this.BottomRight:__clone__().X, this.BottomRight:__clone__().Y, this.Color:getHex(), this.Scale:__clone__().X, this.Scale:__clone__().Y, getCustomFont(this):getMTAFont(), this.HorizontalAlignment:EnumToString(SlipeLuaClientDx.HorizontalAlign):ToLower(), this.VerticalAlignment:EnumToString(SlipeLuaClientDx.VerticalAlign):ToLower(), this.Clip, this.WordBreak, this.PostGUI, this.ColorCoded, this.SubPixelPositioning, this.Rotation, this.RotationOrigin:__clone__().X, this.RotationOrigin:__clone__().Y)
      else
        return SlipeLuaMtaDefinitions.MtaClient.DxDrawText(this.Content, this:getPosition().X, this:getPosition().Y, this.BottomRight:__clone__().X, this.BottomRight:__clone__().Y, this.Color:getHex(), this.Scale:__clone__().X, this.Scale:__clone__().Y, getStandardFont(this):EnumToString(SlipeLuaClientDx.StandardFont):ToLower(), this.HorizontalAlignment:EnumToString(SlipeLuaClientDx.HorizontalAlign):ToLower(), this.VerticalAlignment:EnumToString(SlipeLuaClientDx.VerticalAlign):ToLower(), this.Clip, this.WordBreak, this.PostGUI, this.ColorCoded, this.SubPixelPositioning, this.Rotation, this.RotationOrigin:__clone__().X, this.RotationOrigin:__clone__().Y)
      end
    end
    GetTextWidth = function (text, scale, font, colorCoded)
      return SlipeLuaMtaDefinitions.MtaClient.DxGetTextWidth(text, scale, font:EnumToString(SlipeLuaClientDx.StandardFont):ToLower(), colorCoded)
    end
    return {
      base = function (out)
        return {
          out.SlipeLua.Client.Dx.Dx2DObject,
          out.SlipeLua.Client.Dx.IDrawable
        }
      end,
      useCustomFont = false,
      standardFont = 0,
      getContent = getContent,
      setContent = setContent,
      getBottomRight = getBottomRight,
      setBottomRight = setBottomRight,
      getScale = getScale,
      setScale = setScale,
      getCustomFont = getCustomFont,
      setCustomFont = setCustomFont,
      getStandardFont = getStandardFont,
      setStandardFont = setStandardFont,
      HorizontalAlignment = 0,
      getHorizontalAlignment = getHorizontalAlignment,
      setHorizontalAlignment = setHorizontalAlignment,
      VerticalAlignment = 0,
      getVerticalAlignment = getVerticalAlignment,
      setVerticalAlignment = setVerticalAlignment,
      Rotation = 0,
      getRotation = getRotation,
      setRotation = setRotation,
      getRotationOrigin = getRotationOrigin,
      setRotationOrigin = setRotationOrigin,
      SubPixelPositioning = false,
      getSubPixelPositioning = getSubPixelPositioning,
      setSubPixelPositioning = setSubPixelPositioning,
      Clip = false,
      getClip = getClip,
      setClip = setClip,
      WordBreak = false,
      getWordBreak = getWordBreak,
      setWordBreak = setWordBreak,
      ColorCoded = false,
      getColorCoded = getColorCoded,
      setColorCoded = setColorCoded,
      Draw = Draw,
      GetTextWidth = GetTextWidth,
      __ctor__ = {
        __ctor1__,
        __ctor2__,
        __ctor3__,
        __ctor4__,
        __ctor5__,
        __ctor6__
      },
      __metadata__ = function (out)
        return {
          fields = {
            { "customFont", 0x3, out.SlipeLua.Client.Dx.Font },
            { "standardFont", 0x3, System.Int32 },
            { "useCustomFont", 0x3, System.Boolean }
          },
          properties = {
            { "BottomRight", 0x106, System.Numerics.Vector2, getBottomRight, setBottomRight },
            { "Clip", 0x106, System.Boolean, getClip, setClip },
            { "ColorCoded", 0x106, System.Boolean, getColorCoded, setColorCoded },
            { "Content", 0x106, System.String, getContent, setContent },
            { "CustomFont", 0x106, out.SlipeLua.Client.Dx.Font, getCustomFont, setCustomFont },
            { "HorizontalAlignment", 0x106, System.Int32, getHorizontalAlignment, setHorizontalAlignment },
            { "Rotation", 0x106, System.Single, getRotation, setRotation },
            { "RotationOrigin", 0x106, System.Numerics.Vector2, getRotationOrigin, setRotationOrigin },
            { "Scale", 0x106, System.Numerics.Vector2, getScale, setScale },
            { "StandardFont", 0x106, System.Int32, getStandardFont, setStandardFont },
            { "SubPixelPositioning", 0x106, System.Boolean, getSubPixelPositioning, setSubPixelPositioning },
            { "VerticalAlignment", 0x106, System.Int32, getVerticalAlignment, setVerticalAlignment },
            { "WordBreak", 0x106, System.Boolean, getWordBreak, setWordBreak }
          },
          methods = {
            { ".ctor", 0xF06, __ctor1__, System.String, System.Numerics.Vector2, System.Numerics.Vector2, out.SlipeLua.Shared.Utilities.Color, System.Numerics.Vector2, out.SlipeLua.Client.Dx.Font, System.Int32, System.Int32, System.Single, System.Numerics.Vector2, System.Boolean, System.Boolean, System.Boolean, System.Boolean, System.Boolean },
            { ".ctor", 0x906, __ctor2__, System.String, System.Numerics.Vector2, System.Numerics.Vector2, out.SlipeLua.Shared.Utilities.Color, System.Numerics.Vector2, out.SlipeLua.Client.Dx.Font, System.Int32, System.Int32, System.Single },
            { ".ctor", 0xF06, __ctor3__, System.String, System.Numerics.Vector2, System.Numerics.Vector2, out.SlipeLua.Shared.Utilities.Color, System.Numerics.Vector2, System.Int32, System.Int32, System.Int32, System.Single, System.Numerics.Vector2, System.Boolean, System.Boolean, System.Boolean, System.Boolean, System.Boolean },
            { ".ctor", 0x906, __ctor4__, System.String, System.Numerics.Vector2, System.Numerics.Vector2, out.SlipeLua.Shared.Utilities.Color, System.Numerics.Vector2, System.Int32, System.Int32, System.Int32, System.Single },
            { ".ctor", 0x406, __ctor5__, System.String, System.Numerics.Vector2, System.Numerics.Vector2, out.SlipeLua.Shared.Utilities.Color },
            { ".ctor", 0x206, __ctor6__, System.String, System.Numerics.Vector2 },
            { "Draw", 0x286, Draw, out.SlipeLua.Client.Elements.RootElement, out.SlipeLua.Client.Rendering.Events.OnRenderEventArgs, System.Boolean },
            { "GetTextWidth", 0x48E, GetTextWidth, System.String, System.Single, System.Int32, System.Boolean, System.Single }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)