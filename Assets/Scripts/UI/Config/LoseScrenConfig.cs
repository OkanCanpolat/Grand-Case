using UnityEngine;

[CreateAssetMenu(fileName = "Lose Screen Config", menuName = "Configurations/Lose Screen Config")]
public class LoseScrenConfig : ScriptableObject
{
    public float LoseScreenStartDelay;
    public float LoseScreenCloseDelay;
    public float StampSoundDelay;
}
