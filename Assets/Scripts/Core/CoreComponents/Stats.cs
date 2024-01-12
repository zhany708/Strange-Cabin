using System;
using UnityEngine;

public class Stats : CoreComponent      //���ڹ���������ħ����״̬��Ϣ
{
    public event Action OnHealthZero;       //���շ�ΪDeath�ű�

    public float MaxHealth;
    public float Defense;

    protected float currentHealth;

    protected override void Awake()
    {
        base.Awake();

        currentHealth = MaxHealth;      //��Ϸ��ʼʱ���õ�ǰ����ֵ
    }

    public virtual void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, MaxHealth);    //ȷ������ֵ���ᳬ���������
    }

    public virtual void DecreaseHealth(float amount)
    {
        if (amount > Defense)
        {
            currentHealth -= (amount - Defense);      //�ܵ����˺�ֵ���ڷ�����ʱ�Ż��Ѫ
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            OnHealthZero?.Invoke();     //�ȼ���Ƿ�Ϊ�գ��ٵ�����ʱ����

            //Debug.Log("Health is zero!!");
        }
    }

    #region Setters
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    #endregion
}
