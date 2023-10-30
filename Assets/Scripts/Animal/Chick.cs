using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : Animal
{
    public override AnimalAction GetAnAction()
    {
        if (nextAction != null)
        {
            AnimalAction temp = nextAction;
            nextAction = null;
            return temp;
        }
        AnimalAction chosenAction = null;
        //从窝趴、站立、慢走、快跑、飞跳中随机选择一个和当前行动不一样的行动
        bool pass;
        do
        {
            int choose = Random.Range(0, 5);
            switch (choose)
            {
                case 0:
                    chosenAction = new LieAction();
                    break;
                case 1:
                    chosenAction = new WalkAction();
                    break;
                case 2:
                    chosenAction = new StandAction();
                    break;
                case 3:
                    chosenAction = new RunAction();
                    break;
                case 4:
                    chosenAction = new FlyAction();
                    break;
            }

            if (action != null)
            {
                pass = action.GetType().Name != chosenAction.GetType().Name;
                //前一个动作是躺的时候，下一个动作不可以是站立
                pass = pass && !(action.GetType().Name == "LieAction" && chosenAction.GetType().Name == "StandAction");
            }
            else
                pass = true;
        } while (!pass);

        return chosenAction;
    }

    public override void OnWaterFull()
    {
        base.OnWaterFull();
        ChangeNowAction(new FleaAction());
        StartCoroutine(GameCtr.instance.OnLevelFail());
    }
}
