using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Animal
{
    public override AnimalAction GetAnAction()
    {
        AnimalAction chosenAction = null;
        //����ſ��վ�������ߡ����ܡ����������ѡ��һ���͵�ǰ�ж���һ�����ж�
        do
        {
            int choose = UnityEngine.Random.Range(0, 2);
            switch (choose)
            {
                case 0:
                    chosenAction = new LieAction();
                    break;
                case 1: chosenAction = new WalkAction();
                    break;
            }
        }while (action != null && action.GetType().Name == chosenAction.GetType().Name);

        return chosenAction;
    }

    public override void OnWaterEnter()
    {
        base.OnWaterEnter();
    }
}
