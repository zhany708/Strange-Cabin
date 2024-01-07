using System;
using UnityEngine;

namespace ZhangYu.Utilities     //�����ļ��������Ժ�������Ϸ�����ܻ��õ��ĺ��������ʱ����
{
    public class Timer
    {
        public event Action OnTimerDone;

        float m_StartTime;
        float m_Duration;
        float m_TargetTime;

        bool m_IsActive;

        public Timer(float duration)
        {
            this.m_Duration = duration;
        }



        public void StartTimer()
        {
            m_StartTime = Time.time;
            m_TargetTime = m_StartTime + m_Duration;
            m_IsActive = true;
        }

        public void StopTimer()
        {
            m_IsActive = false;
        }


        
        public void Tick()
        {
            if (!m_IsActive) return;


            if (Time.time > m_TargetTime)
            {
                OnTimerDone?.Invoke();
                StopTimer();    //����Ŀ��ʱ���ֹͣ��ʱ
            }
        }
    }
}
