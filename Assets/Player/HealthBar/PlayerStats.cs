public class PlayerStats : Stats
{
    HealthBar m_PlayerHealthBar;

    
    protected override void Awake()
    {
        base.Awake();

        m_PlayerHealthBar = GetComponentInChildren<HealthBar>();      //��ȡѪ�������Ѫ������ű�
    }




    public override void DecreaseHealth(float amount)
    {
        base.DecreaseHealth(amount);

        m_PlayerHealthBar.SetCurrentHealth(currentHealth);      //����Ѫ���ű��еĸ�������ֵ����
    }

    public override void IncreaseHealth(float amount)
    {
        base.IncreaseHealth(amount);

        m_PlayerHealthBar.SetCurrentHealth(currentHealth);
    }

}

