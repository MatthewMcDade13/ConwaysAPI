using System.Data;
using Conways_API.Common;
using Dapper;
using DataAccess.Common;
using DataAccess.Models;
using Microsoft.Extensions.ObjectPool;

namespace Conways_API;

public static class GameBoardService {

    private static readonly Random rand = new Random();

    public static GameBoard NewBoardFromFile(string filePath) {
        var boardRows = File.ReadAllLines(filePath).Select(s => s.Trim()).ToArray();
        return NewBoardFromRows(boardRows);
    }

    public static GameBoard NewBoardFromString(string board) {
        var boardRows = board.Split('\n');
        return NewBoardFromRows(boardRows);
    }

    public static GameBoard NewBoardFromRows(IEnumerable<string> rows) {
        var boardRows = rows.AsList();

        var width = boardRows.First().Count();
        var height = boardRows.Count();
        var board = new GameBoard(width, height);

        board.ForEach((c, x, y, i) => {
            var cellState = boardRows[y][x] == 'X' ? CellState.Alive : CellState.Dead;
            // Console.WriteLine($"{board.State.Count} | {i}");
            board.State[i] = new Cell {
                State = cellState,
                Position = new Point {
                    X = x,
                    Y = y
                },
                Age = 0,
            };
        });
        return board;
    }

    public static GameBoard NextState(GameBoard board) {
        var nextBoard = board.Clone();

        board.ForEach((cell, x, y, _) => {
            var aliveNeighbors = CountAliveCellNeighbors(board, cell);
            if (cell.State == CellState.Alive) {
                // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
                // Any live cell with more than three live neighbours dies, as if by overpopulation.
                if (aliveNeighbors < 2 || aliveNeighbors > 3) {
                    var nextCell = nextBoard.GetCell(x, y);
                    nextCell.Kill();
                }
            } else {
                // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                if (aliveNeighbors == 3) {
                    var nextCell = nextBoard.GetCell(x, y);
                    nextCell.State = CellState.Alive;
                    nextCell.Age += 1;
                }
            }
        });

        return nextBoard;
    }

    public static GameBoard NextNStates(GameBoard board, int n) {
        GameBoard nextBoard = board;
        for (int i = 0; i < n; i++) {
            nextBoard = NextState(nextBoard);
        }
        return nextBoard;
    }

    public static Result<GameBoard> GetFinalState(GameBoard board, int maxDepth) {
        var currentBoard = board;
        for (int i = 0; i < maxDepth; i ++) {
            var nextBoard = NextState(currentBoard);
            // If we havent changed at all, we have a terminal state
            if (currentBoard.Equals(nextBoard)) {
                return Result<GameBoard>.Ok(nextBoard);
            }
            currentBoard = nextBoard;
        }
        return Result<GameBoard>.Error($"Max depth reached, unable to find terminal state with given maxDepth: {maxDepth}");
    }

    public static int CountAliveCellNeighbors(GameBoard board, Cell cell) {

        var neighbors = GetCellNeighbors(board, cell).AsList();
        for (int i = 0; i < neighbors.Count; i++) {
            if (cell.State == CellState.Alive) {
                Console.WriteLine($"|{cell.Position}| {neighbors[i].Position} {neighbors[i].State}");
            }
        }

        return neighbors.Aggregate(0, (acc, neighbor) => acc + (neighbor.State == CellState.Alive ? 1 : 0));
    }

    public static IEnumerable<Cell> GetCellNeighbors(GameBoard board, Cell cell) {
        var pos = cell.Position;
        return new List<Cell> {
            // top left
            board.GetCell(pos.X - 1, pos.Y + 1),
            // top
            board.GetCell(pos.X, pos.Y - 1),
            // top right
            board.GetCell(pos.X + 1, pos.Y + 1),
            // right
            board.GetCell(pos.X + 1, pos.Y),
            // bottom right
            board.GetCell(pos.X + 1, pos.Y - 1),
            // bottom
            board.GetCell(pos.X, pos.Y + 1),
            // bottom left
            board.GetCell(pos.X -1 , pos.Y -1),
            // left
            board.GetCell(pos.X - 1, pos.Y)
        };
    }

    public static GameBoard RandomBoard(int width, int height) {
        var size = width * height;
        var cells = new List<Cell>(size);
        double spawnChance = GameBoardService.rand.NextDouble();

        var board = new GameBoard(width, height);
        board.ForEach((c, x, y, i) => {
            var aliveChance = GameBoardService.rand.NextDouble();
            board.State[i] = new Cell {
                Age = 0,
                State = aliveChance < spawnChance ? CellState.Alive : CellState.Dead,
                Position = new Point {
                    X = x, Y = y
                },                    
            };
        });
        return board;
    }
}