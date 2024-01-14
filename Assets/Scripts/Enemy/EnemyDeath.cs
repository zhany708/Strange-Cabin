public class EnemyDeath : Death
{
    DoorController m_DoorController;

    


    public override void LogicUpdate()
    {
        if (core.AnimatorInfo.IsName("Death") && core.AnimatorInfo.normalizedTime >= 0.85f)
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
