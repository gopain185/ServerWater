using Microsoft.AspNetCore.Mvc;


namespace ServerWater.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        public class ItemLoginUser
        {
            public string username { get; set; } = "";
            public string password { get; set; } = "";
        }


        [HttpPost]
        [Route("login")]
        public IActionResult Login(ItemLoginUser item)
        {
            return Ok(Program.api_user.login(item.username, item.password));
        }

        [HttpGet]
        [Route("listrole")]
        public IActionResult ListRole()
        {
            return Ok(Program.api_role.getListRole());
        }

        [HttpGet]
        [Route("listuser")]
        public IActionResult listuser([FromHeader] string token)
        {
            return Ok(Program.api_user.listUser(token));
        }

        [HttpGet]
        [Route("listarea")]
        public IActionResult listarea()
        {
            return Ok(Program.api_area.listArea());
        }

        [HttpGet]
        [Route("listcustomer")]
        public IActionResult listcustomer()
        {
            return Ok(Program.api_customer.listCustomer());
        }

        [HttpPut]
        [Route("addUser")]
        public async Task<IActionResult> addUser([FromHeader] string token, string area, string user)
        {
            if (string.IsNullOrEmpty(area) || string.IsNullOrEmpty(user))
            {
                return BadRequest();
            }

            bool flag = await Program.api_area.addUser(token,user, area);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPut]
        [Route("addCustomer")]
        public async Task<IActionResult> addCustomer([FromHeader] string token, string area, string cus)
        {
            if (string.IsNullOrEmpty(area) || string.IsNullOrEmpty(cus))
            {
                return BadRequest();
            }

            bool flag = await Program.api_area.addCustomer(token, cus, area);
            if (flag)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("listCustomerArea")]
        public IActionResult listCustomerArea(string area)
        {
            return Ok(Program.api_area.getListCustomerArea(area));
        }

        [HttpGet]
        [Route("listUserArea")]
        public IActionResult listUserArea(string area)
        {
            return Ok(Program.api_area.getListUserArea(area));
        }
    }
}
