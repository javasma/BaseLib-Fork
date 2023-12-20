
namespace BaseLib.Core.Models
{
    public record CoreReasonCode(int Value, string Description) : IConvertible
    {
        public static CoreReasonCode Null { get { return new CoreReasonCode(0, "Undefined"); } }

        public static implicit operator CoreReasonCode(Enum enumValue)
            => new CoreReasonCode(Convert.ToInt32(enumValue), enumValue.GetDescription());

        public static bool operator ==(CoreReasonCode reasonCode, Enum enumValue)
            => reasonCode.Value == Convert.ToInt32(enumValue);

        public static bool operator !=(CoreReasonCode reasonCode, Enum enumValue)
            => !(reasonCode == enumValue);

        public static bool operator ==(CoreReasonCode reasonCode, Int32 intValue)
            => reasonCode.Value == intValue;

        public static bool operator !=(CoreReasonCode reasonCode, Int32 intValue)
            => !(reasonCode == intValue);


        public static explicit operator int(CoreReasonCode reasonCode)
            => reasonCode.Value;

        public T ToEnum<T>() where T : struct, Enum
        {
            return (T)Enum.ToObject(typeof(T), Value);
        }

        // Implement IConvertible
        public TypeCode GetTypeCode()
        {
            return TypeCode.Int32;
        }

        public int ToInt32(IFormatProvider? provider)
        {
            return Value;
        }

        public bool ToBoolean(IFormatProvider? provider)
        {
            return Convert.ToBoolean(Value, provider);
        }

        public byte ToByte(IFormatProvider? provider)
        {
            return Convert.ToByte(Value, provider);
        }

        public char ToChar(IFormatProvider? provider)
        {
            return Convert.ToChar(Value, provider);
        }

        public DateTime ToDateTime(IFormatProvider? provider)
        {
            return Convert.ToDateTime(Value, provider);
        }

        public decimal ToDecimal(IFormatProvider? provider)
        {
            return Convert.ToDecimal(Value, provider);
        }

        public double ToDouble(IFormatProvider? provider)
        {
            return Convert.ToDouble(Value, provider);
        }

        public short ToInt16(IFormatProvider? provider)
        {
            return Convert.ToInt16(Value, provider);
        }

        public long ToInt64(IFormatProvider? provider)
        {
            return Convert.ToInt64(Value, provider);
        }

        public sbyte ToSByte(IFormatProvider? provider)
        {
            return Convert.ToSByte(Value, provider);
        }

        public float ToSingle(IFormatProvider? provider)
        {
            return Convert.ToSingle(Value, provider);
        }

        public string ToString(IFormatProvider? provider)
        {
            return Convert.ToString(Value, provider);
        }

        public object ToType(Type conversionType, IFormatProvider? provider)
        {
            return Convert.ChangeType(Value, conversionType, provider);
        }

        public ushort ToUInt16(IFormatProvider? provider)
        {
            return Convert.ToUInt16(Value, provider);
        }

        public uint ToUInt32(IFormatProvider? provider)
        {
            return Convert.ToUInt32(Value, provider);
        }

        public ulong ToUInt64(IFormatProvider? provider)
        {
            return Convert.ToUInt64(Value, provider);
        }
    }
}
