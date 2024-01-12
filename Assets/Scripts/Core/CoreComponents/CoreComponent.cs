using UnityEngine;


public class CoreComponent : MonoBehaviour
{
    protected Core core;

    protected Movement Movement
    {
        get
        {
            if (m_Movement) { return m_Movement; }      //�������Ƿ�Ϊ��
            m_Movement = core.GetCoreComponent<Movement>();
            return m_Movement;
        }
    }
    private Movement m_Movement;


    protected Stats Stats
    {
        get
        {
            if (m_Stats) { return m_Stats; }      //�������Ƿ�Ϊ��
            m_Stats = core.GetCoreComponent<Stats>();
            return m_Stats;
        }
    }
    private Stats m_Stats;


    protected ParticleManager particleManager => m_ParticleManager ? m_ParticleManager : core.GetCoreComponent(ref m_ParticleManager);      //�ʺű�ʾ����ʺ���߱���Ϊ�գ��򷵻�ð���ұߵĺ��������򷵻�ð����ߵı���

    private ParticleManager m_ParticleManager;



    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Core>();       //�Ӹ������������Core���

        if (!core)
        {
            Debug.LogError("There is no Core on the parent");
        }

        core.Addcomponent(this);    //��������Ҫ����LogicUpdate����������ӽ�List
    }

    
    protected virtual void Update()
    {
        core.LogicUpdate();
    }
    




    public virtual void LogicUpdate() {  }  
}
