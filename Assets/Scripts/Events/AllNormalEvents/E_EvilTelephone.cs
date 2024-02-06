using UnityEngine;

public class E_EvilTelephone : Event
{
    public AudioClip AnswerClip;


    Animator m_Animator;
    AudioSource m_AudioSource;
    Collider2D m_Collider;




    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        m_Collider = GetComponent<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            m_Animator.SetBool("Ringing", false);       //��ɫ�����绰��ȡ����

            PlayAnswerPhoneSound();     //���Žӵ绰����Ч

            PlayerStats playerHealth = other.GetComponentInParent<Player>().GetComponentInChildren<PlayerStats>();    //��Ϊ���봥��������Core���Combat�������Ȼ�ȡ�����壬�ٻ�ȡ������

            if (playerHealth != null)
            {
                playerHealth.DecreaseHealth(20);
                m_Collider.enabled = false;     //������˺�ȡ��������ײ��
            }
        }

        FinishEvent();      //�����¼�
    }






    public override void StartEvent()
    {
        m_Animator.SetBool("Ringing", true);
    }




    private void PlayRingSound()        //���ڶ���֡�¼�������������
    {
        if (m_AudioSource != null && m_AudioSource.clip != null && !m_AudioSource.isPlaying)    //��ֹ�ظ�������Ч
        {
            m_AudioSource.volume = 0.6f;  //��������
            m_AudioSource.Play();   //������ʼʱ����������
        }
    }

    private void PlayAnswerPhoneSound()     //���Žӵ绰����Ч
    {
        if (m_AudioSource != null)   
        {
            m_AudioSource.clip = AnswerClip;    //����Ϊ�ӵ绰����Ч

            m_AudioSource.volume = 1f;  //���õ��������
            m_AudioSource.Play();   //������ʼʱ����������
        }
    }
}
