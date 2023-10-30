using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������ű�
/// �ص��Ǹ�дInit��OnActionEnd��OnWaterEnter��OnWaterFull
/// </summary>
public class Animal : MonoBehaviour
{
    public AnimalAction nextAction;
    public AnimalSetting Setting => GameCtr.instance.anmMgr.settings[tag];
    public AudioSource audioSource;
    public Animator animator;

    public AnimalAction action;
    public bool isActing = false;
    public bool isLeaving = false;
    public string animName;

    public bool sfxCooldown = false;

    private int _direction = 1;
    public int Direction
    {
        get { return _direction; }
        set 
        {
            if (_direction != value)
            {
                _direction = value;
                walkDist = 0f;
            }
        }
    }
    public float walkDist = 0f;

    private int _curFill = 0;
    public int CurFill
    {
        get { return _curFill; }
        set
        {
            _curFill = value;
            if (Capa != 0)
            {
                Color oldColor = GetComponent<SpriteRenderer>().color;
                GetComponent<SpriteRenderer>().color = new Color(oldColor.r, oldColor.g, oldColor.b, 0.3f + ((float)_curFill / Capa) * 0.7f);
            }
        }
    }
    public int Capa { get; set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        if (isActing)
        {
            action.Act();
            if (action.isEnd)
            {
                OnActionEnd();
            }
        }
    }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public virtual void Init()
    {
        Capa = UnityEngine.Random.Range(-Setting.waterCapaBias, Setting.waterCapaBias + 1) + Setting.waterCapa;
        CurFill = 0;

        ChangeNowAction(GetAnAction());
    }

    public void PlayAnim(string animName)
    {
        if (animator != null)
        {
            animator.Play(animName);
            if (animName == "SitDown")
                this.animName = "Sit";
            else
                this.animName = animName;
        }
    }

    public virtual void OnAngryEnd()
    {
        animator.Play(animName);
    }

    public void ChangeNowAction(AnimalAction act)
    {
        action = act;
        if (act == null) return;
        action.host = this;
        action.OnActionStart();
        isActing = true;
    }

    /// <summary>
    /// ����һ�������ж�
    /// </summary>
    /// <returns></returns>
    public virtual AnimalAction GetAnAction()
    {
        return null;
    }

    /// <summary>
    /// �������궯������Ӧ�Ĳ���
    /// </summary>
    public virtual void OnActionEnd()
    {
        action.OnActionEnd();
        ChangeNowAction(GetAnAction());
    }

    /// <summary>
    /// �ж��Ƿ�Ҫת��
    /// </summary>
    public void TurnCheck()
    {
        if (walkDist > Setting.turnCheckDist && UnityEngine.Random.value < Setting.turnCheckProb)
        {
            Direction = -Direction;
        }
    }

    /// <summary>
    /// ˮ�ν���ʱ��Ӧ�Ĳ���
    /// </summary>
    public virtual void OnWaterEnter()
    {
        if (animator != null)
        {
            animator.Play("Angry");
        }

        if (isLeaving) return;
        CurFill++;
        if (CurFill == Capa)
            OnWaterFull();

        if (!sfxCooldown)
        {
            audioSource.Play();
            StartCoroutine(SfxCoolDown(UnityEngine.Random.Range(2f, 4f)));
        }

        GameCtr.instance.NowScore = math.clamp(GameCtr.instance.NowScore + Setting.waterEnterScore, 0, GameCtr.instance.goalScore);
    }

    public IEnumerator SfxCoolDown(float time)
    {
        sfxCooldown = true;
        yield return new WaitForSeconds(time);
        sfxCooldown = false;
    }

    /// <summary>
    /// ˮ����ʱ��Ӧ�Ĳ���
    /// </summary>
    public virtual void OnWaterFull()
    {
        isLeaving = true;
        GetComponent<Collider2D>().enabled = false;
        GameCtr.instance.NowScore = math.clamp(GameCtr.instance.NowScore + Setting.waterFullScore, 0, GameCtr.instance.goalScore);

        audioSource.Play();

        GameCtr.instance.anmMgr.animalCount[name]--;
    }
}
