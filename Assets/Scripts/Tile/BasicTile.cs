using UnityEngine;
using Zenject;

public class BasicTile : TileBase
{
    [Inject] private Board board;
    
    private void OnMouseDown()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        board.OnClickTile(this, clickPos);
    }
    private void OnMouseUp()
    {
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        board.OnReleaseTile(this, clickPos);
    }
    public override bool FindMatch(bool mark = false)
    {
        return matchFinder.FindMatch(this, mark);
    }
}
