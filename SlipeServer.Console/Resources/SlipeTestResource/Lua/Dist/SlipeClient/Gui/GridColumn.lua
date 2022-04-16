-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaMtaDefinitions = SlipeLua.MtaDefinitions
System.namespace("SlipeLua.Client.Gui", function (namespace)
  --/ <summary>
  --/ Represents a column in a grid list
  --/ </summary>
  namespace.class("GridColumn", function (namespace)
    local getID, getTitle, setTitle, getAbsoluteWidth, setAbsoluteWidth, getRelativeWidth, setRelativeWidth, AutoSize, 
    __ctor__
    __ctor__ = function (this, title, width, gridList)
      setID(this, SlipeLuaMtaDefinitions.MtaClient.GuiGridListAddColumn(gridList:getMTAElement(), title, width))
      this.glist = gridList
    end
    getID = System.property("ID", true)
    getTitle = function (this)
      return SlipeLuaMtaDefinitions.MtaClient.GuiGridListGetColumnTitle(this.glist:getMTAElement(), getID(this))
    end
    setTitle = function (this, value)
      SlipeLuaMtaDefinitions.MtaClient.GuiGridListSetColumnTitle(this.glist:getMTAElement(), getID(this), value)
    end
    getAbsoluteWidth = function (this)
      return SlipeLuaMtaDefinitions.MtaClient.GuiGridListGetColumnWidth(this.glist:getMTAElement(), getID(this), false)
    end
    setAbsoluteWidth = function (this, value)
      SlipeLuaMtaDefinitions.MtaClient.GuiGridListSetColumnWidth(this.glist:getMTAElement(), getID(this), value, false)
    end
    getRelativeWidth = function (this)
      return SlipeLuaMtaDefinitions.MtaClient.GuiGridListGetColumnWidth(this.glist:getMTAElement(), getID(this), true)
    end
    setRelativeWidth = function (this, value)
      SlipeLuaMtaDefinitions.MtaClient.GuiGridListSetColumnWidth(this.glist:getMTAElement(), getID(this), value, true)
    end
    AutoSize = function (this)
      return SlipeLuaMtaDefinitions.MtaClient.GuiGridListAutoSizeColumn(this.glist:getMTAElement(), getID(this))
    end
    return {
      ID = 0,
      getID = getID,
      getTitle = getTitle,
      setTitle = setTitle,
      getAbsoluteWidth = getAbsoluteWidth,
      setAbsoluteWidth = setAbsoluteWidth,
      getRelativeWidth = getRelativeWidth,
      setRelativeWidth = setRelativeWidth,
      AutoSize = AutoSize,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          fields = {
            { "glist", 0x1, out.SlipeLua.Client.Gui.GridList }
          },
          properties = {
            { "AbsoluteWidth", 0x106, System.Single, getAbsoluteWidth, setAbsoluteWidth },
            { "ID", 0x206, System.Int32, getID },
            { "RelativeWidth", 0x106, System.Single, getRelativeWidth, setRelativeWidth },
            { "Title", 0x106, System.String, getTitle, setTitle }
          },
          methods = {
            { ".ctor", 0x306, nil, System.String, System.Single, out.SlipeLua.Client.Gui.GridList },
            { "AutoSize", 0x86, AutoSize, System.Boolean }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
