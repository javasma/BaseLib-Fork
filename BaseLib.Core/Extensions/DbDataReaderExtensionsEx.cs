namespace System.Data.Common
{
    /// <summary>
    /// Extension methods for DbDataReader to handle null values and type conversions.
    /// null values are converted to default values for the type.
    /// </summary> 
    public static class DbDataReaderExtensionsEx
    {
        public static string? GetStringEx(this DbDataReader reader, string name)
        {
            var value = reader[name];
            return (value != null && value != DBNull.Value) ? Convert.ToString(value) : null;
        }

        public static long GetInt64Ex(this DbDataReader reader, string name)
        {
            var value = reader[name];
            return (value != null && value != DBNull.Value) ? Convert.ToInt64(value) : 0;
        }

        public static int GetInt32Ex(this DbDataReader reader, string name)
        {
            var value = reader[name];
            return (value != null && value != DBNull.Value) ? Convert.ToInt32(value) : 0;
        }

        public static short GetInt16Ex(this DbDataReader reader, string name)
        {
            var value = reader[name];
            return (value != null && value != DBNull.Value) ? Convert.ToInt16(value) : (short)0;
        }

        public static DateTime GetDateTimeEx(this DbDataReader reader, string name)
        {
            var value = reader[name];
            return (value != null && value != DBNull.Value) ? Convert.ToDateTime(value) : DateTime.MinValue;
        }

        public static bool GetBooleanEx(this DbDataReader reader, string name)
        {
            var value = reader[name];
            return (value != null && value != DBNull.Value) ? Convert.ToBoolean(value) : false;
        }

        public static double GetDoubleEx(this DbDataReader reader, string name)
        {
            var value = reader[name];
            return (value != null && value != DBNull.Value) ? Convert.ToDouble(value) : 0;
        }

        public static decimal GetDecimalEx(this DbDataReader reader, string name)
        {
            var value = reader[name];
            return (value != null && value != DBNull.Value) ? Convert.ToDecimal(value) : 0;
        }

        public static EnumT GetEnumEx<EnumT>(this DbDataReader reader, string name) where EnumT : struct, Enum
        {
            var value = reader.GetStringEx(name);
            return Enum.TryParse(value, out EnumT result) ? result : default;
        }
    }
}
