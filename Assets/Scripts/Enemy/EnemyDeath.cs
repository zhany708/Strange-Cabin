public class EnemyDeath : Death
{
    DoorController m_DoorController;

    


    public override void LogicUpdate()
    {
        if (core.AnimatorInfo.IsName("Death") && core.AnimatorInfo.normalizedTime >= 0.8f)  //最高不能超过0.8.否则敌人死亡后取消激活前可能来不及执行此函数
        {
            m_DoorController.OpenDoors();       //敌人死亡后开门      
        }
    }


    public override void Die()
    {
        base.Die();

        m_DoorController.IncrementEnemyCount();     //增加敌人计数器的计数
    }




    public void SetDoorController(DoorController door)
    {
        m_DoorController = door;
    }
}
