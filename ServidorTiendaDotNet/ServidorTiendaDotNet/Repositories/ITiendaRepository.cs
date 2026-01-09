using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Repository
{
    public interface ITiendaRepository
    {
        Task<List<Flor>> GetFlores();
        int AddFlor(Flor nuevaFlor);
        bool UpdateFlor(int id, Flor florActualizada);
        bool DeleteFlor(int id);
        Flor GetFlorById(int id);
    }
}
