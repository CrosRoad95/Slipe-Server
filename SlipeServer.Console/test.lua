﻿local object = createObject(321, 5, 5, 5)
setElementPosition(object, 50, 50, 250)
setElementRotation(object, 180, 180, 90)

local object2 = createObject(321, 10, 10, 10)

addEventHandler("onElementDestroyed", object, function() print('OBJECT ELEMENT DESTROYED') end)
addEventHandler("onElementDestroyed", root, function() print('ANY ELEMENT DESTROYED') end)

print(type(object), getElementType(object), object)
print(type(root), getElementType(root), root)

setElementPosition(object, 1337, 2337, 3337)
local x, y, z = getElementPosition(object)
print(x, y, z)

destroyElement(object2)
destroyElement(object)

outputDebugString("Debug message")