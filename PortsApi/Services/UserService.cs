using PortsApi.Models;
using System.Security.Claims;

namespace PortsApi.Services
{
    public class UserService
    {
        private readonly TestContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(TestContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public string GetUserName(HttpContext httpContext)
        {
            ClaimsPrincipal user = httpContext.User;

            if (user == null)
            {
                _logger.LogWarning("HttpContext.User is null");
                return "";
            }

            if (user.Identity == null)
            {
                _logger.LogWarning("HttpContext.User.Identity is null");
                return "";
            }

            if (user.Identity.Name == null)
            {
                _logger.LogWarning("HttpContext.User.Identity.Name is null");
                return "";
            }

            string fullUserName = user.Identity.Name;
            string[] splitUserName = fullUserName.Split('\\');

            return splitUserName.Length > 1 ? splitUserName[1] : fullUserName;
        }


        public string GetDomain(HttpContext httpContext)
        {
            ClaimsPrincipal user = httpContext.User;

            if (user == null || user.Identity == null || user.Identity.Name == null)
            {
                _logger.LogWarning("Unable to determine domain from HttpContext");
                return "";
            }

            string fullUserName = user.Identity.Name;
            string[] splitUserName = fullUserName.Split('\\');

            return splitUserName.Length > 1 ? splitUserName[0] : fullUserName;
        }
    }
}
