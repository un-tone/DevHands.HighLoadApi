using DevHands.HighLoadApi.Data;
using DevHands.HighLoadApi.Helpers;
using DevHands.HighLoadApi.Modules;
using Microsoft.AspNetCore.Mvc;

namespace DevHands.HighLoadApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController(
    [FromKeyedServices("db")]
    IDataStorage storage,
    [FromKeyedServices("cache")]
    IDataStorage cache
) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get()
    {
        var item = await storage.GetItem(id: RandomHelper.GenerateItemId());
        return item is null
            ? NotFound()
            : Ok(item);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create()
    {
        try
        {
            var items = DataModule.GenerateDataItems();
            await Task.WhenAll(
                storage.CreateItems(items),
                cache.CreateItems(items));
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}