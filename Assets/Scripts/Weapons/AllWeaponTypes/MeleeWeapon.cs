using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MeleeWeapon : Weapon
{
    public int CurrentAttackCounter
    {
        get => m_CurrentAttackCounter;
        protected set => m_CurrentAttackCounter = value >= WeaponData.AmountOfAttack ? 0 : value;
    }
    private int m_CurrentAttackCounter;        //��ʾ��������������



    protected SO_MeleeWeaponData aggressiveWeaponData;
    protected CameraShake cameraShake;

    protected List<Idamageable> detectedDamageables = new List<Idamageable>();     //���ڴ��������ڹ�����Χ����ײ��
    protected List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();  //���ڴ������й�����Χ�ڿɻ��˵���ײ��







    protected override void Awake()
    {
        base.Awake();

        if (WeaponData.GetType() == typeof(SO_MeleeWeaponData))
        {
            aggressiveWeaponData = (SO_MeleeWeaponData)WeaponData;     //���WeaponData�뵱ǰAggressiveWeaponData��ͬ���򽫵�ǰ�������������ݵ�Reference����Weapon�ű�
        }
        else
        {
            Debug.LogError("Wrong data for the weapon");
        }

        cameraShake = FindObjectOfType<CameraShake>();    //��ӵ��CameraShake�ű������
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        AddToDetected(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RemoveFromDetected(collision);
    }







    public void CheckMeleeAttack()     //����������ʱ���ô˺���
    {
        //Debug.Log("Checking!");

        MeleeWeaponAttackDetails details = aggressiveWeaponData.AttackDetails[CurrentAttackCounter];        //���ù����������в�ͬ������������Ϣ

        foreach (Idamageable item in detectedDamageables.ToList())      //��ÿһ���п�����˺��ӿڵ���ײ����Ч����ToList��ֹ�������������Bug��ToList���Ը���ԭʼList��
        {
            if (cameraShake != null)
            {
                cameraShake.ShakeCamera(details.CameraShakeIntensity, details.CameraShakeDuration);     //��������𶯽ű�
            }

            item.Damage(details.DamageAmount);      //�Ա���⵽��ײ������˺�
            //item.GetHit(Movement.FacingDirection);      //ʹ�������ײ���������
        }

        foreach (IKnockbackable item in detectedKnockbackables.ToList())
        {
            item.KnockBack(details.KnockbackStrength, mousePosition.normalized);       //����Ŀ��,���˷���Ϊ��귽������ָ��ķ���
            
        }
    }


    public void AddToDetected(Collider2D collision)
    {
        //Debug.Log("Added!");
        if (collision.TryGetComponent<Idamageable>(out var damageable))
        {
            detectedDamageables.Add(damageable);     //�����⵽������˺�����ײ�壬��ӽ�List
        }

        
        if (collision.TryGetComponent<IKnockbackable>(out var knockbackable))
        {
            detectedKnockbackables.Add(knockbackable);    //�����⵽�ɱ����˵���ײ�壬��ӽ�List
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        //Debug.Log("Removed!");
        if (collision.TryGetComponent<Idamageable>(out var damageable))
        {
            detectedDamageables.Remove(damageable);
        }


        if (collision.TryGetComponent<IKnockbackable>(out var knockbackable))
        {
            detectedKnockbackables.Remove(knockbackable);
        }
    }



    #region Animation Events
    protected override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
    }
    #endregion
}
