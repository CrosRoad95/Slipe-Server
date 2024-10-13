outputChatBox("This is a running Lua file")

triggerServerEvent("Slipe.Test.Event", root, "String value", true, 123, {
	x = 5.5,
	y = "string",
	z = {},
	w = false,
	me = localPlayer
})

addEvent("Slipe.Test.ClientEvent", true)
addEventHandler("Slipe.Test.ClientEvent", root, function(...) 
	for k, v in pairs({...}) do
		iprint(k, v)
	end

	triggerServerEvent("Slipe.Test.SampleEvent", localPlayer, {
		Float = 0.5,
		Double = 2.5,
		Integer = 1,
		OptionalInteger = 0,
		Text = "Bob",
		Boolean = true,
		Position = { X = 1.5, Y = 2.5, Z = 3.5 },
		SubValue = {
			Header = "Header text",
			Messages = {
				"Message 1",
				"Message 2"
			}
		}
	})
end)

outputChatBox("Event ready");

addCommandHandler("crun", function(command, ...)
	outputChatBox("Running code")
	local code = table.concat({ ... }, " ")
	local result = loadstring(code)()
	outputChatBox("Result: " .. tostring(result))
end)

addCommandHandler("exporttest", function(command, ...)
	exports.MetaXmlTestResource:exportMeBaby()
end)

setDevelopmentMode(true)

addCommandHandler("TriggerTests", function(command, ...)
    triggerServerEvent("Test1", resourceRoot, "Test1")
    triggerServerEvent("Test2", resourceRoot, "Test2")
end)

setFPSLimit(60)


addCommandHandler("triggerLatent", function(command, ...)
	outputChatBox("Sending latent event...")
	triggerLatentServerEvent("exampleLatentTrigger",5000,false,root,string.rep("a", 10000))
end)

addCommandHandler("mysyncers", function(command)
	outputChatBox("Your syncers:")
	local countPeds = 0;
	local countVehicles = 0;
	for i,v in ipairs(getElementsByType("ped"))do
		if(isElementSyncer(v))then
			outputChatBox(" Ped: "..getElementModel(v));
			countPeds = countPeds + 1;
		end
	end

	for i,v in ipairs(getElementsByType("vehicle"))do
		if(isElementSyncer(v))then
			outputChatBox(" Vehicle: "..getElementModel(v))
			countVehicles = countVehicles + 1;
		end
	end
	outputChatBox("Peds: "..countPeds..", Vehicles: "..countVehicles)
end)

function findClosestByType(x,y,z, type)
	local distance = 9999999;
	local element = nil;
	for i,v in ipairs(getElementsByType(type))do
		if(v ~= localPlayer)then
			local dis = getDistanceBetweenPoints3D(x,y,z, getElementPosition(v));
			if(distance > dis)then
				element = v;
				distance = dis;
			end
		end
	end
	return element;
end


addCommandHandler("mysyncersclose", function(command)
	local x,y,z = getElementPosition(localPlayer)
	local ped = findClosestByType(x,y,z, "ped")
	local vehicle = findClosestByType(x,y,z, "vehicle")
	outputChatBox("Your closests syncers:")
	outputChatBox(" Ped: "..tostring(isElementSyncer(ped)));
	outputChatBox(" Vehicle: "..tostring(isElementSyncer(vehicle)));
end)

addCommandHandler("pedenter", function(command)
	local x,y,z = getElementPosition(localPlayer)
	local ped = findClosestByType(x,y,z, "ped")
	local vehicle = findClosestByType(x,y,z, "vehicle")
	local result = setPedEnterVehicle(ped, vehicle);
	outputChatBox("result: "..tostring(result));
	iprint("ped", ped, "vehicle", vehicle)
end)