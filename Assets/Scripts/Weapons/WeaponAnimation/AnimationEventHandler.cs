using System;
using UnityEngine;


public class AnimationEventHandler : MonoBehaviour      //用于动画帧事件
{
    public event Action OnStart;          //接收方为MeleeWeapon脚本
    public event Action OnFinish;         //接受事件方为Weapon脚本



    private void AnimationActionTrigger() => OnStart?.Invoke();
    private void AnimationFinishTrigger() => OnFinish?.Invoke();
}

