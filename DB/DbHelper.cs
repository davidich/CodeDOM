namespace CodeDOM.DB
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public static class DbHelper
    {
        private static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
            }
        }

        internal static async Task<IEnumerable<TableInfo>> GetTableInfos()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                // Read tables
                List<TableInfo> result = await TableInfo.FetchFromDbAsync(connection);

                // Fill in table columns
                foreach (var tableInfo in result)
                {
                    await tableInfo.FetchColumnInfoAsync(connection);
                }

                return result;
            }
        }
    }
}