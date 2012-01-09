function OnChat(sender, e)
	tsplr = e.Player;
	Print(tsplr.Name .. " tried to execute: \"" .. e.Text .. "\". (Through Lua!)");
	e.Handled = true;
end

Hooks.PlayerHooks.Chat:Register(OnChat)