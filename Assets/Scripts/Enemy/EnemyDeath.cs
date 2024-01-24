public class EnemyDeath : Death
{
    public DoorController DoorController { get; private set; }

    



    public override void Die()
    {
        base.Die();

        if (DoorController != null)
        {
            DoorController.IncrementEnemyCount();     //���ӵ��˼������ļ���
        }  
    }




    public void SetDoorController(DoorController door)
    {
        DoorController = door;
    }
}
