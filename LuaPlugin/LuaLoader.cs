using System;
using System.IO;
using System.ComponentModel;
using LuaInterface;
using TShock;
using TShock.Hooks;

namespace LuaPlugin
{
	internal class LuaLoader
	{
		private readonly Lua _lua = null;
		public string LuaPath = "";
		public string LuaAutorunPath = "";
		readonly IGame _game;
		readonly IHooks _hooks;
		public LuaLoader(string path, IGame game, IHooks hooks)
		{
			_lua = new Lua();
			_game = game;
			_hooks = hooks;
			LuaPath = path;
			LuaAutorunPath = Path.Combine(LuaPath, "autorun");
			SendLuaDebugMsg("Lua 5.1 (serverside) initialized.");

			if (!string.IsNullOrEmpty(LuaPath) && !Directory.Exists(LuaPath))
			{
				Directory.CreateDirectory(LuaPath);
			}
			if (!string.IsNullOrEmpty(LuaAutorunPath) && !Directory.Exists(LuaAutorunPath))
			{
				Directory.CreateDirectory(LuaAutorunPath);
			}

			RegisterLuaFunctions();
			LoadServerAutoruns();
		}

		public void LoadServerAutoruns()
		{
			try
			{
				foreach (string s in Directory.GetFiles(LuaAutorunPath))
				{
					SendLuaDebugMsg("Loading: " + s);
					RunLuaFile(s);
				}
			}
			catch (Exception e)
			{
				SendLuaDebugMsg(e.Message);
				SendLuaDebugMsg(e.StackTrace);
			}
		}

		public void RunLuaString(string s)
		{
			try
			{
				_lua.DoString(s);
			}
			catch (Exception e)
			{
				SendLuaDebugMsg(e.Message);
			}
		}

		public void RunLuaFile(string s)
		{
			try
			{
				_lua.DoFile(s);
			}
			catch (LuaException e)
			{
				SendLuaDebugMsg(e.Message);
			}
		}

		public void SendLuaDebugMsg(string s)
		{
			ConsoleColor previousColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Lua: " + s);
			Console.ForegroundColor = previousColor;
		}

		public void RegisterLuaFunctions()
		{
			_lua["Hooks"] = _hooks;
			_lua["Game"] = _game;
			_lua["Color"] = new Color();			

			//More Lua Functions
			var luaFuncs = new LuaFunctions(this);
			var luaFuncMethods = luaFuncs.GetType().GetMethods();
			foreach (var method in luaFuncMethods)
			{
				_lua.RegisterFunction(method.Name, luaFuncs, method);
			}
		}
	}

	internal class LuaFunctions
	{
		LuaLoader _parent;
		public LuaFunctions(LuaLoader parent)
		{
			_parent = parent;
		}

		[Description("Prints a message to the console from the Lua debugger.")]
		public void Print(string s)
		{
			var previousColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine(s);
			Console.ForegroundColor = previousColor;
		}
	}
}
