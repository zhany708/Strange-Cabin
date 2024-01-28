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
            m_Animator.SetBool("Ringing", false);       //��ɫ�����绰��ȡ����

            PlayerStats player = other.GetComponentInParent<Player>().GetComponentInChildren<PlayerStats>();    //��Ϊ���봥��������Core���Combat�������Ȼ�ȡ�����壬�ٻ�ȡ������

            if (player != null)
            {
                player.DecreaseHealth(20);
                m_Collider.enabled = false;     //������˺�ȡ��������ײ��
            }
        }

        FinishEvent();      //�����¼�
    }






    public override void StartEvent()
    {
        m_Animator.SetBool("Ringing", true);
    }
}
