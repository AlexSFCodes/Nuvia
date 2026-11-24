using System.Collections.Concurrent;
using NuviaApi.Modulos.CuentaHogar.Aplicacion;

namespace NuviaApi.Modulos.CuentaHogar.Infraestructura;

// Almacenamiento temporal: los hogares y sus restricciones se pierden al reiniciar la aplicación.
public sealed class HogaresEnMemoria : IRepositorioHogares
{
    private readonly ConcurrentDictionary<Guid, HogarDto> _hogares = new();

    public HogarDto Obtener(Guid usuarioId)
    {
        return _hogares.GetOrAdd(usuarioId, _ => new HogarDto(0m, 1, Array.Empty<string>()));
    }

    public void Guardar(Guid usuarioId, HogarDto hogar)
    {
        // Se reemplaza el hogar completo para mantener presupuesto, tamaño y restricciones juntos.
        _hogares[usuarioId] = hogar with
        {
            Restricciones = Array.AsReadOnly(hogar.Restricciones.ToArray())
        };
    }
}
