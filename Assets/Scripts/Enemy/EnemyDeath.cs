using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : Death
{
    DoorController m_doorController;

    protected override void Awake()
    {
        base.Awake();

        m_doorController = GetComponentInParent<RoomController>().GetComponentInChildren<DoorController>();
    }


    public override void Die()
    {
        base.Die();
        
        //��Ҫʵ�֣��������������ٴ������Ŷ�������Ȼ��������������û�����žͿ���
        m_doorController.OpenDoors();       //������������
    }
}
