using System;
using UnityEngine;

public class Stats : CoreComponent      //���ڹ���������ħ����״̬��Ϣ
{
    public event Action OnHealthZero;       //���շ�ΪDeath�ű�

    public float MaxHealth {  get; private set; }



    protected float currentHealth;

    float m_Defense;





    protected override void Awake()
    {
        base.Awake();

        //currentHealth = MaxHealth;      //��Ϸ��ʼʱ���õ�ǰ����ֵ
    }

    private void Start()
    {
        MaxHealth = core.MaxHealth;     //��Core�����ò���
        m_Defense = core.Defense;

        currentHealth = MaxHealth;      //��Ϸ��ʼʱ���õ�ǰ����ֵ
    }




    public virtual void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, MaxHealth);    //ȷ������ֵ���ᳬ���������
    }

    public virtual void DecreaseHealth(float amount)
    {
        if (currentHealth != 0)      //����ֵΪ0ʱ�Ͳ������������
        {
            if (amount > m_Defense)    
            {
                currentHealth -= (amount - m_Defense);      //�ܵ����˺�ֵ���ڷ�����ʱ�Ż��Ѫ
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                OnHealthZero?.Invoke();     //�ȼ���Ƿ�Ϊ�գ��ٵ�����ʱ����

                //Debug.Log("Health is zero!!");
            }
        }
    }

    #region Getters
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    #endregion

    #region Setters
    public void SetCurrentHealth(float health)
    {
        currentHealth = health;
    }
    #endregion
}
