namespace DataAccess.Models;

public class Cell : IEquatable<Cell> {

    public CellState State { get; set; } = CellState.Dead;
    public int Age { get; set; }
    public Point Position { get; set; } = new Point { X = 0, Y = 0 };

    public Cell Clone()
    {
        return new Cell {
            State = State,
            Age = Age,
            Position = Position
        };
    }

    public bool Equals(Cell? other)
    {
        if (other == null) {
            return false;
        }
        // Ignore age equality. Cells from difference states on the same board
        // will have different ages. We only care that the position and State is the same
        // (really just the State, but position ensures we have the same cell from the other generation)
        return State == other.State && Position == other.Position;
    }
    public override bool Equals(object? obj) => Equals(obj as Cell);
    public override int GetHashCode() => HashCode.Combine(State, Position);

    public void Kill() {
        State = CellState.Dead;
        Age = 0;
    }
}


public enum CellState {
    Dead = 0,
    Alive
}

public record Point {
    public int X { get; set; }
    public int Y { get; set; }
}