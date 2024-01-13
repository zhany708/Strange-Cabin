using UnityEngine;


public class FireBall : MonoBehaviour
{
    public int DamageAmount = 2;
    public float DamageKnockbackStrength = 2f;


    Rigidbody2D m_RigidBody2d;
    Vector2 m_AttackDirection;

    void Awake()
    {
        m_RigidBody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.magnitude >= 30f)        //火球经过一段距离后自我销毁
        {
            ParticlePool.Instance.PushObject(gameObject);
        }
    }



    public void Launch(Vector2 direction, float force)
    {
        m_AttackDirection = direction;
        m_RigidBody2d.AddForce(direction * force);
    }



    
    private void OnCollisionEnter2D(Collision2D other)
    {        
        ParticlePool.Instance.PushObject(gameObject);       //使火球碰到其他碰撞体（墙壁，家具等）时自毁
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Idamageable damageable = other.GetComponent<Idamageable>();
        IKnockbackable knockbackable = other.GetComponent<IKnockbackable>();

        if (damageable != null)
        {
            damageable.Damage(DamageAmount);
            damageable.GetHit(m_AttackDirection);
        }

        if (knockbackable != null)
        {
            knockbackable.KnockBack(DamageKnockbackStrength, m_AttackDirection);
        }

        ParticlePool.Instance.PushObject(gameObject);
    }
}
