using AldhamrimediaApi.Dtos.UtilitieDtos;
using AldhamrimediaApi.Models;

namespace AldhamrimediaApi.Service.interfaces
{
    public interface IServicesRepository
    {



        object GetServiceAsync(Guid id);
        Task<string> AddServicesAsync(AddUtilitieDto model);
        Task<string> AddSubServiceAsync(AddSubServiceDto model);
        bool DeleteServices(Guid id);
        Task<IEnumerable<object>> GetAllServicesAsync();
        Task<IEnumerable<object>> GetBestServicesAsync();
        Task<List<GetServiceDto>> GetAccountManagementServiceAsync();

        List<SubserviceDto> GetSubService(Guid id);


    }
}
