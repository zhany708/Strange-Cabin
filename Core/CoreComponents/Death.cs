using UnityEngine;

public class Death : CoreComponent      //如果需要不同的死亡效果，则新建一个脚本，然会继承此脚本
{
    //[SerializeField] private GameObject[] m_DeathParticles;

    /*
    ParticleManager m_Manager => m_manager ? m_manager : core.GetCoreComponent(ref m_manager);      //用于敌人死亡后爆炸的特效
    ParticleManager m_manager;
    */

    Stats m_Stats => m_stats ? m_stats : core.GetCoreComponent(ref m_stats);
    Stats m_stats;


    private void OnEnable()
    {
        m_Stats.OnHealthZero += Die;    //将函数加进事件
    }

    private void OnDisable()
    {
        m_Stats.OnHealthZero -= Die;    //物体禁用后从事件中移除函数，防止因为找不到函数所在的脚本而报错
    }




    public void Die()
    {
        /*
        foreach (var particle in m_DeathParticles)
        {
            m_Manager.StartParticles(particle);
        }
        */

        //core.transform.parent.gameObject.SetActive(false);  //禁用游戏物体

        core.Animator.SetBool("Death", true);
    }
}
