using UnityEngine;
using Zenject;

[RequireComponent (typeof(Camera))]
public class CameraResizer : MonoBehaviour
{
    public float Padding;
    public float CameraZOffset;
    [Inject] private Board board;
    [Inject] private SignalBus signalBus;
    private Camera mainCam;
    private void Awake()
    {
        mainCam = GetComponent<Camera>();
        signalBus.Subscribe<BoardCreateSignal>(SetupCamera);
    }

    private void SetupCamera()
    {
        float aspectRatio = Screen.width / (float)Screen.height;

        float width = board.Width;
        float height = board.Height;

        mainCam.transform.position = new Vector3((width - 1) / 2, (height - 1) / 2, CameraZOffset);

        if (width >= height)
        {
            mainCam.orthographicSize = (width / 2f + Padding) / aspectRatio;
        }

        else
        {
            float size = (width / 2f + Padding) / aspectRatio;
            Debug.Log(aspectRatio);
            if(size <= height / 2f)
            {
                mainCam.orthographicSize = height  + Padding;
            }

            else
            {
                mainCam.orthographicSize = (width / 2f + Padding) / aspectRatio;
            }
        }

        signalBus.TryFire<CameraSetupSignal>();
    }
}

public class CameraSetupSignal { }
