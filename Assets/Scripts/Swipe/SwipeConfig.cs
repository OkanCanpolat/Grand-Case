using UnityEngine;

[CreateAssetMenu (fileName = "Swipe Config", menuName = "Configurations/Swipe Config")]
public class SwipeConfig : ScriptableObject
{
    public float MinDistanceToSwipe;
    public float SwipeTime;

    [Header("Punch Animation")]
    public Vector3 PunchScale;
    public Vector3 PunchRotation;

    [Header("Match Animation")]
    public float ScalePingPongScaler;
    public float MatchAnimationTime;
    public float MatchAnimationDelay;
    public float MatchAnimationScaleTime;
}
