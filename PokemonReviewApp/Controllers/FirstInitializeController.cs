using API_Dinamis.Data;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.IO;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirstInitializeController : Controller
    {
        private readonly IFirstInitializeRepository _firstinitializerepository;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authrepository;
        private readonly DataContext _context;

        public FirstInitializeController(IFirstInitializeRepository firstInitializeRepository, IMapper mapper, IAuthRepository authrepository, DataContext context)
        {
            _firstinitializerepository = firstInitializeRepository;
            _mapper = mapper;
            _context = context;
            _authrepository = authrepository;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SetFirstInitialization()
        {
            var response = "";

            //set standard value 
            var countryResult = await SetBasicCountry();
            if (countryResult)
            {
                response += "Successfully set country data";
            }
            else
            {
                response += "Failed to set country data";
            }
            var cityResult = await SetBasicCity();
            if (cityResult)
            {
                response += ", Successfully set city data";
            }
            else
            {
                response += "Failed to set city data";
            }
            var zipCodeResult = await SetBasicZipCode();
            if (zipCodeResult)
            {
                response += ", Successfully set zip code data";
            }
            else
            {
                response += "Failed to set zip code data";
            }
            var standardValueResult = await SetBasicStandardValue();
            if (standardValueResult)
            {
                response += ", Successfully set standard value data";
            }
            else
            {
                response += "Failed to set standard value data";
            }
            var moduleResult = await SetBasicModule();
            if (moduleResult)
            {
                response += ", Successfully set standard value data";
            }
            else
            {
                response += "Failed to set standard value data";
            }
            var menuResult = await SetBasicMenu();
            if (menuResult)
            {
                response += ", Successfully set standard value data";
            }
            else
            {
                response += "Failed to set standard value data";
            }

            return Ok(response);
        }
        private async Task<bool> SetBasicCountry()
        {
            var filePath = "Data/Basic/Country.json";
            if (!System.IO.File.Exists(filePath))
            {
                // Handle file not found
                // You can log an error or return false, depending on your requirement
                return false;
            }
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                // Other settings...
            };
            try
            {
                var dataToAdd = JsonConvert.DeserializeObject<List<Country>>(json, jsonSerializerSettings);

                await _firstinitializerepository.InjectDefaultCountry(dataToAdd);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Deserialization error: " + ex.Message);

                return false;
            }
        }
        private async Task<bool> SetBasicCity()
        {
            var filePath = "Data/Basic/City.json";
            if (!System.IO.File.Exists(filePath))
            {
                // Handle file not found
                // You can log an error or return false, depending on your requirement
                return false;
            }
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                // Other settings...
            };
            try
            {
                var dataToAdd = JsonConvert.DeserializeObject<List<City>>(json, jsonSerializerSettings);

                await _firstinitializerepository.InjectDefaultCity(dataToAdd);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Deserialization error: " + ex.Message);

                return false;
            }
        }
        private async Task<bool> SetBasicZipCode()
        {
            var filePath = "Data/Basic/ZipCode.json";
            if (!System.IO.File.Exists(filePath))
            {
                // Handle file not found
                // You can log an error or return false, depending on your requirement
                return false;
            }
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                // Other settings...
            };
            try
            {
                var dataToAdd = JsonConvert.DeserializeObject<List<Zipcode>>(json, jsonSerializerSettings);

                await _firstinitializerepository.InjectDefaultZipCode(dataToAdd);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Deserialization error: " + ex.Message);

                return false;
            }
        }

        private async Task<bool> SetBasicStandardValue()
        {
            var filePath = "Data/Basic/StandardValue.json";
            if (!System.IO.File.Exists(filePath))
            {
                // Handle file not found
                // You can log an error or return false, depending on your requirement
                return false;
            }
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                // Other settings...
            };
            try
            {
                var dataToAdd = JsonConvert.DeserializeObject<List<StandardValue>>(json, jsonSerializerSettings);

                await _firstinitializerepository.InjectDefaultStandardValue(dataToAdd);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Deserialization error: " + ex.Message);

                return false;
            }
        }

        private async Task<bool> SetBasicModule()
        {
            var filePath = "Data/Basic/Module.json";
            if (!System.IO.File.Exists(filePath))
            {
                // Handle file not found
                // You can log an error or return false, depending on your requirement
                return false;
            }
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                // Other settings...
            };
            try
            {
                var dataToAdd = JsonConvert.DeserializeObject<List<Module>>(json, jsonSerializerSettings);

                await _firstinitializerepository.InjectDefaultModule(dataToAdd);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Deserialization error: " + ex.Message);

                return false;
            }
        }

        private async Task<bool> SetBasicMenu()
        {
            var filePath = "Data/Basic/Menu.json";
            if (!System.IO.File.Exists(filePath))
            {
                // Handle file not found
                // You can log an error or return false, depending on your requirement
                return false;
            }
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                // Other settings...
            };
            try
            {
                var dataToAdd = JsonConvert.DeserializeObject<List<Menu>>(json, jsonSerializerSettings);

                await _firstinitializerepository.InjectDefaultMenu(dataToAdd);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Deserialization error: " + ex.Message);

                return false;
            }
        }

    }
}
