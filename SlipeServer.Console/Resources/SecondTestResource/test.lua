outputChatBox("I am a second resource")

addCommandHandler("trigger", function(command, ...)
    outputChatBox("triggeredB test1, test2")
    triggerServerEvent("Test1", resourceRoot)
    triggerServerEvent("Test2", resourceRoot)
end)