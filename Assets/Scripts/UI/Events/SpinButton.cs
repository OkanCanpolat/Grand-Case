using UnityEngine;
using Zenject;

[RequireComponent (typeof (CustomButton))]
public class SpinButton : MonoBehaviour
{
    public CustomButton Button;
    [Inject] private SpinController spinController;
    [Inject] private SignalBus signalBus;
    [Inject] private GameViewSoundConfig soundConfig;
    [Inject] private IAudioService audioService;
    private void Awake()
    {
        signalBus.Subscribe<SpinStopSignal>(() => Button.Enable());
        signalBus.Subscribe<RestartGameSignal>(() => Button.Enable());
        signalBus.Subscribe<SwipeStartSignal>(OnSwipe);
        signalBus.Subscribe<RestartGameSignal>(OnRestartGame);
    }

    public void OnClick()
    {
        audioService.PlaySoundOnce(soundConfig.ButtonClickSound, soundConfig.ButtonClickVolume);
        spinController.StartSpin();
        Button.Disable();
    }
    private void OnSwipe()
    {
        Button.Disable();
        signalBus.Unsubscribe<SwipeStartSignal>(OnSwipe);
    }
    private void OnRestartGame()
    {
        signalBus.Subscribe<SwipeStartSignal>(OnSwipe);
    }
}
