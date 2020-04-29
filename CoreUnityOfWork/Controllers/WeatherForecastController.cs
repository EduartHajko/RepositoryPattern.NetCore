using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using unityOfWork.BuissnesServices.IServices;

namespace CoreUnityOfWork.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        public IValuesService IValuesService { get; }

        public WeatherForecastController(IValuesService IValuesService)
        {
            this.IValuesService = IValuesService;
        }


        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            var result = await IValuesService.getAllValues();

            return Ok(result);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var result = await IValuesService.getValue(id);

            return Ok(result);
        }


    }
}
