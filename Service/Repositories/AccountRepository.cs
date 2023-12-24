using AldhamrimediaApi.DbContext;
using AldhamrimediaApi.Dto.UserDto;
using AldhamrimediaApi.Dtos.AcountDto;
using AldhamrimediaApi.Dtos.UserDto;
using AldhamrimediaApi.Models;
using AldhamrimediaApi.Service.interfaces;
using Bookify.Web.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Role = AldhamrimediaApi.Models.Role;

namespace HealthCare.Services.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        private IConfiguration configuration;
        private readonly Cloudinary _cloudinary;
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IOptions<Jwt> jwt;
        private readonly Jwt _jwt;
        //private readonly Cloudinary _cloudinary;
        public AccountRepository(ApplicationDbContext dbContext, IOptions<Jwt> jwt, IConfiguration configuration,
            UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<CloudinarySettings> cloudinary)
        {
            this.jwt = jwt;
            _jwt = jwt.Value;
            this.configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;

            Account account = new()
            {
                Cloud = cloudinary.Value.Cloud,
                ApiKey = cloudinary.Value.ApiKey,
                ApiSecret = cloudinary.Value.ApiSecret


            };
            _cloudinary = new Cloudinary(account);



        }


        public async Task<AuthModel> LoginUserAsync(LoginUserDto model)
        {

            var existing_user = await userManager.FindByEmailAsync(model.Email);



            if (existing_user == null)
            {
                return new AuthModel
                {

                    Message = "the email or password is incorrect",
                    IsAuthenticated = false,

                };


            }


            var isCorrect = await userManager.CheckPasswordAsync(existing_user, model.Password);
            if (!isCorrect)
            {
                return new AuthModel
                {

                    Message = "the email or password is incorrect",
                    IsAuthenticated = false,

                };

            }



            var role = userManager.GetRolesAsync(existing_user).Result;
            var jwtSecurityToken = await CreateJwtToken(existing_user);


            return new AuthModel()
            {
               Message = $"welcome {existing_user.FullName}",
                Email = existing_user.Email,

                //DepartmentId= (int)existing_user.DepartmentId,
                IsAuthenticated = true,
                Role = role.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserId = existing_user.Id,

            };





        }

        public async Task<AuthModel> RegesterUserAsync(RegistraionUserDto model, string roleName = "User")
        {

            var user_exist = await userManager.FindByEmailAsync(model.Email);
            if (user_exist != null)
            {
                return new AuthModel
                {

                    Message = "the email Is used",
                    IsAuthenticated = false,

                };

            };
            var Imageurl = await UploadImageAsync(model.Image);

            var user = new User()
            {

                Email = model.Email,
                UserName = model.Email,
                FullName=model.Name,
                PhoneNumber = model.PhoneNumber,
                Country =model.Country,
                Country_code=model.Country_code,
                ImageUrl = Imageurl.Item2,
                ImagePublicId = Imageurl.Item1


                //Type= TypesOfPeople.User

            };
            var is_created = await userManager.CreateAsync(user, model.Password);
         

            if (is_created.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);
                var jwtSecurityToken = await CreateJwtToken(user);

                var role = userManager.GetRolesAsync(user).Result;
                var result1 = new AuthModel
                {

                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Email = model.Email,

                    UserId = user.Id,
                    Message = $"welcome  {user.FullName}",
                    IsAuthenticated = true,


                    Role = role.ToList(),
                   

                };
                return result1;


            }

            else
            {

                return new AuthModel
                {

                    Message = "User Not Added try Again",
                    IsAuthenticated = false,

                };


            }

        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<ProfileDto> GetProfileAsync(int  id )
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new ArgumentNullException("Not Found");

            var result = new ProfileDto()
            {
                My_balance=user.My_balance,
                Email = user.Email,
                Name = user.FullName,
                Country = user.Country,
                Id = user.Id,
                phoneNumber = user.PhoneNumber,
                ImageUrl = user.ImageUrl
            };
            return result;

        }
        public async Task<List<string>> GetNotificationAsync(int userId)
        {
            var messages =await dbContext.notifications.Where(x => x.userId == userId).Select(x=>x.message).ToListAsync();
          

            return messages;
        }

        public async Task<bool> AddNotificationAsync(int userId,string Message)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new ArgumentNullException("Not Found");
            var notification = new Notifications
            {
                userId=user.Id,
                message=Message
            };

            await dbContext.notifications.AddAsync(notification);
            await dbContext.SaveChangesAsync();
            return true;

        }

        public async Task<(string, string)> UploadImageAsync(IFormFile? photo)
        {

            if (photo !=null)
            {
                var fileName = Path.GetFileName(photo.FileName);

                using (var stream = photo.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(fileName, stream),
                        PublicId = Guid.NewGuid().ToString("N"),
                        Folder = "my_photos"
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    var PublicId = uploadResult.PublicId;
                    var Url = uploadResult.SecureUri.ToString();
                    return (PublicId, Url);

                };
            }
            else
            {
                return (" "," ");
            }


        }

        public async Task<string> DeleteImageAsync(string publicId)
        {


            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };

            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Result == "ok")
            {
                return " Image deleted successfully";
            }
            else
            {
                return result.Result;
            }



        }
        public async Task<string> Recharge_Wallet( int user_id ,decimal amount)
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == user_id);
                if (user == null)
                    throw new ArgumentNullException("not found");

                user.My_balance += amount;
                await dbContext.SaveChangesAsync(); var rsultOfNotification = await AddNotificationAsync(user.Id,
                          $"The wallet was loaded with an amount of fifty dollar {amount} ");

                return "successfully done";
            }catch(Exception ex)
            {
                return $"{ex.Message}";
            }
        }

        public async Task<string> buyService(Buy_Service_Dto model ,int user_id  )
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == user_id);
                var service = await dbContext.utilities.FirstOrDefaultAsync(x => x.Id == model.service_Id);
                if (user == null || service == null)
                    throw new ArgumentNullException("not found");

                if (user.My_balance >= model.Number_of_money_paid)
                {
                    var myService = new Purchases
                    {
                        Service_ImgUrl = service.ImageUrlLogo,
                        Service_name = service.Name,
                        Type_of_service = service.Type,
                        Service_request_date = DateTime.Now,
                        Account_link = model.Account_link,
                        userId = user.Id,
                        Number_of_money_paid = model.Number_of_money_paid,
                        Required_quantity = model.Required_quantity

                    };
                    await dbContext.Purchases.AddAsync(myService);
                    user.My_balance=user.My_balance - model.Number_of_money_paid;

                  var rsultOfNotification =await  AddNotificationAsync( user.Id,
                      $"Your request has been accepted.{service.Name} on the platform {service.Type} Thank you very much for your trust in us");
                    await dbContext.SaveChangesAsync();

                    return "successfully done";
                }
                else
                {
                   return"There is not enough balance in the wallet, please recharge";
                  
                }
            }catch(Exception ex)
            {
                throw new ApplicationException($"{ex.Message}");
            }

        }
        public async Task<List<GetRecordsDto>> My_Records(int user_Id)
        {
            try
            {
                var records = await dbContext.Purchases.Where(x => x.userId == user_Id).Select(x => new GetRecordsDto
                {
                    Id = x.Id,
                    Service_ImgUrl = x.Service_ImgUrl,
                    Service_name = x.Service_name,
                    Service_request_date = x.Service_request_date,
                    Type_of_service = x.Type_of_service,
                    Number_of_money_paid = x.Number_of_money_paid

                }).ToListAsync();

                if (records == null)
                    throw new ArgumentNullException("not found");
                return records;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"{ex.Message}");
            }
        }


    }
}
