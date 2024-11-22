using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(MoveCountController))]
public class MoveCountUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text moveText;
    [Inject] private MoveCountController moveCountController;

    private void Awake()
    {
        moveCountController.OnMoveCountChange += OnTextChanged;
    }

    private void OnTextChanged(int value)
    {
        moveText.text = value.ToString();
    }
}
