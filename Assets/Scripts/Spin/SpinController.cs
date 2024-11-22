using System.Collections;
using UnityEngine;
using Zenject;

public class SpinController : MonoBehaviour
{
    private float currentSpeed;
    [Inject] private SignalBus signalBus;
    [Inject] private SpinConfig spinConfig;

    [Inject] public SpinStateMachine StateMachine;
    [Inject (Id = "Begin")] public SpinBeginState BeginState;
    [Inject(Id = "Idle")] public SpinIdleState IdleState;
    [Inject(Id = "Stop")] public SpinStopState StopState;

    public float CurrentSpeed { get => currentSpeed; set => currentSpeed = value; }
   
    private void Update()
    {
        StateMachine.CurrentState.OnUpdate();
    }
    public void StartSpin()
    {
        StateMachine.ChangeState(BeginState);
    }
    public void StopSpin()
    {
        StateMachine.ChangeState(StopState);
    }
    public IEnumerator Accelerate()
    {
        float t = 0;
        float initialSpeed = currentSpeed;

        while (t < 1)
        {
            currentSpeed = Mathf.Lerp(initialSpeed, spinConfig.MaxSpeed, t);
            t += Time.deltaTime / spinConfig.AccelerationTime;
            yield return null;
        }

        currentSpeed = spinConfig.MaxSpeed;
        signalBus.TryFire<SpinReachMaximumSpeedSignal>();
    }
    public IEnumerator Decelerate()
    {
        float t = 0;
        float initialSpeed = currentSpeed;

        while (t < 1)
        {
            currentSpeed = Mathf.Lerp(initialSpeed, spinConfig.MinSpeed, t);
            t += Time.deltaTime / spinConfig.AccelerationTime;
            yield return null;
        }

        currentSpeed = spinConfig.MinSpeed;
    }
}

public class SpinReachMaximumSpeedSignal { }
