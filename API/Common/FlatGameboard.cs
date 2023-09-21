using DataAccess.Models;
namespace Conways_API.Common;

public record FlatGameBoard(string BoardFlatGrid, int Width, int Height) {
    public GameBoard ToGameBoard() {
        var rows = BoardFlatGrid.Chunk(Width).ToArray();
        // for (int i = 0; i < rows.Count(); i++) {
        //     System.Console.WriteLine(new string(rows[i]));
        // }
        return GameBoardService.NewBoardFromRows(rows.Select(cs => new string(cs)));
    }

}


public static class BoardExtensions {
    public static FlatGameBoard ToFlatGameBoard(this GameBoard board) {
        return new FlatGameBoard(board.ToFlatGridString(), board.Width, board.Height);
    }
}
