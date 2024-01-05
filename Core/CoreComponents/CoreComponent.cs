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
            if (m_Movement) { return m_Movement; }      //检查组件是否为空
            m_Movement = core.GetCoreComponent<Movement>();
            return m_Movement;
        }
    }
    private Movement m_Movement;

    protected Stats Stats
    {
        get
        {
            if (m_Stats) { return m_Stats; }      //检查组件是否为空
            m_Stats = core.GetCoreComponent<Stats>();
            return m_Stats;
        }
    }
    private Stats m_Stats;




    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Core>();       //从父物体那里调用Core组件

        if (!core)
        {
            Debug.LogError("There is no Core on the parent");
        }

        //core.Addcomponent(this);    //将所有需要运用LogicUpdate函数的组件加进List
    }
}
