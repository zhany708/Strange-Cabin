using UnityEngine;

public class E_EvilTelephone : Event
{

    Animator m_Animator;
    Collider2D m_Collider;




    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_Animator.SetBool("Ringing", false);       //角色触碰电话后取消震动

            PlayerStats player = other.GetComponentInParent<Player>().GetComponentInChildren<PlayerStats>();    //因为进入触发器的是Core里的Combat，所以先获取父物体，再获取子物体

            if (player != null)
            {
                player.DecreaseHealth(20);
                m_Collider.enabled = false;     //玩家受伤后，取消激活碰撞框
            }
        }

        FinishEvent();      //结束事件
    }






    public override void StartEvent()
    {
        m_Animator.SetBool("Ringing", true);
    }
}
