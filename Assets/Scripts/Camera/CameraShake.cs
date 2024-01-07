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
            m_VirtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();     //调用震动组件
        }
    }


    public void ShakeCamera(float intensity, float duration)    //震动函数
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
        m_VirtualCameraNoise.m_AmplitudeGain = 0f;      //持续时间结束后取消震动
    }
}