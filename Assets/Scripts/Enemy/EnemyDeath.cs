public class EnemyDeath : Death
{
    DoorController m_doorController;

    protected override void Awake()
    {
        base.Awake();

        m_doorController = GetComponentInParent<RoomController>().GetComponentInChildren<DoorController>();
    }

    public override void LogicUpdate()
    {
        if (core.AnimatorInfo.IsName("Death") && core.AnimatorInfo.normalizedTime >= 0.85f)
        {
            m_doorController.OpenDoors();       //敌人死亡后开门      
        }
    }


    public override void Die()
    {
        base.Die();

        m_doorController.IncrementEnemyCount();     //增加敌人计数器的计数
    }
}
