using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Animal
{
    public override AnimalAction GetAnAction()
    {
        AnimalAction chosenAction = null;
        //����ſ��վ�������ߡ����������ѡ��һ���͵�ǰ�ж���һ�����ж�
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
