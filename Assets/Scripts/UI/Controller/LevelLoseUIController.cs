using System.Collections;
using UnityEngine;
using Zenject;

public class LevelLoseUIController : MonoBehaviour
{
    [SerializeField] private GameObject losePopup;
    [Inject] private LoseScrenConfig config;
    [Inject] private SignalBus signalBus;
    [Inject] private IAudioService audioService;
    [Inject] private LoseScreenSoundConfig soundConfig;
    private void Awake()
    {
        signalBus.Subscribe<LevelFailedSignal>(OnLose);
    }

    private void OnLose()
    {
        StartCoroutine(CoOnLose());
    }
    private IEnumerator CoOnLose()
    {
        audioService.PlaySoundOnce(soundConfig.OutOfMovesSound, soundConfig.OutOfMovesVolume);
        yield return new WaitForSeconds(config.LoseScreenStartDelay);
        audioService.PlaySoundOnce(soundConfig.LoseSound, soundConfig.LoseVolume);
        losePopup.SetActive(true);
        yield return new WaitForSeconds(config.StampSoundDelay);
        audioService.PlaySoundOnce(soundConfig.StampSound, soundConfig.StampVolume);
        yield return new WaitForSeconds(config.LoseScreenCloseDelay);
        losePopup.SetActive(false);
        signalBus.TryFire<RestartGameSignal>();
    }
}
