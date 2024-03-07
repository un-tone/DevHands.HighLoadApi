using DevHands.HighLoadApi.Data;
using DevHands.HighLoadApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DevHands.HighLoadApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CacheController(
    [FromKeyedServices("cache")]
    IDataStorage cache
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get()
    {
        var item = await cache.GetItem(id: RandomHelper.GenerateItemId());
        return item is null
            ? NotFound()
            : Ok(item);
    }
}