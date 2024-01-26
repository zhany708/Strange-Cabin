using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float Speed;


    protected Rigidbody2D rigidBody2D;

    GunWeapon m_Gun;



    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.magnitude >= 30f)        //经过一段距离后自我销毁
        {
            ParticlePool.Instance.PushObject(gameObject);
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        ParticlePool.Instance.PushObject(gameObject);       //使子弹碰到其他碰撞体（墙壁，家具等）时自毁
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Idamageable damageable = other.GetComponent<Idamageable>();

        if (damageable != null)
        {
            damageable.Damage(m_Gun.GunData.AttackDetail.DamageAmount);
            //damageable.GetHit(m_AttackDirection);
        }

        ParticlePool.Instance.PushObject(gameObject);
    }




    public virtual void SetSpeed(Vector2 direction)
    {
        rigidBody2D.velocity = direction.normalized * Speed;
    }

    public void SetWeapon(GunWeapon thisWeapon)
    {
        m_Gun = thisWeapon;
    }
}
