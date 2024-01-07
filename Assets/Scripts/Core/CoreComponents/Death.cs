using UnityEngine;

public class Death : CoreComponent      //�����Ҫ��ͬ������Ч�������½�һ���ű���Ȼ��̳д˽ű�
{
    //[SerializeField] private GameObject[] m_DeathParticles;

    /*
    ParticleManager m_Manager => m_manager ? m_manager : core.GetCoreComponent(ref m_manager);      //���ڵ���������ը����Ч
    ParticleManager m_manager;
    */

    Stats m_Stats => m_stats ? m_stats : core.GetCoreComponent(ref m_stats);
    Stats m_stats;


    private void OnEnable()
    {
        m_Stats.OnHealthZero += Die;    //�������ӽ��¼�
    }

    private void OnDisable()
    {
        m_Stats.OnHealthZero -= Die;    //������ú���¼����Ƴ���������ֹ��Ϊ�Ҳ����������ڵĽű�������
    }




    public void Die()
    {
        /*
        foreach (var particle in m_DeathParticles)
        {
            m_Manager.StartParticles(particle);
        }
        */

        //core.transform.parent.gameObject.SetActive(false);  //������Ϸ����

        core.Animator.SetBool("Death", true);
    }
}
