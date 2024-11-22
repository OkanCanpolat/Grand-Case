using UnityEngine;

[CreateAssetMenu(fileName = "LoseScreenSoundConfig", menuName = "Sound Configs/Lose Screen Sound Config")]

public class LoseScreenSoundConfig : ScriptableObject
{
    public AudioClip OutOfMovesSound;
    [Range(0, 1)]
    public float OutOfMovesVolume = 1;

    public AudioClip LoseSound;
    [Range(0, 1)]
    public float LoseVolume = 1;

    public AudioClip StampSound;
    [Range(0, 1)]
    public float StampVolume = 1;
}
