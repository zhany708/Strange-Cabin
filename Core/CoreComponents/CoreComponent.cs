using System.Collections;
using System.Collections.Generic;
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




    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Core>();       //�Ӹ������������Core���

        if (!core)
        {
            Debug.LogError("There is no Core on the parent");
        }

        //core.Addcomponent(this);    //��������Ҫ����LogicUpdate����������ӽ�List
    }
}
