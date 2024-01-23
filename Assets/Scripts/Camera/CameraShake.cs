using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    CinemachineBasicMultiChannelPerlin m_VirtualCameraNoise;

    float m_Intensity;
    bool m_IsShake =  false;

    void Start()
    {
        if (virtualCamera != null)
        {
            m_VirtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();     //调用震动组件
        }
    }


    public void ShakeCamera(float intensity, float duration)    //震动函数
    {
        if (m_VirtualCameraNoise != null && !m_IsShake)     //只有不在震动时才会开始震动
        {
            m_Intensity = intensity;
            m_IsShake = true;

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
            m_VirtualCameraNoise.m_AmplitudeGain = Mathf.Lerp(m_Intensity, 0f, elapsed / duration);     //使震动强度逐渐降低，而不是突然变成0
            yield return null;
        }

        m_VirtualCameraNoise.m_AmplitudeGain = 0f;      //持续时间结束后取消震动

        m_IsShake = false;
    }
}