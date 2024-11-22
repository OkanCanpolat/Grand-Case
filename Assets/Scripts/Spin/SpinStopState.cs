using UnityEngine;
using Zenject;

public class SpinStopState : ISpinState
{
    private float elapsedTime;
    private float moveValue;
    private SignalBus signalBus;
    private SpinController spinController;
    [Inject(Id = "Begin")] private SpinBeginState beginState;
    [Inject] private IAudioService audioService;
    [Inject] private GameViewSoundConfig soundConfig;
    [Inject] private Board board;
    [Inject] private SpinConfig spinConfig;
    public SpinStopState(SpinController spinController, SignalBus signalBus)
    {
        this.signalBus = signalBus;
        this.spinController = spinController;
    }
    public void OnEnter()
    {
        audioService.FadeOut(1, spinConfig.AccelerationTime);
        elapsedTime = beginState.ElapsedTime;
        spinController.StartCoroutine(spinController.Decelerate());
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        moveValue = spinController.CurrentSpeed * Time.deltaTime;
        elapsedTime += moveValue;

        foreach (var container in board.Containers)
        {
            TileBase tile = container.Tile;
            Vector3 targetPos = new Vector3(tile.Column, tile.Row - 1, -1);
            tile.transform.position = Vector3.MoveTowards(tile.transform.position, targetPos, moveValue);
        }

        if (elapsedTime >= 1)
        {
            if (spinController.CurrentSpeed == spinConfig.MinSpeed)
            {
                audioService.PlaySoundOnce(soundConfig.SpinStopSound, soundConfig.SpinStopVolume);
                board.DestroyBottom();
                board.DecreaseRows();
                spinController.CurrentSpeed = 0;
                signalBus.TryFire<SpinStopSignal>();
                spinController.StateMachine.ChangeState(spinController.IdleState);
                return;
            }

            elapsedTime = 0;
            board.DestroyBottom();
            board.DecreaseRows();
            board.CreateExtraLine();
        }
    }
}


public class SpinStopSignal { }

