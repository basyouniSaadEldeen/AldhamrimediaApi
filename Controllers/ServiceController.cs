using AldhamrimediaApi.Dtos.UtilitieDtos;
using AldhamrimediaApi.Service.interfaces;
using JsonBasedLocalization.Api.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AldhamrimediaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        public readonly IServicesRepository servicesRepository;
    

        public ServiceController(IServicesRepository servicesRepository
           )
        {
           
            this.servicesRepository = servicesRepository;
        }

       

     

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] AddUtilitieDto model)
        {
            if (ModelState.IsValid)
            {

                var result = await servicesRepository.AddServicesAsync(model);
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }
        [HttpPost("AddSubService")]
        public async Task<IActionResult> AddSubService([FromForm] AddSubServiceDto model)
        {
            if (ModelState.IsValid)
            {

                var result = await servicesRepository.AddSubServiceAsync(model);
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var listOfService = await servicesRepository
                    .GetAllServicesAsync();
                return Ok(listOfService);

            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
        [HttpGet("Get_important_services")]
        public async Task<IActionResult> Get_important_services()
        {
            try
            {

                var listOfService = await servicesRepository
                    .GetBestServicesAsync();
                return Ok(listOfService);

            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
        [HttpGet("Get_management_services")]
        public async Task<IActionResult> Get_management_services()
        {
            try
            {

                var listOfService =await  servicesRepository
                    .GetAccountManagementServiceAsync();
                return Ok(listOfService);

            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
        [HttpGet("Get_service_details")]
        public IActionResult Get_service_details( Guid id)
        {
            try
            {

                var Service =  servicesRepository
                    .GetServiceAsync(id);
                return Ok(Service);

            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
        [HttpDelete("Delete_service")]
        public IActionResult Delete_service(Guid id)
        {
            try
            {

                var Service = servicesRepository
                    .DeleteServices(id);
                return Ok(Service);

            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

    }
}
