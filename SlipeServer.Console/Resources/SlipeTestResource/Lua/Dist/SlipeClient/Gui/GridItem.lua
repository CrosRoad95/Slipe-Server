-- Generated by CSharp.lua Compiler
local System = System
local SlipeLuaMtaDefinitions = SlipeLua.MtaDefinitions
local SlipeLuaSharedUtilities = SlipeLua.Shared.Utilities
System.namespace("SlipeLua.Client.Gui", function (namespace)
  --/ <summary>
  --/ Represents a single item in a Gui gridlist
  --/ </summary>
  namespace.class("GridItem", function (namespace)
    local getColumn, getRow, getColor, setColor, getContent, setContent, getData, setData, 
    SetSection, __ctor__
    __ctor__ = function (this, column, row, gridList)
      setColumn(this, column)
      setRow(this, row)
      this.glist = gridList
    end
    getColumn = System.property("Column", true)
    getRow = System.property("Row", true)
    getColor = function (this)
      local color = SlipeLuaMtaDefinitions.MtaClient.GuiGridListGetItemColor(this.glist:getMTAElement(), getRow(this):getID(), getColumn(this):getID())
      return System.new(SlipeLuaSharedUtilities.Color, 3, System.toByte(color[1]), System.toByte(color[2]), System.toByte(color[3]), System.toByte(color[4]))
    end
    setColor = function (this, value)
      SlipeLuaMtaDefinitions.MtaClient.GuiGridListSetItemColor(this.glist:getMTAElement(), getRow(this):getID(), getColumn(this):getID(), value:getR(), value:getG(), value:getB(), value:getA())
    end
    getContent = function (this)
      return SlipeLuaMtaDefinitions.MtaClient.GuiGridListGetItemText(this.glist:getMTAElement(), getRow(this):getID(), getColumn(this):getID())
    end
    setContent = function (this, value)
      SlipeLuaMtaDefinitions.MtaClient.GuiGridListSetItemText(this.glist:getMTAElement(), getRow(this):getID(), getColumn(this):getID(), value, false, false)
    end
    getData = function (this)
      if System.String.IsNullOrEmpty(getContent(this)) then
        setContent(this, "")
      end
      return SlipeLuaMtaDefinitions.MtaClient.GuiGridListGetItemData(this.glist:getMTAElement(), getRow(this):getID(), getColumn(this):getID())
    end
    setData = function (this, value)
      if System.String.IsNullOrEmpty(getContent(this)) then
        setContent(this, "")
      end
      SlipeLuaMtaDefinitions.MtaClient.GuiGridListSetItemData(this.glist:getMTAElement(), getRow(this):getID(), getColumn(this):getID(), value)
    end
    SetSection = function (this, content)
      return SlipeLuaMtaDefinitions.MtaClient.GuiGridListSetItemText(this.glist:getMTAElement(), getRow(this):getID(), getColumn(this):getID(), content, true, false)
    end
    return {
      getColumn = getColumn,
      getRow = getRow,
      getColor = getColor,
      setColor = setColor,
      getContent = getContent,
      setContent = setContent,
      getData = getData,
      setData = setData,
      SetSection = SetSection,
      __ctor__ = __ctor__,
      __metadata__ = function (out)
        return {
          fields = {
            { "glist", 0x1, out.SlipeLua.Client.Gui.GridList }
          },
          properties = {
            { "Color", 0x106, out.SlipeLua.Shared.Utilities.Color, getColor, setColor },
            { "Column", 0x206, out.SlipeLua.Client.Gui.GridColumn, getColumn },
            { "Content", 0x106, System.String, getContent, setContent },
            { "Data", 0x106, System.Object, getData, setData },
            { "Row", 0x206, out.SlipeLua.Client.Gui.GridRow, getRow }
          },
          methods = {
            { ".ctor", 0x306, nil, out.SlipeLua.Client.Gui.GridColumn, out.SlipeLua.Client.Gui.GridRow, out.SlipeLua.Client.Gui.GridList },
            { "SetSection", 0x186, SetSection, System.String, System.Boolean }
          },
          class = { 0x6 }
        }
      end
    }
  end)
end)
