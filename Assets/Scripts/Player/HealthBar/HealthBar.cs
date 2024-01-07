using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image hpImage;
    public Image hpEffectImage;     //Ѫ������ͼƬ


    PlayerStats m_PlayerStats;
    float m_MaxHp;
    float m_CurrentHp;
    float m_BuffTime = 0.5f;    //����ʱ��

    Coroutine m_UpdateCoroutine;    //��ֹ��һ��Э�̻�û�����Ϳ�ʼ�µ�Э�̣���Ѫ��

    private void Start()
    {
        m_PlayerStats = GetComponentInParent<PlayerStats>();

        m_MaxHp = m_PlayerStats.MaxHealth;        //��Ϸ��ʼʱ��ʼ���������ֵ
        m_CurrentHp = m_MaxHp;

        UpdateHealthBar();
    }




    public void SetCurrentHealth(float health)
    {
        m_CurrentHp = Mathf.Clamp(health, 0f, m_MaxHp);     //���ص�ֵ������0��Ѫ������֮��
        UpdateHealthBar();
    }


    private void UpdateHealthBar()
    {
        hpImage.fillAmount = m_CurrentHp / m_MaxHp;

        if (m_UpdateCoroutine != null)      
        {
            StopCoroutine(m_UpdateCoroutine);     //���Э�����ڽ��У���ֹͣ��Ȼ��ʼ�µ�Э�̣���ֻ֤��һ��Э�̴���
        }

        m_UpdateCoroutine = StartCoroutine(UpdateHpEffect());
    }





    private IEnumerator UpdateHpEffect()
    {
        float effectLength = hpEffectImage.fillAmount - hpImage.fillAmount;     //�����Ѫ��
        float elapsedTime = 0f;     //����ȷ������ʱ����0.5����

        while (elapsedTime < m_BuffTime && effectLength != 0f)
        {
            elapsedTime += Time.deltaTime;
            hpEffectImage.fillAmount = Mathf.Lerp(hpImage.fillAmount + effectLength, hpImage.fillAmount, elapsedTime/m_BuffTime);   //����ֵ���ݵ��������������� 0�򷵻ز���һ��1�򷵻ز�������0.5�򷵻��е�

            yield return null;      //�ȴ�һ֡��ʱ��
        }

        hpEffectImage.fillAmount = hpImage.fillAmount;      //��ֹ����Ѫ��������ɫѪ��
    }
}
