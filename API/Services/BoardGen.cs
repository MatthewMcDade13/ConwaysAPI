using Conways_API.Common;
using DataAccess;
using DataAccess.Common;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Conways_API.Services;

public class BoardGenService : IBoardGenService {
    private readonly BoardDB boardDB;

    public BoardGenService()
    {
        this.boardDB = new BoardDB("board.db");
    }

    public GameBoard Get(int id)
    {
        return this.boardDB.GetBoard(id);
    }

    public GameBoard GetNext(int id)
    {
        var board = this.boardDB.GetBoard(id);
        return GameBoardService.NextState(board);
    }

    public GameBoard GetNextN(int id, int n)
    {
        var board = this.boardDB.GetBoard(id);
        return GameBoardService.NextNStates(board, n);
    }

    public Result<GameBoard> GetFinalState(int id, int maxDepth)
    {
        var board = this.boardDB.GetBoard(id);
        return GameBoardService.GetFinalState(board, maxDepth);        
    }

    public int SaveBoard(FlatGameBoard saveBoard)
    {        
        var gameBoard = saveBoard.ToGameBoard();
        return this.boardDB.SaveBoard(gameBoard);
    }

    public GameBoard NewRandomBoard(int width, int height)
    {
        return GameBoardService.RandomBoard(width, height);
    }
}

public interface IBoardGenService {
    GameBoard Get(int id);
    GameBoard GetNext(int id);
    GameBoard GetNextN(int id, int n);
    GameBoard NewRandomBoard(int width, int height);
    Result<GameBoard> GetFinalState(int id, int maxDepth);
    int SaveBoard(FlatGameBoard board);
}/*  */