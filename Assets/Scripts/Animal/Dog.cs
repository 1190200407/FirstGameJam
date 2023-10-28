using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Animal
{
    public override AnimalAction GetAnAction()
    {
        AnimalAction chosenAction = null;
        //从窝趴、站立、慢走、快跑中随机选择一个和当前行动不一样的行动
        do
        {
            int choose = UnityEngine.Random.Range(0, 4);
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
            }
        } while (action != null && action.GetType().Name == chosenAction.GetType().Name);

        return chosenAction;
    }
}
