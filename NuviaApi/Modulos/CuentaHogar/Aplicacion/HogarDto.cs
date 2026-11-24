namespace NuviaApi.Modulos.CuentaHogar.Aplicacion;

public sealed record HogarDto(
    decimal PresupuestoSemanal,
    int TamanoHogar,
    IReadOnlyList<string> Restricciones);
