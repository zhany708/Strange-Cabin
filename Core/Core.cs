using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Animator Animator { get; private set; }

    public Movement Movement
    {
        get => GenericNotImplementedError<Movement>.TryGet(m_Movement, transform.parent.name);  //����Ƿ���õ����ض����

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
        Animator = GetComponentInParent<Animator>();        //���ø�����Ķ������������

        Movement = GetComponentInChildren<Movement>();      //�����������Movement�ű�
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
