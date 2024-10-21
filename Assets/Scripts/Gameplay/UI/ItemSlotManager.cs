using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotManager : MonoSingleton<ItemSlotManager>
{
    public Vector2 initScl = new Vector2(30f, 30f);
    public Vector2 originalScl = new Vector2(80f, 80f);
    public Vector2 closeDir = new Vector2(-100f, -80f);
    private Vector2 originPos;
    private Vector2 closePos;

    public GameObject slotPrefab;
    private List<ItemSlot> slots = new List<ItemSlot>();
    private Dictionary<string, ItemSlot> nameSlotDict = new Dictionary<string, ItemSlot>();
    private ItemSlot selectedSlot;

    void Start()
    {
        originPos = transform.localPosition;
        closePos = originPos + closeDir;
    }

    void OnEnable()
    {
        MyEventSystem.Register<StateChangeEvent>(OnChangeState);
        MyEventSystem.Register<SelectItemEvent>(OnSelectItem);
        MyEventSystem.Register<UseItemEvent>(OnUseItem);
        MyEventSystem.Register<CannotUseItemEvent>(OnCannotUseItem);
        MyEventSystem.Register<GetItemEvent>(OnGetItem);
    }
    
    void OnDisable()
    {
        MyEventSystem.Unregister<StateChangeEvent>(OnChangeState);
        MyEventSystem.Unregister<SelectItemEvent>(OnSelectItem);
        MyEventSystem.Unregister<CannotUseItemEvent>(OnCannotUseItem);
        MyEventSystem.Unregister<UseItemEvent>(OnUseItem);
        MyEventSystem.Unregister<GetItemEvent>(OnGetItem);
    }

    public void MakeSlot(GameItemData data, int count)
    {
        if (data.isEmpty) return;
        GameObject slotObj = GameObject.Instantiate(slotPrefab, transform);
        ItemSlot slot = new ItemSlot
        {
            gameObject = slotObj, icon = slotObj.transform.GetChild(0).GetComponent<Image>() , cntText = slotObj.GetComponentInChildren<TMP_Text>()
        };
        slots.Add(slot);
        nameSlotDict.Add(data.name, slot);

        slot.icon.sprite = data.icon;
        slot.cntText.gameObject.SetActive(data.hasNumberLimit);
        RectTransform rectTransform = slot.gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = initScl;
        ScaleToOrigin(slot.gameObject);

        if (data.hasNumberLimit) slot.cntText.text = count.ToString();
    }
    
    public void OnChangeState(StateChangeEvent @event)
    {
        if (@event.newState == PlayerState.Soul)
        {
            transform.DOLocalMove(closePos, 0.5f).SetUpdate(true).SetEase(Ease.OutQuint);
        }
        else
        {
            transform.DOLocalMove(originPos, 0.5f).SetUpdate(true).SetEase(Ease.OutQuint);
        }
    }

    private void ScaleToDoubleOrigin(GameObject go)
    {
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.DOSizeDelta(originalScl * 1.5f, 0.5f).SetUpdate(true).SetEase(Ease.InOutBack);
    }

    private void ScaleToOrigin(GameObject go)
    {
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.DOSizeDelta(originalScl, 0.5f).SetUpdate(true).SetEase(Ease.InOutBack);
    }

    private void OnSelectItem(SelectItemEvent @event)
    {
        if (!@event.itemData.isEmpty)
        {
            if (!nameSlotDict.ContainsKey(@event.itemData.name)) return;

            ItemSlot newSlot = nameSlotDict[@event.itemData.name];
            ScaleToDoubleOrigin(newSlot.gameObject);
            if (selectedSlot != null && selectedSlot != newSlot) ScaleToOrigin(selectedSlot.gameObject);

            selectedSlot = newSlot;
        }
        else
        {
            if (selectedSlot != null) ScaleToOrigin(selectedSlot.gameObject);
            selectedSlot = null;
        }
    }
    
    private void OnUseItem(UseItemEvent @event)
    {
        if (!nameSlotDict.ContainsKey(@event.itemData.name)) return;
        if (@event.itemData.hasNumberLimit)
        {
            nameSlotDict[@event.itemData.name].cntText.text = @event.newCount.ToString();
        }
    }

    private void OnCannotUseItem(CannotUseItemEvent @event)
    {
        if (!nameSlotDict.ContainsKey(@event.itemData.name)) return;
        nameSlotDict[@event.itemData.name].icon.transform.DOShakePosition(0.6f, 8f, randomness: 30);
    }
    
    private void OnGetItem(GetItemEvent @event)
    {
        if (!nameSlotDict.ContainsKey(@event.itemData.name))
        {
            MakeSlot(@event.itemData, @event.newCount);
        }
        if (@event.itemData.hasNumberLimit)
        {
            nameSlotDict[@event.itemData.name].cntText.text = @event.newCount.ToString();
        }
    }
}

public class ItemSlot
{
    public GameObject gameObject;
    public Image icon;
    public TMP_Text cntText;
}