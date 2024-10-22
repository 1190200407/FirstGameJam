using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerMovement player;

    private List<GameItemData> gameItemDatas = new List<GameItemData>();
    private List<GameItemScript> gameItems = new List<GameItemScript>();
    private Dictionary<string, GameItemData> nameDataDict = new Dictionary<string, GameItemData>();
    private int curSelection = 0;
    public GameItemScript SelectedItem => curSelection <= gameItems.Count ? gameItems[curSelection] : null;

    void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        player = GetComponent<PlayerMovement>();
    }

    void OnEnable()
    {
        MyEventSystem.Register<StateChangeEvent>(OnChangeState);
    }

    void OnDisable()
    {
        MyEventSystem.Unregister<StateChangeEvent>(OnChangeState);
    }

    public void LoadItemDatas(List<GameItemData> datas)
    {
        gameItemDatas.Clear();
        //根据Data生成道具
        foreach (var data in datas)
        {
            GetItem(data);
        }

        Select(0);
    }

    public void GetItem(GameItemData data)
    {
        gameItemDatas.Add(data);
        GameItemScript script = Activator.CreateInstance(Type.GetType(data.scriptName, true)) as GameItemScript;
        script.data = data;
        gameItems.Add(script);
        nameDataDict.Add(data.name, data);
        script.OnCreate();

        MyEventSystem.Trigger(new GetItemEvent()
        {
            itemData = data, newCount = 1
        });
    }
    
    public void LoseItem(GameItemData data)
    {
    }

    void Update()
    {
        if (player.CurState == PlayerState.Soul || gameItemDatas.Count == 0) 
        {
            inputHandler.itemFlag = false;
            return;
        }
        // if (inputHandler.scroll != 0)
        // {
        //     RollSelect(inputHandler.scroll > 0 ? -1 : 1);
        // }

        SelectedItem?.OnHold();
        if (inputHandler.itemFlag)
        {
            inputHandler.itemFlag = false;
            if (!MyEventSystem.IsPointerOverUI())
                SelectedItem?.OnActivate();
        }
    }

    public void OnChangeState(StateChangeEvent @event)
    {
        if (@event.newState == PlayerState.Soul)
        {
            //取消选择
            Select(0);
        }
    }

    public GameItemData GetDataByName(string name)
    {
        return nameDataDict[name];
    }

    public void RollSelect(int offset)
    {
        int newSelection = curSelection + offset;
        if (newSelection == -1) newSelection = gameItems.Count - 1;
        if (newSelection == gameItems.Count) newSelection = 0;

        Select(newSelection);
    }

    public void Select(int idx)
    {
        if (idx == curSelection) return;
        SelectedItem?.OnUnselect();

        curSelection = idx;
        SelectedItem?.OnSelect();

        // MyEventSystem.Trigger(new SelectItemEvent()
        // {
        //     itemData = gameItemDatas[idx]
        // });
    }
}
