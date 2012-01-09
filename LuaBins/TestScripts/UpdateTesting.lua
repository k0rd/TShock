function OnUpdate(sender, e)
	Print("Update")
	Game.Players[0]:SendMessage("UPDATE", Color.Blue)
	Hooks.GameHooks.Update:UnRegister(OnUpdate)
	Game.Players[0]:SendMessage("Unregistered UPDATE", Color.Red)
end

Hooks.GameHooks.Update:Register(OnUpdate)