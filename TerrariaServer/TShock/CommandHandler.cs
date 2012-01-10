using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TShock
{
	public delegate void CommandDelegate(CommandArgs args);

	public class CommandArgs : EventArgs
	{
		public string Message { get; private set; }
		public IPlayer Player { get; private set; }

		/// <summary>
		/// Parameters passed to the arguement. Does not include the command name.
		/// IE '/kick "jerk face"' will only have 1 argument
		/// </summary>
		public List<string> Parameters { get; private set; }

		public IPlayer TPlayer
		{
			get { return Player; }
		}

		public CommandArgs(string message, IPlayer ply, List<string> args)
		{
			Message = message;
			Player = ply;
			Parameters = args;
		}
	}

	public class Command
	{
		public string Name
		{
			get { return Names[0]; }
		}

		public List<string> Names { get; protected set; }
		public bool DoLog { get; set; }
		public string Permission { get; protected set; }
		private CommandDelegate command;

		public Command(string permissionneeded, CommandDelegate cmd, params string[] names)
			: this(cmd, names)
		{
			Permission = permissionneeded;
		}

		public Command(CommandDelegate cmd, params string[] names)
		{
			if (names == null || names.Length < 1)
				throw new NotSupportedException();
			Permission = null;
			Names = new List<string>(names);
			command = cmd;
			DoLog = true;
		}

		public bool Run(string msg, IPlayer ply, List<string> parms)
		{
			/*
			if (!ply.Group.HasPermission(Permission))
				return false;*/

			try
			{
				command(new CommandArgs(msg, ply, parms));
			}
			catch (Exception e)
			{
				ply.SendMessage("Command failed, check logs for more details.");
				return false;
				//Log.Error(e.ToString());
			}

			return true;
		}

		public bool HasAlias(string name)
		{
			return Names.Contains(name);
		}

		public bool CanRun(IPlayer ply)
		{
			throw new NotImplementedException();
		}
	}

	public static class Commands
	{
		public static List<Command> ChatCommands = new List<Command>();

		public delegate void AddChatCommand(string permission, CommandDelegate command, params string[] names);

		public static AddChatCommand Add = (p, c, n) => ChatCommands.Add(new Command(p, c, n));

		public static bool HandleCommand(IPlayer player, string text)
		{
			string cmdText = text.Remove(0, 1);

			var args = ParseParameters(cmdText);
			if (args.Count < 1)
				return false;

			string cmdName = args[0];
			args.RemoveAt(0);

			Command cmd = ChatCommands.FirstOrDefault(c => c.HasAlias(cmdName));

			if (cmd == null)
			{
				player.SendMessage("Invalid Command Entered. Type /help for a list of valid Commands.", Color.Red);
				return true;
			}

			if (cmd.DoLog)
				Console.WriteLine(string.Format("{0} executed: /{1}", player.Name, cmdText));
			return cmd.Run(cmdText, player, args);
		}

		/// <summary>
		/// Parses a string of parameters into a list. Handles quotes.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		private static List<String> ParseParameters(string str)
		{
			var ret = new List<string>();
			var sb = new StringBuilder();
			bool instr = false;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];

				if (instr)
				{
					if (c == '\\')
					{
						if (i + 1 >= str.Length)
							break;
						c = GetEscape(str[++i]);
					}
					else if (c == '"')
					{
						ret.Add(sb.ToString());
						sb.Clear();
						instr = false;
						continue;
					}
					sb.Append(c);
				}
				else
				{
					if (IsWhiteSpace(c))
					{
						if (sb.Length > 0)
						{
							ret.Add(sb.ToString());
							sb.Clear();
						}
					}
					else if (c == '"')
					{
						if (sb.Length > 0)
						{
							ret.Add(sb.ToString());
							sb.Clear();
						}
						instr = true;
					}
					else
					{
						sb.Append(c);
					}
				}
			}
			if (sb.Length > 0)
				ret.Add(sb.ToString());

			return ret;
		}

		private static char GetEscape(char c)
		{
			switch (c)
			{
				case '\\':
					return '\\';
				case '"':
					return '"';
				case 't':
					return '\t';
				default:
					return c;
			}
		}

		private static bool IsWhiteSpace(char c)
		{
			return c == ' ' || c == '\t' || c == '\n';
		}
	}
}
