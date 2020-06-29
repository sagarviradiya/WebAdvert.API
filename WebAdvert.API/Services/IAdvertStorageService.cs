using System.Threading.Tasks;
using WebAdvert.Models;

namespace WebAdvert.API.Services
{
    public interface IAdvertStorageService
    {
        Task<string> Add(AdvertModel model);
        Task Confirm(ConfirmAdvertModel model);
        Task<AdvertModel> GetByIdAsync(string id);
        Task<bool> CheckHealthAsync();
    }
}
