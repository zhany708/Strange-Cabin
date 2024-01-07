using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image hpImage;
    public Image hpEffectImage;     //血量缓冲图片


    PlayerStats m_PlayerStats;
    float m_MaxHp;
    float m_CurrentHp;
    float m_BuffTime = 0.5f;    //缓冲时间

    Coroutine m_UpdateCoroutine;    //防止上一轮协程还没结束就开始新的协程（扣血）

    private void Start()
    {
        m_PlayerStats = GetComponentInParent<PlayerStats>();

        m_MaxHp = m_PlayerStats.MaxHealth;        //游戏开始时初始化最大生命值
        m_CurrentHp = m_MaxHp;

        UpdateHealthBar();
    }




    public void SetCurrentHealth(float health)
    {
        m_CurrentHp = Mathf.Clamp(health, 0f, m_MaxHp);     //返回的值限制在0和血量上限之间
        UpdateHealthBar();
    }


    private void UpdateHealthBar()
    {
        hpImage.fillAmount = m_CurrentHp / m_MaxHp;

        if (m_UpdateCoroutine != null)      
        {
            StopCoroutine(m_UpdateCoroutine);     //如果协程正在进行，则停止它然后开始新的协程，保证只有一个协程存在
        }

        m_UpdateCoroutine = StartCoroutine(UpdateHpEffect());
    }





    private IEnumerator UpdateHpEffect()
    {
        float effectLength = hpEffectImage.fillAmount - hpImage.fillAmount;     //缓冲的血量
        float elapsedTime = 0f;     //用于确保缓冲时间在0.5秒内

        while (elapsedTime < m_BuffTime && effectLength != 0f)
        {
            elapsedTime += Time.deltaTime;
            hpEffectImage.fillAmount = Mathf.Lerp(hpImage.fillAmount + effectLength, hpImage.fillAmount, elapsedTime/m_BuffTime);   //返回值根据第三个参数决定， 0则返回参数一，1则返回参数二，0.5则返回中点

            yield return null;      //等待一帧的时间
        }

        hpEffectImage.fillAmount = hpImage.fillAmount;      //防止缓冲血条超过红色血条
    }
}
