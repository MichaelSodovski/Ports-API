using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortsApi.Services;


namespace PortsApi
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly UsersLogic _usersLogic;
        private readonly PermissionsLogic _permissionsLogic;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserService userService, UsersLogic usersLogic,
            PermissionsLogic permissionsLogic, ILogger<AuthController> logger)
        {
            _userService = userService;
            _usersLogic = usersLogic;
            _permissionsLogic = permissionsLogic;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetUserPermissions")]
        public IActionResult GetUserPermissions([FromQuery] int LayerID)
        {
            try
            {
                string userName = _userService.GetUserName(HttpContext);

                if (string.IsNullOrEmpty(userName))
                {
                    _logger.LogWarning("User not found");
                    return BadRequest("User not found");
                }

                User user = _usersLogic.GetUser(userName);

                FilePermission userPermissions = new FilePermission();

                if (user.UserName != null)
                {
                    userPermissions = _permissionsLogic.GetPermissions(user.UserName, LayerID);
                }

                if (userPermissions == null)
                {
                    _logger.LogWarning("User not found");
                    return BadRequest("User not found");
                }

                return Ok(userPermissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user permissions");

                if (ex.InnerException != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException.Message);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
        }
    }
}
