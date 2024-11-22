using UnityEngine;
using Zenject;

public class SpinBeginState : ISpinState
{
    private float moveValue;
    private float elapsedTime;
    private SpinController spinController;
    private SignalBus signalBus;
    [Inject] private IAudioService audioService;
    [Inject] private GameViewSoundConfig soundConfig;
    [Inject] private Board board;
    public float ElapsedTime => elapsedTime;
    public SpinBeginState(SpinController spinController, SignalBus signalBus)
    {
        this.spinController = spinController;
        this.signalBus = signalBus;
    }
    public void OnEnter()
    {
        moveValue = 0;
        elapsedTime = 0;
        spinController.CurrentSpeed = 0;
        board.CreateExtraLine();
        spinController.StartCoroutine(spinController.Accelerate());
        signalBus.TryFire<SpinStartSignal>();

        audioService.FadeIn(1, soundConfig.SpinSound);
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
            elapsedTime = 0;
            board.DestroyBottom();
            board.DecreaseRows();
            board.CreateExtraLine();
        }
    }
}

public class SpinStartSignal { }

