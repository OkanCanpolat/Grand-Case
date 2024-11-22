using UnityEngine;
using Zenject;

[RequireComponent (typeof (CustomButton))]
public class StopButton : MonoBehaviour
{
    public CustomButton Button;
    [Inject] private SpinController spinController;
    [Inject] private SignalBus signalBus;
    [Inject] private GameViewSoundConfig soundConfig;
    [Inject] private IAudioService audioService;
    private void Awake()
    {
        signalBus.Subscribe<SpinReachMaximumSpeedSignal>(() => Button.Enable());
    }
    public void OnClick()
    {
        audioService.PlaySoundOnce(soundConfig.ButtonClickSound, soundConfig.ButtonClickVolume);
        spinController.StopSpin();
        Button.Disable();
    }
}
