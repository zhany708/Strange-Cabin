using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Serialization;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D Rigidbody2d {  get; private set; }

    public Vector2 FacingDirection { get; private set; }

    Vector2 m_WorkSpace;

    protected override void Awake()
    {
        base.Awake();

        Rigidbody2d = GetComponentInParent<Rigidbody2D>();
    }

    /*
    public void LogicUpdate()
    {
        CurrentVelocity = Rigidbody2d.velocity;     //ͨ������ʵʱ��ȡ��ǰ�ٶ�
    }
    */


    #region Setters
    public void SetVelocityZero()
    {
        Rigidbody2d.velocity = Vector2.zero;
    }

    public void SetVelocity(Vector2 velocity)
    {
        m_WorkSpace = velocity;
        Rigidbody2d.velocity = m_WorkSpace;     //�ƶ��Ĺؼ�

        if (m_WorkSpace != Vector2.zero)        //��ֹ���ֹͣ�ƶ����ɫ�̶�������
        {
            FacingDirection = m_WorkSpace.normalized;
        }
    }

    public void SetAnimationDirection(Vector2 faceDirection, Vector2 currentDirection)      //�������Ҫ��ȥ��ǰ���꣬��ڶ���������Vector2.Zero
    {
        if (faceDirection != null)
        {
            Vector2 direction = (faceDirection - currentDirection).normalized;      //ֻ��Ҫ����

            //���ų�������Ķ���
            core.Animator.SetFloat("MoveX", direction.x);
            core.Animator.SetFloat("MoveY", direction.y);
        }
    }
    #endregion
}
