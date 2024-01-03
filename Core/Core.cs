using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Animator Animator { get; private set; }

    public Movement Movement {  get; private set; }

    private void Awake()
    {
        Animator = GetComponentInParent<Animator>();        //���ø�����Ķ������������

        Movement = GetComponentInChildren<Movement>();      //�����������Movement�ű�

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
