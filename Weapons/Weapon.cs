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
        [SerializeField] float m_AttackCounterResetCooldown;    //�ָ������ļ��

        public int CurrentAttackCounter
        {
            get => m_CurrentAttackCounter;
            private set => m_CurrentAttackCounter = value >= m_NumberOfAttack ? 0 : value;      //ʹ������¼��Զ����ﵽ���������

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
        Timer m_AttackCounterResetTimer;        //��ʱ���ű������ڼ�ʱ��ʱ�䲻������ָ�������

        int m_CurrentAttackCounter;




        private void Awake()
        {
            core = GetComponentInParent<Player>().GetComponentInChildren<Core>();   //�ȵ���Player�����壬Ȼ���ٴӸ�������Ѱ��Core������

            BaseGameObject = transform.Find("Base").gameObject;
            WeaponSpriteGameObject = transform.Find("WeaponSprite").gameObject;

            m_Animator = BaseGameObject.GetComponent<Animator>();   //����Base�����ϵĶ�����

            m_AnimationEventHandler = BaseGameObject.GetComponent<AnimationEventHandler>();

            m_AttackCounterResetTimer = new Timer(m_AttackCounterResetCooldown);    //��ʼ����ʱ���ű�
        }

        private void Update()
        {
            m_AttackCounterResetTimer.Tick();   //����ʱ����ʼʱ���������м�ʱ
        }

        private void OnEnable()
        {
            m_AnimationEventHandler.OnFinish += Exit;   //���˽ű���Exit�����ӽ��¼�
            m_AttackCounterResetTimer.OnTimerDone += ResetAttackCounter;
        }

        private void OnDisable()
        {
            m_AnimationEventHandler.OnFinish -= Exit;   //������ú���¼����Ƴ���������ֹ��Ϊ�Ҳ����������ڵĽű�������
            m_AttackCounterResetTimer.OnTimerDone -= ResetAttackCounter;
        }


        public void Enter()
        {
            print($"{transform.name} enter");

            m_AttackCounterResetTimer.StopTimer();      //��������ʱֹͣ��ʱ������ֹ�кܳ�������������û������ͻָ�

            //ͨ��Core�е�Facing Direction����ȷ����������,Ȼ�����ù���ΪTrue
            m_Animator.SetFloat("MoveX", Movement.FacingDirection.x);
            m_Animator.SetFloat("MoveY", Movement.FacingDirection.y);

            m_Animator.SetInteger("AttackCounter", m_CurrentAttackCounter);     //Ϊ���������õ�ǰ��������
            m_Animator.SetBool("Attack", true);

            OnEnter?.Invoke();      //ÿ����ʼ����ʱ���ø����
        }

        private void Exit()
        {
            m_Animator.SetBool("Attack", false);
            CurrentAttackCounter++;   //������������������������¼
            m_AttackCounterResetTimer.StartTimer();     //����������ʼ��ʱ

            OnExit?.Invoke();   //�ʺű�ʾ�ȼ���Ƿ�Ϊ��
        }     
        


        private void ResetAttackCounter() => CurrentAttackCounter = 0;
    }
}
