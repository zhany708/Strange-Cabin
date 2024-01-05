using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, Idamageable, IKnockbackable    //���ڹ����ܻ�
{
    public bool IsHit {  get; private set; }
    public float HitResistance;     //���˿���

    [SerializeField] GameObject m_DamageParticles;

    ParticleManager m_ParticleManager => m_particleManager ? m_particleManager : core.GetCoreComponent(ref m_particleManager);      //�ʺű�ʾ����ʺ���߱���Ϊ�գ��򷵻�ð���ұߵĺ��������򷵻�ð����ߵı���

    ParticleManager m_particleManager;

    //public override void LogicUpdate() { }

    public void Damage(float amount)
    {
        IsHit = true;

        //Debug.Log(core.transform.parent.name + " Damaged!");
        Stats.DecreaseHealth(amount);

        m_ParticleManager.StartParticleWithRandomRotation(m_DamageParticles);   //����˺�ʱ������Ч
    }

    public void GetHit(Vector2 direction)
    {
        Movement.SetAnimationDirection(Vector2.zero, direction);

        //Debug.Log(core.transform.parent.name + " Faced you!");
    }



    public void KnockBack(float strength, Vector2 direction)
    {
        if (strength > HitResistance)
        {
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
