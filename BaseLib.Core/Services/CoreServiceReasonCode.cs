using System.ComponentModel;

namespace BaseLib.Core.Services
{
    public enum CoreServiceReasonCode
    {
        [Description("Operación exitosa")]
        Succeeded = 1,

        [Description("Error Operacion")]
        Failed = 2,

        [Description("Resultado de validación de request no es es válido")]
        ValidationResultNotValid = 125,

        [Description("Operación no implementada")]
        NotImplemented = 126,

        [Description("Ocurrio una excepción en el sistema")]
        ExceptionHappened = 127
    }
}