using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, Idamageable, IKnockbackable
{
    public bool IsHit {  get; private set; }
    public float DamageAmount { get; private set; }


    public void Damage(float amount)
    {
        IsHit = true;
        DamageAmount = amount;

        Debug.Log(core.transform.parent.name + " Damaged!");
    }

    public void GetHit(Vector2 direction)
    {
        core.Movement.SetAnimationDirection(Vector2.zero, direction);

        Debug.Log(core.transform.parent.name + " Faced you!");
    }



    public void KnockBack(float strength, Vector2 direction)
    {
        core.Movement.SetVelocity(strength, direction);
    }

    #region Setters
    public void SetIsHiTrue()
    {
        IsHit = true;
    }

    public void SetIsHitFalse()
    {
        IsHit = false;
    }
    #endregion
}
