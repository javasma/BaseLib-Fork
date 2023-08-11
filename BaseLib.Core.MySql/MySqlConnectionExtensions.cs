

namespace MySql.Data.MySqlClient
{
    public static class MySqlConnectionExtensions
    {
        public static void SetWaitTimeout(this MySqlConnection connection, int timeout)
        {
            using( var command = new MySqlCommand($"SET session wait_timeout={timeout};", connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public static int GetWaitTimeout(this MySqlConnection connection)
        {
            using( var command = new MySqlCommand("SHOW VARIABLES LIKE 'wait_timeout';", connection))
            using( var reader = command.ExecuteReader())
            {
                if( reader.Read())
                {
                    return reader.GetInt32(1);
                }
                return 0;
            }
        }
    }
}