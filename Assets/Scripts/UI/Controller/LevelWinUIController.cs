using System.Collections;
using UnityEngine;
using Zenject;

public class LevelWinUIController : MonoBehaviour
{
    [SerializeField] private GameObject congratsPopup;
    [SerializeField] private GameObject fireworkParticleParent;
    [SerializeField] private GameObject gameNameTextContainer;
    [SerializeField] private GameObject winPopup;
    [Inject] private IAudioService audioService;
    [Inject] private SignalBus signalBus;
    [Inject] private WinScreenConfig winScreenConfig;
    [Inject] private WinScreenSoundConfig soundConfig;

    private void Awake()
    {
        signalBus.Subscribe<MatchFoundSignal>(OnMatch);
    }
    private void OnMatch()
    {
        StartCoroutine(CoOnMatch());
    }
    private IEnumerator CoOnMatch()
    {
        yield return new WaitForSeconds(winScreenConfig.CongratsScreenStartDelay);
        audioService.PlaySoundOnce(soundConfig.WinSound, soundConfig.WinVolume);
        congratsPopup.SetActive(true);
        gameNameTextContainer.SetActive(true);
        yield return new WaitForSeconds(winScreenConfig.FireworksStartDelay);
        audioService.PlaySoundOnce(soundConfig.FireworkSound, soundConfig.FireworkVolume);
        fireworkParticleParent.SetActive(true);
        yield return new WaitForSeconds(winScreenConfig.FireworkCompleteTime);
        gameNameTextContainer.SetActive(false);
        fireworkParticleParent.SetActive(false);
        winPopup.SetActive(true);
        audioService.PlaySoundOnce(soundConfig.GoldSound, soundConfig.GoldVolume);
        yield return new WaitForSeconds(winScreenConfig.WinScreenCompleteTime);
        winPopup.SetActive(false);
        congratsPopup.SetActive(false);
        signalBus.TryFire<RestartGameSignal>();
    }
}

public class RestartGameSignal { }
