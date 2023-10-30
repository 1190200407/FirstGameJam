using UnityEngine;

public class Cat : Animal
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
        bool pass;
        //����ſ��վ�������ߡ����ܡ����������ѡ��һ���͵�ǰ�ж���һ�����ж�
        do
        {
            int choose = Random.Range(0, 5);
            switch (choose)
            {
                case 0: chosenAction = new LieAction();
                    break;
                case 1: chosenAction = new WalkAction();
                    break;
                case 2: chosenAction = new StandAction();
                    break;
                case 3: chosenAction = new RunAction();
                    break;
                case 4: chosenAction = new JumpAction();
                    break;
            }

            if (action != null)
            {
                pass = action.GetType().Name != chosenAction.GetType().Name;
                //ǰһ���������ɵ�ʱ����һ��������������վ��
                pass = pass && !(action.GetType().Name == "LieAction" && chosenAction.GetType().Name == "StandAction");
            }
            else
                pass = true;
        } while (!pass);

        return chosenAction;
    }

    public override void OnAngryEnd()
    {
        base.OnAngryEnd();

        if (!(action is JumpAction))
        {
            ChangeNowAction(GetAnAction());
        }
    }

    public override void OnWaterEnter()
    {
        base.OnWaterEnter();
        if (isLeaving)
            nextAction = new FleaAction();
        else
            nextAction = new RunAction();
    }

    public override void OnWaterFull()
    {
        base.OnWaterFull();
    }
}
