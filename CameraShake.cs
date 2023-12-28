using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    CinemachineBasicMultiChannelPerlin m_VirtualCameraNoise;

    void Start()
    {
        if (virtualCamera != null)
        {
            m_VirtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();     //���������
        }
    }


    public void ShakeCamera(float intensity, float duration)    //�𶯺���
    {
        if (m_VirtualCameraNoise != null)
        {
            m_VirtualCameraNoise.m_AmplitudeGain = intensity;
            StartCoroutine(StopShake(duration));
        }
    }
    
    IEnumerator StopShake(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_VirtualCameraNoise.m_AmplitudeGain = 0f;      //����ʱ�������ȡ����
    }
}