using System;
using UnityEngine;


public class AnimationEventHandler : MonoBehaviour      //���ڶ���֡�¼�
{
    public event Action OnStart;          //���շ�ΪMeleeWeapon�ű�
    public event Action OnFinish;         //�����¼���ΪWeapon�ű�



    private void AnimationActionTrigger() => OnStart?.Invoke();
    private void AnimationFinishTrigger() => OnFinish?.Invoke();
}

