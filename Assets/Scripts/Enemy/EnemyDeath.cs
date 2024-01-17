public class EnemyDeath : Death
{
    DoorController m_DoorController;

    


    public override void LogicUpdate()
    {
        if (core.AnimatorInfo.IsName("Death") && core.AnimatorInfo.normalizedTime >= 0.8f)  //��߲��ܳ���0.8.�������������ȡ������ǰ����������ִ�д˺���
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
