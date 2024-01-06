using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhangYu.Utilities;

namespace ZhangYu.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] int m_NumberOfAttack;
        [SerializeField] float m_AttackCounterResetCooldown;    //恢复连击的间隔

        public int CurrentAttackCounter
        {
            get => m_CurrentAttackCounter;
            private set => m_CurrentAttackCounter = value >= m_NumberOfAttack ? 0 : value;      //使连击记录永远不会达到最大连击数

        }

        public event Action OnEnter;
        public event Action OnExit;

        public GameObject BaseGameObject {  get; private set; }
        public GameObject WeaponSpriteGameObject { get; private set; }

        protected Core core;

        protected Movement Movement => m_Movement ? m_Movement : core.GetCoreComponent(ref m_Movement);
        private Movement m_Movement;


        Animator m_Animator;

        AnimationEventHandler m_AnimationEventHandler;
        Timer m_AttackCounterResetTimer;        //计时器脚本，用于计时长时间不攻击后恢复连击数

        int m_CurrentAttackCounter;




        private void Awake()
        {
            core = GetComponentInParent<Player>().GetComponentInChildren<Core>();   //先调用Player父物体，然后再从父物体中寻找Core子物体

            BaseGameObject = transform.Find("Base").gameObject;
            WeaponSpriteGameObject = transform.Find("WeaponSprite").gameObject;

            m_Animator = BaseGameObject.GetComponent<Animator>();   //调用Base物体上的动画器

            m_AnimationEventHandler = BaseGameObject.GetComponent<AnimationEventHandler>();

            m_AttackCounterResetTimer = new Timer(m_AttackCounterResetCooldown);    //初始化计时器脚本
        }

        private void Update()
        {
            m_AttackCounterResetTimer.Tick();   //当计时器开始时，持续进行计时
        }

        private void OnEnable()
        {
            m_AnimationEventHandler.OnFinish += Exit;   //将此脚本的Exit函数加进事件
            m_AttackCounterResetTimer.OnTimerDone += ResetAttackCounter;
        }

        private void OnDisable()
        {
            m_AnimationEventHandler.OnFinish -= Exit;   //物体禁用后从事件中移除函数，防止因为找不到函数所在的脚本而报错
            m_AttackCounterResetTimer.OnTimerDone -= ResetAttackCounter;
        }


        public void Enter()
        {
            print($"{transform.name} enter");

            m_AttackCounterResetTimer.StopTimer();      //武器攻击时停止计时器，防止有很长的武器动画还没播放完就恢复

            //通过Core中的Facing Direction向量确定动画方向,然后设置攻击为True
            m_Animator.SetFloat("MoveX", Movement.FacingDirection.x);
            m_Animator.SetFloat("MoveY", Movement.FacingDirection.y);

            m_Animator.SetInteger("AttackCounter", m_CurrentAttackCounter);     //为动画器设置当前连击次数
            m_Animator.SetBool("Attack", true);

            OnEnter?.Invoke();      //每当开始攻击时调用该组件
        }

        private void Exit()
        {
            m_Animator.SetBool("Attack", false);
            CurrentAttackCounter++;   //攻击结束后增加连击次数记录
            m_AttackCounterResetTimer.StartTimer();     //攻击结束后开始计时

            OnExit?.Invoke();   //问号表示先检查是否为空
        }     
        


        private void ResetAttackCounter() => CurrentAttackCounter = 0;
    }
}
