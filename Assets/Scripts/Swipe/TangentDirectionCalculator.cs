using UnityEngine;

public class TangentDirectionCalculator : ISwipeDirectionCalculator
{
    public Vector2 GetDirection(Vector3 touch, Vector3 release)
    {
        float swipeAngle = CalculateTangent(touch, release);
        Vector2 direction = GetTangentResult(swipeAngle);
        return direction;
    }

    private  float CalculateTangent(Vector2 first, Vector2 second)
    {
        float swipeAngle = Mathf.Atan2(second.y - first.y, second.x - first.x);
        swipeAngle *= Mathf.Rad2Deg;
        return swipeAngle;
    }
    private  Vector2 GetTangentResult(float swipeAngle)
    {
        if (swipeAngle > -45 && swipeAngle <= 45)
        {
            return Vector2.right;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135)
        {
            return Vector2.up;
        }
        else if (swipeAngle > 135 || swipeAngle <= -135)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.down;
        }
    }
}
