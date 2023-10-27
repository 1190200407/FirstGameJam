using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FollowingMouse : MonoBehaviour
{
    [Header("���ٸ���ʱ����ת�ٶ�")]
    public float fastFollowSpeed = 100f;
    [Header("���ٸ���ʱ����ת�ٶ�")]
    public float slowFollowSpeed = 10f;

    public bool IsFollowing = true;

    private SpriteRenderer _renderer;
    private bool _isFlip;
    public bool IsFlip
    {
        get { return _isFlip; }
        set 
        { 
            _isFlip = value;

            if (_isFlip && transform.localScale.y > 0)
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            if (!_isFlip && transform.localScale.y < 0)
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
    }

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
   
    private void Update()
    {
        if (IsFollowing)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            FollowPosition(mousePosition, GameCtr.instance.drawAimLine.IsAiming);

            IsFlip = transform.right.x < 0;
        }
    }

    /// <summary>
    /// ��ת�����λ��
    /// </summary>
    /// <param name="mousePosition">���λ��</param>
    private void FollowPosition(Vector3 mousePosition, bool slow = false)
    {
        Vector3 dir = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle), (slow ? slowFollowSpeed : fastFollowSpeed) * Time.deltaTime);
    }
}
