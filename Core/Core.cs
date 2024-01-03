using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Animator Animator { get; private set; }

    public Movement Movement {  get; private set; }

    private void Awake()
    {
        Animator = GetComponentInParent<Animator>();        //调用父物体的动画控制器组件

        Movement = GetComponentInChildren<Movement>();      //调用子物体的Movement脚本

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
