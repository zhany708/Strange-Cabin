using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Animator Animator { get; private set; }

    public Movement Movement
    {
        get => GenericNotImplementedError<Movement>.TryGet(m_Movement, transform.parent.name);  //检测是否调用到了特定组件

        private set => m_Movement = value; 
    }

    public Combat Combat
    {
        get => GenericNotImplementedError<Combat>.TryGet(m_Combat, transform.parent.name);

        private set => m_Combat = value;
    }

    Movement m_Movement;
    Combat m_Combat;

    private void Awake()
    {
        Animator = GetComponentInParent<Animator>();        //调用父物体的动画控制器组件

        Movement = GetComponentInChildren<Movement>();      //调用子物体的Movement脚本
        Combat = GetComponentInChildren<Combat>();

        if (!Movement)
        {
            Debug.LogError("Missing Core Compoent");
        }
    }


    public void LogicUpdate()
    {
        //Movement.LogicUpdate();
    }
}
