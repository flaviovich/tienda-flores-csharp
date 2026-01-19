using ServidorTiendaDotNet.DTOs;
using ServidorTiendaDotNet.Models;

namespace ServidorTiendaDotNet.Services
{
    public interface IFlorService
    {
        Task<List<FlorResponse>> GetAllAsync();
        Task<FlorResponse?> GetByIdAsync(int id);
        Task<FlorCreateDto> CreateAsync(FlorCreateDto flor);
        Task<bool> UpdateAsync(int id, FlorUpdateDto flor);
        Task<bool> DeleteAsync(int id);
    }
}
