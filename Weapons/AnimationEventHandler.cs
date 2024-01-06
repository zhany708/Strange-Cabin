using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZhangYu.Weapons
{
    public class AnimationEventHandler : MonoBehaviour
    {
        public event Action OnFinish;

        private void AnimationFinishTrigger() => OnFinish?.Invoke();     //用于动画帧事件
    }
}
