using UnityEngine;

public class Movement : CoreComponent   //用于管理移动
{
    public Rigidbody2D Rigidbody2d {  get; private set; }

    public Vector2 FacingDirection { get; private set; }


    Vector2 m_WorkSpace;

    protected override void Awake()
    {
        base.Awake();

        Rigidbody2d = GetComponentInParent<Rigidbody2D>();
    }

    
    //public override void LogicUpdate() { }

    #region Setters
    public void SetVelocityZero()
    {
        m_WorkSpace = Vector2.zero;

        SetFinalVelocity();
    }

    public void ReduceVelocity(float reduceAmount)
    {
        m_WorkSpace *= (1 - reduceAmount);
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        m_WorkSpace = velocity * direction;
        
        SetFinalVelocity();
    }

    /*
    public void SetVelocity(float velocity)
    {
        m_WorkSpace *= velocity;

        SetFinalVelocity();
    }
    */

    public void SetFinalVelocity()
    {
        Rigidbody2d.velocity = m_WorkSpace;

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
