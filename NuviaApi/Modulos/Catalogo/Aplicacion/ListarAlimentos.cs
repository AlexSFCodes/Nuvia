namespace NuviaApi.Modulos.Catalogo.Aplicacion;

public sealed class ListarAlimentos
{
    public IReadOnlyList<AlimentoCatalogoDto> Ejecutar()
    {
        return new[]
        {
            new AlimentoCatalogoDto("Leche entera", "Lácteos", "Funda de 1 litro"),
            new AlimentoCatalogoDto("Avena", "Cereales", "Bolsa de 500 g")
        };
    }
}