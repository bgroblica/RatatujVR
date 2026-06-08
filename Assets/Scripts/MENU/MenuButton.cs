using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    public SpriteRenderer sr;

    public Sprite normalSprite;
    public Sprite hoverSprite;
    public Sprite pressedSprite;
    public Sprite selectedSprite;

    public bool staySelected;

    public UnityEvent onPressed;

    private XRSimpleInteractable interactable;
    private bool isSelected;

    private void Awake()
    {
        interactable =
            GetComponent<XRSimpleInteractable>();

        sr.sprite =
            normalSprite;
    }

    private void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);

        interactable.selectEntered.AddListener(OnSelectEnter);
        interactable.selectExited.AddListener(OnSelectExit);
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEnter);
        interactable.hoverExited.RemoveListener(OnHoverExit);

        interactable.selectEntered.RemoveListener(OnSelectEnter);
        interactable.selectExited.RemoveListener(OnSelectExit);
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (isSelected)
            return;

        sr.sprite =
            hoverSprite;
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        if (isSelected)
            return;

        sr.sprite =
            normalSprite;
    }

    public void OnSelectEnter(SelectEnterEventArgs args)
    {
        sr.sprite =
            pressedSprite;

        onPressed?.Invoke();

        if (staySelected)
        {
            isSelected = true;

            sr.sprite =
                    selectedSprite;
        }
        Debug.Log("Selected");
    }

    public void OnSelectExit(SelectExitEventArgs args)
    {
        if (isSelected)
            return;

        sr.sprite =
            selectedSprite;
    }
}