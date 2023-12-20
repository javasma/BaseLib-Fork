using System.ComponentModel;

namespace BaseLib.Core.Services
{
    public enum CoreServiceReasonCode
    {
        [Description("Undefined")]
        Undefined = 1,

        [Description("Operación exitosa")]
        Succeeded = 1,

        [Description("Error Operacion")]
        Failed = 2,

        [Description("Operación no implementada")]
        NotImplemented = 64,

        [Description("Operación en mantenimiento")]
        Maintenance = 65,

        [Description("Resultado de validación de request no es es válido")]
        ValidationResultNotValid = 96,

        [Description("Ocurrio una excepción en el sistema")]
        ExceptionHappened = 127
    }
}