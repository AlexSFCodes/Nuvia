using System.ComponentModel.DataAnnotations;

namespace NuviaApi.Modulos.CuentaHogar.Aplicacion;

public sealed record ActualizarCuentaHogarDto(
    [Required(ErrorMessage = "El presupuesto semanal es obligatorio.")]
    [Range(typeof(decimal), "0", "79228162514264337593543950335",
        ErrorMessage = "El presupuesto semanal no puede ser negativo.")]
    decimal? PresupuestoSemanal,
    [Required(ErrorMessage = "El tamaño del hogar es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El hogar debe tener al menos una persona.")]
    int? TamanoHogar,
    [Required(ErrorMessage = "Las restricciones son obligatorias; envía una lista vacía si no hay restricciones.")]
    IReadOnlyList<string>? Restricciones) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PresupuestoSemanal is decimal presupuesto && decimal.Round(presupuesto, 2) != presupuesto)
        {
            yield return new ValidationResult(
                "El presupuesto semanal debe tener como máximo dos decimales.",
                [nameof(PresupuestoSemanal)]);
        }

        if (Restricciones is null)
        {
            yield break;
        }

        for (var i = 0; i < Restricciones.Count; i++)
        {
            var alimento = Restricciones[i];

            if (string.IsNullOrWhiteSpace(alimento))
            {
                yield return new ValidationResult(
                    "Cada restricción debe indicar el alimento que no consumes.",
                    [$"{nameof(Restricciones)}[{i}]"]);
            }
            else if (alimento.Trim().Length > 100)
            {
                yield return new ValidationResult(
                    "El nombre del alimento debe tener como máximo 100 caracteres.",
                    [$"{nameof(Restricciones)}[{i}]"]);
            }
        }
    }
}
