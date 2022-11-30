
using Microsoft.AspNetCore.Mvc;

namespace ServerWater.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;

        public JobController(ILogger<JobController> logger)
        {
            _logger = logger;
        }


    }
}
