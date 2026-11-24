namespace NuviaApi.Modulos.CuentaHogar.Aplicacion;

public sealed record CuentaHogarDto(
    Guid UsuarioId,
    string Usuario,
    string Correo,
    decimal PresupuestoSemanal,
    int TamanoHogar,
    IReadOnlyList<string> Restricciones);
