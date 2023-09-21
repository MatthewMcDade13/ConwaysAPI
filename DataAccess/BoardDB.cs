namespace DataAccess;

using System;
using System.Data;
using System.Net.NetworkInformation;
using Dapper;
using DataAccess.Models;
using Microsoft.Data.Sqlite;
 
public class BoardDB {

    public string ConnectionString { get; }


    public BoardDB(string dbFilePath)
    {

        ConnectionString = $"Data Source={dbFilePath}";
        using (var conn = new SqliteConnection(ConnectionString)) {
            conn.Open();
            conn.Execute(@"
                CREATE TABLE IF NOT EXISTS board (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    width INTEGER NOT NULL,
                    height INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS board_cell (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    pos_x INTEGER NOT NULL,
                    pos_y INTEGER NOT NULL,
                    age INTEGER NOT NULL,
                    is_alive BOOLEAN NOT NULL,
                    board_id INTEGER NOT NULL,
                    FOREIGN KEY (board_id)
                        REFERENCES board (id)
                        ON DELETE CASCADE
                );
            ");
        }
        
    }


    public int SaveBoard(GameBoard board) {
        int boardId = 0;
        using (var conn = new SqliteConnection(ConnectionString)) {
            conn.Open();

            boardId = conn.ExecuteScalar<int>(@"
                INSERT INTO board (width, height)
                VALUES(@width, @height)
                RETURNING id;
            ", new { width = board.Width, height = board.Height });

            var cells = board.State.Select(cell => new {
                pos_x = cell.Position.X,
                pos_y = cell.Position.Y,
                age = cell.Age,
                is_alive = cell.State == CellState.Alive,
                board_id = boardId,
            });

            conn.Execute(@"
                INSERT INTO board_cell (pos_x, pos_y, age, is_alive, board_id)
                VALUES(@pos_x, @pos_y, @age, @is_alive, @board_id);
            ", cells);
        }
        return boardId;
    }

    public GameBoard GetBoard(int boardId) {
        using (var conn = new SqliteConnection(ConnectionString)) {
            conn.Open();

            var queryResult = conn.Query<BoardCellQueryResult>(@"
                SELECT 
                    c.pos_x as PosX, 
                    c.pos_y as PosY, 
                    c.age as Age, 
                    c.is_alive as IsAlive,
                    b.width as BoardWidth,
                    b.height as BoardHeight
                FROM board_cell as c
                JOIN board as b
                ON b.id = c.board_id
                WHERE c.board_id = @board_id;
            ", new {board_id = boardId}).AsList();

            var width = queryResult.First().BoardWidth;
            var height = queryResult.First().BoardHeight;

            var cells = queryResult.Select(res => new Cell {
                Age = res.Age,
                Position = new Point {
                    X = res.PosX,
                    Y = res.PosY
                },
                State = res.IsAlive ? CellState.Alive : CellState.Dead,
            }).ToList();

            return new GameBoard(width, height, cells);
        }
    }

    private class BoardCellQueryResult {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Age { get; set; }
        public bool IsAlive { get; set; }
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
    }
}
