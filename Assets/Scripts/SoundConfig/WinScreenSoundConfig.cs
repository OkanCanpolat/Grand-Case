using UnityEngine;

[CreateAssetMenu(fileName = "WinScreenSoundConfig", menuName = "Sound Configs/Win Screen Sound Config")]
public class WinScreenSoundConfig : ScriptableObject
{
    public AudioClip WinSound;
    [Range(0, 1)]
    public float WinVolume = 1;

    public AudioClip FireworkSound;
    [Range(0, 1)]
    public float FireworkVolume = 1;

    public AudioClip GoldSound;
    [Range(0, 1)]
    public float GoldVolume = 1;
}
