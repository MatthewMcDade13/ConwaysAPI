using Conways_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Conways_API.Controllers;

[ApiController]
[Route("[controller]")]
public class DisplayGameBoardController : ControllerBase
{

    private readonly ILogger<DisplayGameBoardController> logger;
    private readonly IBoardGenService genService;

    public DisplayGameBoardController(ILogger<DisplayGameBoardController> logger, IBoardGenService genService)
    {
        this.logger = logger;
        this.genService = genService;
    }

    [HttpGet]
    [Route("NewRandom/{width}/{height}")]
    public ActionResult<string> NewRandom(int width, int height)
    {
        return this.genService.NewRandomBoard(width, height).ToGridString();
    }

    [HttpGet]
    [Route("Get/{id}")]
    public string Get(int id)
    {
        return this.genService.Get(id).ToGridString();
    }

    [HttpGet]
    [Route("GetNext/{id}")]
    public ActionResult<string> GetNext(int id)
    {
        return this.genService.GetNext(id).ToGridString();
    }

    [HttpGet]
    [Route("GetNextN/{id}")]
    public ActionResult<string> GetNextN(int id, int n)
    {
        return this.genService.GetNextN(id, n).ToGridString();
    }


    [HttpGet]
    [Route("GetFinalState/{id}")]
    public ActionResult<string> GetFinalState(int id, int maxDepth)
    {
        var result = this.genService.GetFinalState(id, maxDepth);
        if (result.Success) {
            return result.Data?.ToGridString()!;
        } else {
            return result.Message!;
        }
    }
}
