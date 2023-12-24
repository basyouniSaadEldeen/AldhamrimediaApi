using AldhamrimediaApi.DbContext;
using AldhamrimediaApi.Dtos.UtilitieDtos;
using AldhamrimediaApi.Models;
using AldhamrimediaApi.Service.interfaces;
using Bookify.Web.Settings;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.AccessControl;
using System.Security.Principal;
using ResourceType = System.Security.AccessControl.ResourceType;
using AldhamrimediaApi.Enums;
using AldhamrimediaApi.Controllers;
using Microsoft.Extensions.Localization;
using System.Xml.Linq;

namespace AldhamrimediaApi.Service.Repositories
{
    public class ServicesRepository : IServicesRepository
    {
        public readonly ApplicationDbContext _dbContext;
        private readonly Cloudinary _cloudinary;
        private readonly IStringLocalizer<ServiceController> _localization;


        public ServicesRepository(ApplicationDbContext dbContext, IOptions<CloudinarySettings> cloudinary,
            IStringLocalizer<ServiceController> localization)
        {
            _dbContext = dbContext;
            _localization = localization;
            Account account = new()
            {
                Cloud = cloudinary.Value.Cloud,
                ApiKey = cloudinary.Value.ApiKey,
                ApiSecret = cloudinary.Value.ApiSecret


            };
            _cloudinary = new Cloudinary(account);

        }



        public async Task<string> AddServicesAsync(AddUtilitieDto model)
        {
            try
            {
                if (model == null)
                    throw new ArgumentNullException("please add information");


                var ImageurlLogo = await UploadImageAsync(model.LogoImage);
                var ImageurlPoster = await UploadImageAsync(model.PosterImage);
                utilitie utilitie = new()
                {
                    Description = model.Description,
                  
                    Type = model.Type.ToString(),

                    Name = model.Name,
                    ImageUrlLogo = ImageurlLogo.Item2,
                    ImagePublicIdLogo = ImageurlLogo.Item1,
                    ImageUrlPoster = ImageurlPoster.Item2,
                    ImagePublicIdPoster = ImageurlPoster.Item1,
                    IsManagementService = model.IsManagementService


                };
                await _dbContext.utilities.AddAsync(utilitie);
                await _dbContext.SaveChangesAsync();

                return $"utilitie {utilitie.Type} is added successfully   ";
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"{ex.Message}");
            }


        }


        public async Task<string> AddSubServiceAsync(AddSubServiceDto model)
        {
            try
            {
                if (model == null)
                    throw new ArgumentNullException("please add information");

                var utilite =  await _dbContext.utilities.FirstOrDefaultAsync(x => x.Id == model.utilitieId);
                if (utilite == null)
                    throw new ArgumentNullException("nOT FOUND SERVICE");


                SubService subService = new()
                {
                    Description = model.Description,
                    Name = model.Name,
                   utilitieId=utilite.Id,
                };

                await _dbContext.subServices.AddAsync(subService);
                await _dbContext.SaveChangesAsync();

                return $"utilitie {subService.Name} is added successfully   ";
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"{ex.Message}");
            }


        }

        public bool DeleteServices(Guid id)
        {
            
            var serviceDetails = _dbContext.utilities.FirstOrDefault(x => x.Id == id);
        
            if (serviceDetails == null)
                throw new ArgumentNullException("Not Found");
            _dbContext.utilities.Remove(serviceDetails);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<IEnumerable<object>> GetAllServicesAsync()
        {
            var services = 
                await _dbContext.utilities.Where(x=>!x.IsManagementService)
                .Select(x=>  new { Id = x.Id, Name = string.Format(_localization[x.Type]), x.ImageUrlLogo  }).ToListAsync();
           
            if (services == null)
                throw new ArgumentNullException("Not Found");
            return services;
        }

        public async Task<IEnumerable<object>> GetBestServicesAsync()
        {
            var services = await _dbContext.utilities.Select
                (x => new { Id = x.Id, Name = string.Format(_localization[x.Type.ToString()]) ,
                    Service = string.Format(_localization[x.Name]), x.ImageUrlLogo }).ToListAsync();
            if (services == null)
                throw new ArgumentNullException("Not Found");
            return services;
        }

      
        public object GetServiceAsync(Guid id)
        {
            var serviceDetails = _dbContext.utilities.Where(x => x.Id == id).Select(x => new GetServiceDto
            {
                Id=x.Id,
                Description= string.Format(_localization[x.Description]),
                ImageUrlLogo=x.ImageUrlLogo,
                Name= string.Format(_localization[x.Name]),
                ImageUrlPoster=x.ImageUrlPoster,
                Type= string.Format(_localization[x.Type]),
                subservices=x.subServices.Select(x=> new SubserviceDto
                {
                    Id = x.Id,
                    Description = string.Format(_localization[x.Description]),
                    Name = string.Format(_localization[x.Name])
                }).ToList()
                
                
            });
            return serviceDetails;
        }
        public async Task< List<GetServiceDto>> GetAccountManagementServiceAsync()
        {
            var serviceDetailsList =await _dbContext.utilities.Where(x=>x.IsManagementService ).Select(x => new GetServiceDto
            {
                
                Id = x.Id,
                Description = string.Format(_localization[x.Description]),
                ImageUrlLogo = x.ImageUrlLogo,
                Name = string.Format(_localization[x.Name]),
                ImageUrlPoster = x.ImageUrlPoster,
                Type = string.Format(_localization[x.Type]),
                subservices = x.subServices.Select(x => new SubserviceDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    Name = x.Name
                }).ToList()


            }).ToListAsync();
            return serviceDetailsList;
        }


        public List<SubserviceDto> GetSubService(Guid id)
        {
            var serviceDetails = _dbContext.subServices.Where(x => x.utilitieId == id).Select(x => new SubserviceDto
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name
            }).ToList();


         
            return serviceDetails;
        }

        public async Task<(string, string)> UploadImageAsync(IFormFile photo)
        {

            if (photo.Length > 0)
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
                throw new ApplicationException(" error please try again ");
            }


        }


        public async Task<string> DeleteImageAsync(string publicId)
        {


            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = CloudinaryDotNet.Actions.ResourceType.Image
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



    }
}
