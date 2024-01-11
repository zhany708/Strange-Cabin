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
        
        //需要实现：当敌人死亡后再触发开门动画，不然敌人死亡动画还没结束门就开了
        m_doorController.OpenDoors();       //敌人死亡后开门
    }
}
