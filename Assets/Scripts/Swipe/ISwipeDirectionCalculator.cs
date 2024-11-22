using UnityEngine;

public interface ISwipeDirectionCalculator 
{
    public Vector2 GetDirection(Vector3 touch, Vector3 release);
}
