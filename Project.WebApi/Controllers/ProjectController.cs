using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.WebApi.Controllers;

[ApiController]
[Route("project")]
public class ProjectController : ControllerBase
{
    [HttpGet("user")]
    [Authorize(Roles = "USER")]
    public ActionResult User() => Ok("Hello User");

    [HttpGet("admin")]
    [Authorize(Roles = "ADMIN")]
    public ActionResult Admin() => Ok("Hello Admin");
}