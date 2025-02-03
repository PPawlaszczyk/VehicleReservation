using FluentValidation;

namespace VehicleReservationAPI.Extensions
{
    public static class ValidationExtensions
    {
        public static void ThrowIfInvalid<T>(this IValidator<T> validator, T instance)
        {
            var result = validator.Validate(instance);

            if (result != null && !result.IsValid)
            {
                var errorMessages = string.Join("; ", result.Errors.Select(error => error.ErrorMessage));
                throw new InvalidOperationException(errorMessages);
            }
        }
    }
}