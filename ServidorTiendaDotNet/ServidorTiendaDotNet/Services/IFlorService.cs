using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public interface IFlorService
    {
        Task<List<Flor>> GetAllAsync();
        Task<Flor?> GetByIdAsync(int id);
        Task<Flor> CreateAsync(Flor flor);
        Task<bool> UpdateAsync(int id, Flor flor);
        Task<bool> DeleteAsync(int id);
    }
}
