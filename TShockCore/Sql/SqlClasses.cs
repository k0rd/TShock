using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace TShockCore.Sql
{
	public class SqlValue
	{
		public string Name { get; set; }
		public object Value { get; set; }

		public SqlValue(string name, object value)
		{
			Name = name;
			Value = value;
		}
	}

	public class SqlTable
	{
		public List<SqlColumn> Columns { get; protected set; }
		public string Name { get; protected set; }

		public SqlTable(string name, params SqlColumn[] columns)
			: this(name, new List<SqlColumn>(columns))
		{
		}

		public SqlTable(string name, List<SqlColumn> columns)
		{
			Name = name;
			Columns = columns;
		}
	}

	public class SqlColumn
	{
		//Required
		public string Name { get; set; }
		public MySqlDbType Type { get; set; }

		//Optional
		public bool Unique { get; set; }
		public bool Primary { get; set; }
		public bool AutoIncrement { get; set; }
		public bool NotNull { get; set; }
		public string DefaultValue { get; set; }

		/// <summary>
		/// Length of the data type, null = default
		/// </summary>
		public int? Length { get; set; }

		public SqlColumn(string name, MySqlDbType type)
			: this(name, type, null)
		{
		}

		public SqlColumn(string name, MySqlDbType type, int? length)
		{
			Name = name;
			Type = type;
			Length = length;
		}
	}
}
