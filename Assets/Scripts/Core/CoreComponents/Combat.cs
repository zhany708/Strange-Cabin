using UnityEngine;


public class Combat : CoreComponent, Idamageable, IKnockbackable    //���ڹ����ܻ�
{
    [SerializeField] private GameObject m_DamageParticles;

    public bool IsHit {  get; private set; }
    public float HitResistance;     //���˿���




    //public override void LogicUpdate() { }

    public void Damage(float amount)
    {
        IsHit = true;

        //Debug.Log(core.transform.parent.name + " Damaged!");
        Stats.DecreaseHealth(amount);

        particleManager.StartParticleWithRandomRotation(m_DamageParticles);   //����˺�ʱ������Ч
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
            Movement.SetVelocity(strength - HitResistance, direction);      //ֻ�е��������ȴ��ڻ��˿���ʱ�Żᱻ����
        }
    }

    #region Setters
    public void SetIsHitFalse()
    {
        IsHit = false;
    }
    #endregion
}
