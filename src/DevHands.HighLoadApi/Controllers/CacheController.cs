using DevHands.HighLoadApi.Containers;
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
    public async Task<IActionResult> Get(int r = 1)
    {
        if (r <= 0)
            r = 1;

        var list = new List<DataItem>();
        for (int i = 0; i < r; i++)
        {
            var item = await cache.GetItem(id: RandomHelper.GenerateItemId());
            if (item != null)
                list.Add(item);
        }

        return list.Any()
            ? Ok(list)
            : NotFound();
    }
}