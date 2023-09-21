using System.Collections;
using System.Text;
using System.Xml;

namespace DataAccess.Models;


public class GameBoard : IEquatable<GameBoard> {
    public List<Cell> State { get; set; }
    public int Width { get; set; }
    public int Height { get; set;}
    
    public GameBoard(int width, int height)
    {
        State = new List<Cell>(width * height);
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                State.Add(new Cell {
                    Position = new Point { X = x, Y = y },
                    Age = 0,
                    State = CellState.Dead,                    
                });
            }
        }
        Width = width;
        Height = height;
    }

    public GameBoard(int width, int height, List<Cell> cells) {
        State = cells;
        Width = width;
        Height = height;
    }

    public void ForEach(Action<Cell, int, int, int> action)
    {
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                action(GetCell(x, y), x, y, (x * Width) + y);
            }
        }
    }

    public GameBoard Clone()
    {
        var cells = State.Select(cell => cell.Clone()).ToList();

        return new GameBoard(Width, Height, cells);
    }

    public Cell GetCell(int x, int y) {
        var point = WrapCoords(x, y);

        var pos = (point.X * Width) + point.Y;
        // System.Console.WriteLine($"|{State.Count}, {Width}, {Height}| ({x}, {y}) -> {pos}");
        return State[pos];
    }


    public string ToGridString()
    {
        var str = new StringBuilder();
        for (int y = 0; y < Height; y++) {
            str.Append("\n");
            for (int x = 0; x < Width; x++) {
                var cell = GetCell(x, y);
                str.Append(cell.State == CellState.Alive ? "X " : ". ");
            }
        }
        return str.ToString();
    }

    public string ToFlatGridString() => State
        .Aggregate("", (acc, curr) => acc + (curr.State == CellState.Alive ? "X" : "."));


    public bool Equals(GameBoard? other)
    {
        if (other == null) {
            return false;
        }
        return Width == other.Width
            && Height == other.Height
            && State.SequenceEqual(other.State);
    }

    public override bool Equals(object? obj) => Equals(obj as GameBoard);
    public override int GetHashCode() => HashCode.Combine(State, Width, Height);

    private Point WrapCoords(int x, int y) {
        int ix = x;
        int iy = y;
        if (x < 0) {
            ix = (Width - 1) + x;
        }
        if (x > Width - 1) {
            ix = (Width - 1) % x;            
        }
        if (y < 0) {
            iy = (Height - 1) + y;
        }
        if (y > Height - 1) {
            iy = (Height - 1) % y;
        }
        return new Point { X = ix, Y = iy };
    }
}

