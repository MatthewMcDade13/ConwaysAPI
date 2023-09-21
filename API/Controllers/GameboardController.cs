using System.Reflection.Metadata.Ecma335;
using Conways_API.Common;
using Conways_API.Services;
using DataAccess;
using DataAccess.Common;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Conways_API.Controllers;

[ApiController]
[Route("[controller]")]
public class GameBoardController : ControllerBase
{

    private readonly ILogger<GameBoardController> logger;
    private readonly IBoardGenService genService;

    public GameBoardController(ILogger<GameBoardController> logger, IBoardGenService genService)
    {
        this.logger = logger;
        this.genService = genService;
    }

    [HttpGet]
    [Route("Get/{id}")]
    public ActionResult<FlatGameBoard> Get(int id)
    {
        return this.genService.Get(id).ToFlatGameBoard();
    }

    [HttpGet]
    [Route("GetNext/{id}")]
    public ActionResult<FlatGameBoard> GetNext(int id)
    {
        return this.genService.GetNext(id).ToFlatGameBoard();
    }

    [HttpGet]
    [Route("GetNextN/{id}")]
    public ActionResult<FlatGameBoard> GetNextN(int id, int n)
    {
        return this.genService.GetNextN(id, n).ToFlatGameBoard();
    }


    [HttpGet]
    [Route("GetFinalState/{id}")]
    public ActionResult<Result<FlatGameBoard>> GetFinalState(int id, int maxDepth)
    {
        return this.genService
            .GetFinalState(id, maxDepth)
            .Map(board => board?.ToFlatGameBoard())!;
        
    }

    [HttpPost]
    [Route("SaveBoard")]
    public ActionResult<int> SaveBoard([FromBody] FlatGameBoard board)
    {
        return this.genService.SaveBoard(board);
    }


    [HttpGet]
    [Route("NewRandom/{width}/{height}")]
    public ActionResult<FlatGameBoard> NewRandom(int width, int height)
    {
        return this.genService.NewRandomBoard(width, height).ToFlatGameBoard();
    }
}
