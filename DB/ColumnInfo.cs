namespace CodeDOM.DB
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    internal class ColumnInfo
    {
        public string Name { get; set; }
        public bool IsNullable { get; set; }
        public string DataType { get; set; }

        public static async Task<List<ColumnInfo>> GetFromDbAsync(SqlConnection connection, string dbName, string schema, string tableName)
        {
            var result = new List<ColumnInfo>();

            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = GetCommandText();

                command.Parameters.AddWithValue("@DbName", dbName);
                command.Parameters.AddWithValue("@Schema", schema);
                command.Parameters.AddWithValue("@TableName", tableName);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(ParseDbReader(reader));
                    }
                }
            }

            return result;
        }

        private static string GetCommandText()
        {
            return @"SELECT COLUMN_NAME, IS_NULLABLE, DATA_TYPE
                     FROM INFORMATION_SCHEMA.COLUMNS 
                     WHERE TABLE_CATALOG = @DbName AND TABLE_SCHEMA = @Schema AND TABLE_NAME = @TableName
                     ORDER BY ORDINAL_POSITION";
        }

        private static ColumnInfo ParseDbReader(SqlDataReader reader)
        {
            return new ColumnInfo
                   {
                       Name = reader.GetString(0),
                       IsNullable = reader.GetString(1) == "YES",
                       DataType = reader.GetString(2),
                   };
        }
    }
}