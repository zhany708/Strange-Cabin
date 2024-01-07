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
        if (transform.position.magnitude >= 30f)        //���򾭹�һ�ξ������������
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
        Destroy(gameObject);      //ʹ��������������ײ�壨ǽ�ڣ��Ҿߵȣ�ʱ�Ի�
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

        Destroy(gameObject);
    }
}
