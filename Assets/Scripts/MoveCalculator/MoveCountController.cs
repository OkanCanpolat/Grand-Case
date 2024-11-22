using System;
using UnityEngine;
using Zenject;

public class MoveCountController : MonoBehaviour
{
    public Action<int> OnMoveCountChange;
    [Inject] private IMinMoveCalculator calculator;
    [Inject] private SignalBus signalBus;
    private int remainingMoveCount;
    private void Awake()
    {
        signalBus.Subscribe<BoardCreateSignal>(CalculateMoveCount);
        signalBus.Subscribe<SpinStopSignal>(CalculateMoveCount);
        signalBus.Subscribe<UnvalidSwipeSignal>(OnUnvalidMatch);

    }

    private void CalculateMoveCount()
    {
        remainingMoveCount = calculator.GetMinimumMoveToMatch();
        OnMoveCountChange?.Invoke(remainingMoveCount);
    }

    private void OnUnvalidMatch()
    {
        remainingMoveCount--;
        OnMoveCountChange?.Invoke(remainingMoveCount);

        if(remainingMoveCount <= 0)
        {
            signalBus.TryFire<LevelFailedSignal>();
        }
    }
}

public class LevelFailedSignal { }
