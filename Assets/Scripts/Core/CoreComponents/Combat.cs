using UnityEngine;


public class Combat : CoreComponent, Idamageable, IKnockbackable    //用于管理受击
{
    [SerializeField] private GameObject m_DamageParticles;

    public bool IsHit {  get; private set; }
    public float HitResistance;     //击退抗性




    //public override void LogicUpdate() { }

    public void Damage(float amount)
    {
        IsHit = true;

        //Debug.Log(core.transform.parent.name + " Damaged!");
        Stats.DecreaseHealth(amount);

        particleManager.StartParticleWithRandomRotation(m_DamageParticles);   //造成伤害时生成特效
    }

    /*
    public int GetHit(Vector2 direction)
    {
        return Movement.GetFlipNum(direction, Vector2.zero);

        //Debug.Log(core.transform.parent.name + " Faced you!");
    }
    */


    public void KnockBack(float strength, Vector2 direction)
    {
        if (strength > HitResistance)
        {
            //Debug.Log("You got knocked!");
            Movement.SetVelocity(strength - HitResistance, direction);      //只有当击退力度大于击退抗性时才会被击退
        }
    }

    #region Setters
    public void SetIsHitFalse()
    {
        IsHit = false;
    }
    #endregion
}
