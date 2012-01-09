function OnGreet(sender, e)
	e.Player:SendMessage("Welcome! Lua is working!", Color.Blue)
end

Hooks.PlayerHooks.Greet:Register(OnGreet)