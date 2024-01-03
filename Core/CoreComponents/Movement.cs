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
        CurrentVelocity = Rigidbody2d.velocity;     //通过刚体实时获取当前速度
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
        Rigidbody2d.velocity = m_WorkSpace;     //移动的关键

        if (m_WorkSpace != Vector2.zero)        //防止玩家停止移动后角色固定朝向上
        {
            FacingDirection = m_WorkSpace.normalized;
        }
    }

    public void SetAnimationDirection(Vector2 faceDirection, Vector2 currentDirection)      //如果不需要减去当前坐标，则第二个参数用Vector2.Zero
    {
        if (faceDirection != null)
        {
            Vector2 direction = (faceDirection - currentDirection).normalized;      //只需要方向

            //播放朝向坐标的动画
            core.Animator.SetFloat("MoveX", direction.x);
            core.Animator.SetFloat("MoveY", direction.y);
        }
    }
    #endregion
}
