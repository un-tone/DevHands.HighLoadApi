using DevHands.HighLoadApi.Containers;
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
    public async Task<IActionResult> Get(int r = 1, bool multi = false)
    {
        if (r <= 0 || multi)
            r = 1;

        var cachedList = new List<DataItem>();
        var list = new List<DataItem>();
        for (int i = 0; i < r; i++)
        {
            if (multi)
            {
                var cachedItem = await cache.GetItem(id: RandomHelper.GenerateItemId());
                if (cachedItem != null)
                    cachedList.Add(cachedItem);
            }

            var item = await storage.GetItem(id: RandomHelper.GenerateItemId());
            if (item != null)
                list.Add(item);
        }

        return list.Any()
            ? multi
                ? Ok(new[] { list, cachedList })
                : Ok(list)
            : NotFound();
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