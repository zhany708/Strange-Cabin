using UnityEngine;


public class Combat : CoreComponent, Idamageable, IKnockbackable    //���ڹ����ܻ�
{
    [SerializeField] private GameObject m_DamageParticles;

    public bool IsHit {  get; private set; }



    float m_HitResistance;     //���˿���





    private void Start()
    {
        m_HitResistance = core.HitResistance;   //��Core�����ò���
    }

    public void Damage(float amount)
    {
        IsHit = true;

        //Debug.Log(core.transform.parent.name + " Damaged!");
        Stats.DecreaseHealth(amount);

        particleManager.StartParticleWithRandomRotation(m_DamageParticles);   //����˺�ʱ���ܻ�������Χ������Ч
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
        if (strength > m_HitResistance)
        {
            //Debug.Log("You got knocked!");
            Movement.SetVelocity(strength - m_HitResistance, direction);      //ֻ�е��������ȴ��ڻ��˿���ʱ�Żᱻ����
        }
    }

    #region Setters
    public void SetIsHit(bool isTrue)
    {
        IsHit = isTrue;
    }
    #endregion
}
