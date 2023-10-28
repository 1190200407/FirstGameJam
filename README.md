# GameJam in master 修改说明
> ljy

1. >[!WARNING]
   >WinScene中的所有Button没有交互反应
2. >[!NOTE]
   >进入WinScene条件判定
3. >[!NOTE]
   > 从WinScene进入下一关的代码实现
  
   统计当前通过关卡的变量应该放在哪个类，用什么方式访问比较合理？

   MainMenu中的`PlayerPrefs`获取的是当前scene层数还是通过的level数？
   ```c#
   public void SetLevelButtons()
    {
        int nowLevel = PlayerPrefs.GetInt("ClearLevels", 0);
        for (int i = 0; i < levelButtons.Count; i++)
            levelButtons[i].interactable = i <= nowLevel;
    }
   ```
