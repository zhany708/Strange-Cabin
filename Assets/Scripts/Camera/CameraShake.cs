using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera m_PlayerCamera;
    CinemachineBasicMultiChannelPerlin m_VirtualCameraNoise;

    float m_Intensity;
    bool m_IsShake =  false;

    void Start()
    {
        if (m_PlayerCamera != null)
        {
            m_PlayerCamera = GetComponent<CinemachineVirtualCamera>();
            m_VirtualCameraNoise = m_PlayerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();     //���������
        }
    }


    public void ShakeCamera(float intensity, float duration)    //�𶯺���
    {
        if (m_VirtualCameraNoise != null && !m_IsShake)     //ֻ�в�����ʱ�ŻῪʼ��
        {
            m_Intensity = intensity;
            m_IsShake = true;

            m_VirtualCameraNoise.ReSeed();      //ÿ�ο�ʼ��ǰ�޸�������ӣ�ʹÿ���𶯷��򶼲�һ��
            m_VirtualCameraNoise.m_AmplitudeGain = m_Intensity;
            StartCoroutine(StopShake(duration));
        }
    }
    
    IEnumerator StopShake(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            m_VirtualCameraNoise.m_AmplitudeGain = Mathf.Lerp(m_Intensity, 0f, elapsed / duration);     //ʹ��ǿ���𽥽��ͣ�������ͻȻ���0
            yield return null;
        }

        m_VirtualCameraNoise.m_AmplitudeGain = 0f;      //����ʱ�������ȡ����

        m_IsShake = false;
    }
}