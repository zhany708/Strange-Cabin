public class EnemyDeath : Death
{
    DoorController m_DoorController;

    


    public override void LogicUpdate()
    {
        if (core.AnimatorInfo.IsName("Death") && core.AnimatorInfo.normalizedTime >= 0.85f)
        {
            m_DoorController.OpenDoors();       //������������      
        }
    }


    public override void Die()
    {
        base.Die();

        m_DoorController.IncrementEnemyCount();     //���ӵ��˼������ļ���
    }




    public void SetDoorController(DoorController door)
    {
        m_DoorController = door;
    }
}
