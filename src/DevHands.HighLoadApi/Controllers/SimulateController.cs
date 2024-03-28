using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace DevHands.HighLoadApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SimulateController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Get(int? cpu, int? io) =>
        cpu == null && io == null
            ? BadRequest($"Invalid parameters: cpu={cpu} | io={io}")
            : Ok(new
            {
                Cpu = MakeCpu(cpu),
                Io = MakeIo(io)
            });

    private static long? MakeCpu(int? cpu)
    {
        var process = Process.GetCurrentProcess();
        var startTime = process.UserProcessorTime;
        long? cpuResult = null;

        if (cpu > 0)
        {
            var bytes = new byte[100];
            using var md5 = MD5.Create();
            TimeSpan diffTime;
            do
            {
                diffTime = process.UserProcessorTime - startTime;
                Random.Shared.NextBytes(bytes);
                md5.ComputeHash(bytes);
            } while (diffTime.TotalMilliseconds < cpu);

            cpuResult = (long)diffTime.TotalMilliseconds;
        }

        return cpuResult;
    }

    private static long? MakeIo(int? io)
    {
        long? ioResult = null;
        if (io > 0)
        {
            var sw = Stopwatch.StartNew();
            Thread.Sleep(io.Value);
            ioResult = sw.ElapsedMilliseconds;
        }

        return ioResult;
    }
}