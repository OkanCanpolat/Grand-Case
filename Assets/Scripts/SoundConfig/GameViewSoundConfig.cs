using UnityEngine;

[CreateAssetMenu(fileName = "GameViewSoundConfig", menuName = "Sound Configs/Game View Sound Config")]
public class GameViewSoundConfig : ScriptableObject
{
    public AudioClip BackgroundSound;
    [Range(0, 1)]
    public float BackgroundVolume = 1;

    public AudioClip ButtonClickSound;
    [Range(0, 1)]
    public float ButtonClickVolume = 1;

    public AudioClip SpinSound;
    [Range(0, 1)]
    public float SpinVolume = 1;

    public AudioClip SpinStopSound;
    [Range(0, 1)]
    public float SpinStopVolume = 1;

    public AudioClip SwipeSound;
    [Range(0, 1)]
    public float SwipeVolume = 1;

    public AudioClip MatchSound;
    [Range(0, 1)]
    public float MatchVolume = 1;
}
