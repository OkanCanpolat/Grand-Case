public class BasicMatchFinder : IMatchFinder
{
    private Board board;
    public BasicMatchFinder(Board board)
    {
        this.board = board;
    }
    public bool FindMatch(TileBase tile, bool mark = false)
    {
        return FindHorizontalMatch(tile, mark) | FindVerticalMatch(tile, mark);
    }

    private bool FindHorizontalMatch(TileBase tile, bool mark = false)
    {
        bool matchFound = false;

        TileBase right1 = board.GetTile(tile.Column + 1, tile.Row);
        TileBase right2 = board.GetTile(tile.Column + 2, tile.Row);
        TileBase left1 = board.GetTile(tile.Column - 1, tile.Row);
        TileBase left2 = board.GetTile(tile.Column - 2, tile.Row);

        matchFound = ControlMatch(right1, right2, tile, mark) | matchFound;
        matchFound = ControlMatch(left1, left2, tile, mark) | matchFound;
        matchFound = ControlMatch(left1, right1, tile, mark) | matchFound;

        return matchFound;
    }
    private bool FindVerticalMatch(TileBase tile, bool mark = false)
    {
        bool matchFound = false;

        TileBase up1 = board.GetTile(tile.Column, tile.Row + 1);
        TileBase up2 = board.GetTile(tile.Column, tile.Row + 2);
        TileBase down1 = board.GetTile(tile.Column, tile.Row - 1);
        TileBase down2 = board.GetTile(tile.Column, tile.Row - 2);

        matchFound = ControlMatch(up1, up2, tile, mark) | matchFound;
        matchFound = ControlMatch(down1, down2, tile, mark) | matchFound;
        matchFound = ControlMatch(down1, up1, tile, mark) | matchFound;

        return matchFound;
    }

    private bool ControlMatch(TileBase first, TileBase second, TileBase source, bool mark = false)
    {
        if (first != null && second != null && first.Type == source.Type && second.Type == source.Type)
        {
            if (mark)
            {
                first.IsMatched = true;
                second.IsMatched = true;
                source.IsMatched = true;
            }

            return true;
        }
        return false;
    }
}
