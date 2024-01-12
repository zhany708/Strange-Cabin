using System;
using UnityEngine;

public class Stats : CoreComponent      //用于管理生命，魔力等状态信息
{
    public event Action OnHealthZero;       //接收方为Death脚本

    public float MaxHealth;
    public float Defense;

    protected float currentHealth;

    protected override void Awake()
    {
        base.Awake();

        currentHealth = MaxHealth;      //游戏开始时重置当前生命值
    }

    public virtual void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, MaxHealth);    //确保生命值不会超过最大上限
    }

    public virtual void DecreaseHealth(float amount)
    {
        if (amount > Defense)
        {
            currentHealth -= (amount - Defense);      //受到的伤害值大于防御力时才会扣血
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            OnHealthZero?.Invoke();     //先检查是否为空，再调用延时函数

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
