using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent      //���ڹ���������ħ����״̬��Ϣ
{
    public event Action OnHealthZero;       //�¼����͵ı���

    public float MaxHealth;
    public float Defense;

    float m_CurrentHealth;

    protected override void Awake()
    {
        base.Awake();

        m_CurrentHealth = MaxHealth;      //��Ϸ��ʼʱ���õ�ǰ����ֵ
    }

    public void IncreaseHealth(float amount)
    {
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth + amount, 0, MaxHealth);    //ȷ������ֵ���ᳬ���������
    }

    public void DecreaseHealth(float amount)
    {
        if (amount > Defense)
        {
            m_CurrentHealth -= (amount - Defense);      //�ܵ����˺�ֵ���ڷ�����ʱ�Ż��Ѫ
        }

        if (m_CurrentHealth <= 0)
        {
            m_CurrentHealth = 0;

            OnHealthZero?.Invoke();     //�ȼ���Ƿ�Ϊ�գ��ٵ�����ʱ����

            Debug.Log("Health is zero!!");
        }
    }

    #region Setters
    public float GetCurrentHealth()
    {
        return m_CurrentHealth;
    }
    #endregion
}
