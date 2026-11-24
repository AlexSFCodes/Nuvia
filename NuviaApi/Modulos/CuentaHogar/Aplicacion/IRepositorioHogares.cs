namespace NuviaApi.Modulos.CuentaHogar.Aplicacion;

public interface IRepositorioHogares
{
    HogarDto Obtener(Guid usuarioId);

    void Guardar(Guid usuarioId, HogarDto hogar);
}
