using UnityEngine;

[CreateAssetMenu(fileName = "Spin Config", menuName = "Configurations/Spin Config")]

public class SpinConfig : ScriptableObject
{
    public float MaxSpeed;
    public float MinSpeed;
    public float AccelerationTime;
}
