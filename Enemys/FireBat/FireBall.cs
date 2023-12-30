using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class FireBall : MonoBehaviour
{
    Rigidbody2D m_RigidBody2d;
    Vector2 m_AttackDirection;
    int m_damage = 2;

    void Awake()
    {
        m_RigidBody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.magnitude >= 30f)        //火球经过一段距离后自我销毁
        {
            Destroy(gameObject);
        }
    }



    public void Launch(Vector2 direction, float force)
    {
        m_AttackDirection = direction;
        m_RigidBody2d.AddForce(direction * force);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.collider.GetComponent<PlayerController>();      //调用玩家函数
        if (player != null)
        {
            player.PlayerGetHit(m_damage, m_AttackDirection);
        }

        Destroy(gameObject);    
    }
}
