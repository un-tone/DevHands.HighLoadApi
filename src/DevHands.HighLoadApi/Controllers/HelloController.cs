using Microsoft.AspNetCore.Mvc;

namespace DevHands.HighLoadApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public string Get() => "Hello, World!";
}