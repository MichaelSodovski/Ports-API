using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PortsApi.Services;
using System.Text.Json;

namespace PortsApi
{
    [Authorize]
    [Route("api/Log")]
    [EnableCors("AllowAllOrigins")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly GeoContext _geocontext; // Replace YourDbContext with the correct DbContext name

        public LogController(UserService userService, GeoContext geocontext) // Replace YourDbContext with the correct DbContext name
        {
            _userService = userService;
            _geocontext = geocontext;
        }

        //public TESTLog GetLog()
        //{
        //    string userName = _userService.GetCurrentUserName(User); // Use the UserService to get the user name

        //    if (String.IsNullOrEmpty(userName))
        //    {
        //        return null;
        //    }

        //    TESTLog log = _geocontext.Log.Where(x => x.User.ToLower() == userName.ToLower()).FirstOrDefault();

        //    if (log != null)
        //    {
        //        return log;
        //    } 

        //    return log;
        //}

        //[HttpPost]
        //public async Task<ActionResult<TESTLog>> Log([FromBody] JsonElement data)
        //{
        //    TESTLog log = new TESTLog();
        //    log.User = _userService.GetCurrentUserName(User);
        //    log.EventDetails = data.GetProperty("actionDetails").ToString();
        //    log.EventType = data.GetProperty("event").ToString();
        //    log.CreationDate = DateTime.Now;
        //    if (log != null)
        //    {
        //        return log;
        //    }
        //    return CreatedAtAction("GetLog", new { id = log.Id }, log);
        //}
    }
}
