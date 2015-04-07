namespace CodeDOM.DB
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    internal class TableInfo
    {
        /// <summary>
        /// Catalog of the table.
        /// </summary>
        public string DbName { get; private set; }

        /// <summary>
        /// Schema that contains the table.
        /// </summary>
        public string Schema { get; private set; }

        /// <summary>
        /// Table name.
        /// </summary>
        public string Name { get; private set; }

        public List<ColumnInfo> Columns { get; private set; }

        public static async Task<List<TableInfo>> FetchFromDbAsync(SqlConnection connection)
        {
            var result = new List<TableInfo>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = GetCommandText();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(ParseDataReader(reader));
                    }
                }
            }

            return result;
        }

        private static string GetCommandText()
        {
            return @"SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME
                     FROM INFORMATION_SCHEMA.TABLES
                     WHERE TABLE_TYPE = 'BASE TABLE'";
        }

        private static TableInfo ParseDataReader(SqlDataReader reader)
        {
            return new TableInfo
                   {
                       DbName = reader.GetString(0),
                       Schema = reader.GetString(1),
                       Name = reader.GetString(2),
                   };
        }

        public async Task FetchColumnInfoAsync(SqlConnection connection)
        {
            Columns = await ColumnInfo.GetFromDbAsync(connection, DbName, Schema, Name);
        }
    }
}