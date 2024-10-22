using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonWithSound : Button
{
    private bool pointerWasUp;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySfx("按钮点击");
        base.OnPointerClick(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        pointerWasUp = true;
        base.OnPointerUp(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (pointerWasUp)
        {
            pointerWasUp = false;
        }

        AudioManager.Instance.PlaySfx("按钮悬浮");
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        pointerWasUp = false;
        base.OnPointerExit(eventData);
    }
}
