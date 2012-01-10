using System.Collections.Generic;

namespace TShockCore.Sql
{
	public interface IDatabase
	{
		bool CreateTable(SqlTable table);
		bool AlterTable(SqlTable from, SqlTable to);
		bool UpdateValue(string table, List<SqlValue> values, List<SqlValue> wheres);
		bool InsertValues(string table, List<SqlValue> values);
		bool ReadColumn(string table, List<SqlValue> wheres);
		bool DeleteRow(string table, List<SqlValue> wheres);
	}
}
