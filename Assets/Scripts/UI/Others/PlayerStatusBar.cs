using UnityEngine;
using UnityEngine.UI;



public class PlayerStatusBar : BasePanel
{
    public Image HpImage;
    public Image HpEffectImage;     //Ѫ������ͼƬ



    HealthBar m_PlayerHealthBar;





    protected override void Awake()
    {
        GameObject playerHealthBar = GameObject.Find("PlayerHealthBar");    //��ȡPlayerHealthBar��Ϸ����

        if (playerHealthBar != null )
        {
            m_PlayerHealthBar = playerHealthBar.GetComponent<HealthBar>();       //��ȡ�ű����
        }


        if (m_PlayerHealthBar != null )
        {
            m_PlayerHealthBar.SetHpImage(HpImage);
            m_PlayerHealthBar.SetHpEffectImage(HpEffectImage);
        }        
    }
}
