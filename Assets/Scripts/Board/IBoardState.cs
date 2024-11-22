using UnityEngine;

public interface IBoardState 
{
    public void OnEnter();
    public void OnExit();
    public void OnClickTile(TileBase tile, Vector3 clickPosition);
    public void OnReleaseTile(TileBase tile, Vector3 releasePosition);
}
