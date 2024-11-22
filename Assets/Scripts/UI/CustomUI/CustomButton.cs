using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class CustomButton : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public bool Interactable = true;

    public Color NormalColor = Color.white;
    public Color HoverColor = Color.white;
    public Color PressedColor = Color.white;
    public Color DisabledColor = Color.white;

    public Sprite NormalSprite;
    public Sprite HoverSprite;
    public Sprite PressedSprite;
    public Sprite DisabledSprite;

    public UnityEvent OnClick;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (NormalSprite == null) NormalSprite = spriteRenderer.sprite;
    }
    private void OnEnable()
    {
        if (Interactable)
        {
            spriteRenderer.color = NormalColor;
            spriteRenderer.sprite = NormalSprite;
        }
        else
        {
            spriteRenderer.color = DisabledColor;
            spriteRenderer.sprite = DisabledSprite == null ? NormalSprite : DisabledSprite;
        }
    }
    private void OnMouseEnter()
    {
        if (!Interactable) return;

        spriteRenderer.color = HoverColor;

        if (HoverSprite != null)
        {
            spriteRenderer.sprite = HoverSprite;
        }
    }
    private void OnMouseExit()
    {
        if (!Interactable) return;

        spriteRenderer.color = NormalColor;
        spriteRenderer.sprite = NormalSprite;
    }
    private void OnMouseDown()
    {
        if (!Interactable) return;

        spriteRenderer.color = PressedColor;

        if (PressedSprite != null)
        {
            spriteRenderer.sprite = PressedSprite;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (!Interactable) return;

        spriteRenderer.color = HoverColor;

        if (HoverSprite != null)
        {
            spriteRenderer.sprite = HoverSprite;
        }
        else
        {
            spriteRenderer.sprite = NormalSprite;
        }
        OnClick?.Invoke();
    }

    public void Enable()
    {
        Interactable = true;
        spriteRenderer.color = NormalColor;
        spriteRenderer.sprite = NormalSprite;
    }
    public void Disable()
    {
        Interactable = false;
        spriteRenderer.color = DisabledColor;
        spriteRenderer.sprite = DisabledSprite == null ? NormalSprite : DisabledSprite;
    }
}
