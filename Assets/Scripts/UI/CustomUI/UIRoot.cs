using UnityEngine;
using Zenject;

public class UIRoot : MonoBehaviour
{
    private Vector3 initialScale;
    [Inject] private SignalBus signalBus;
    [Inject(Id = "ReferenceCamSize")] private float referenceCameraSize;
    private void Awake()
    {
        signalBus.Subscribe<CameraSetupSignal>(AdjustRoot);
        initialScale = transform.localScale;
    }
    public void AdjustRoot()
    {
        AdjustPosition();
        AdjustScale();
    }
    private void AdjustPosition()
    {
        Vector3 targetPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        transform.position = targetPos;
    }
    private void AdjustScale()
    {
        float cameraSize = Camera.main.orthographicSize;
        float scaler = cameraSize / referenceCameraSize;
        Vector3 targetScale = new Vector3(initialScale.x * scaler, initialScale.y * scaler, initialScale.z);
        transform.localScale = targetScale;
    }
}
