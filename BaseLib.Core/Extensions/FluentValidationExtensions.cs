using System;

namespace FluentValidation
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithReasonCode<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Enum reasonCode, params string[] messages)
        {
            DefaultValidatorOptions.Configurable(rule).Current.ErrorCode = Convert.ToInt32(reasonCode).ToString();
            DefaultValidatorOptions.Configurable(rule).Current.SetErrorMessage(reasonCode?.GetDescription());
            return rule;
        }
    }
}