
using AldhamrimediaApi.Dto.UserDto;
using AldhamrimediaApi.Dtos.AcountDto;
using AldhamrimediaApi.Dtos.UserDto;

namespace AldhamrimediaApi.Service.interfaces
{
    public interface IAccountRepository
    {
        Task<AuthModel> RegesterUserAsync(RegistraionUserDto model, string roleName = "User");

        Task<AuthModel> LoginUserAsync(LoginUserDto model);
        Task<ProfileDto> GetProfileAsync(int id);
        Task<List<string>> GetNotificationAsync(int userId);
        Task<string> Recharge_Wallet(int user_id, decimal amount);
        Task<string> buyService(Buy_Service_Dto model, int user_id);
        Task<List<GetRecordsDto>> My_Records(int user_Id);



    }
}
