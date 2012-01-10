using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TShockCore
{
	public interface IConfig
	{
		bool Read();
		bool Write();
		void AddMember(string key, object o);
		object ReadMember(string key);
		void EditMember(string key, object o);
	}

	public class Member
	{
		public string Key;
		public object Item;
	}

	public enum ConfigType
	{
		XML,
		Json
	}
}
