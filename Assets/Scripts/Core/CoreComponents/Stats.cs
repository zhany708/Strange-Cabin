using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent      //用于管理生命，魔力等状态信息
{
    public event Action OnHealthZero;       //事件类型的变量

    public float MaxHealth;
    public float Defense;

    float m_CurrentHealth;

    protected override void Awake()
    {
        base.Awake();

        m_CurrentHealth = MaxHealth;      //游戏开始时重置当前生命值
    }

    public void IncreaseHealth(float amount)
    {
        m_CurrentHealth = Mathf.Clamp(m_CurrentHealth + amount, 0, MaxHealth);    //确保生命值不会超过最大上限
    }

    public void DecreaseHealth(float amount)
    {
        if (amount > Defense)
        {
            m_CurrentHealth -= (amount - Defense);      //受到的伤害值大于防御力时才会扣血
        }

        if (m_CurrentHealth <= 0)
        {
            m_CurrentHealth = 0;

            OnHealthZero?.Invoke();     //先检查是否为空，再调用延时函数

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
