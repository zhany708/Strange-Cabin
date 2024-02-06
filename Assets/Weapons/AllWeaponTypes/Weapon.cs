using System;
using Unity.Burst.CompilerServices;
using UnityEngine;
using ZhangYu.Utilities;




public class Weapon : MonoBehaviour
{
    public event Action OnExit;      //�����¼���ΪPlayerAttackState�ű�

    #region Components
    //public AudioClip WeaponAudio;     //���������ж��ֹ�����Ч�����������ڽű�������������ֱ�ӷŽ�AudioSource

    public SO_WeaponData WeaponData;


    protected Animator animator;
    protected AudioSource audioSource;

    protected Core core;
    protected Player player;
    protected Flip weaponInventoryFlip;

    protected Movement Movement => m_Movement ? m_Movement : core.GetCoreComponent(ref m_Movement);   //���m_Movement�Ƿ�Ϊ�գ����ǵĻ��򷵻������ǵĻ������GetCoreComponent�����Ի�ȡ���
    private Movement m_Movement;
    #endregion

    #region Variables
    protected Vector2 mousePosition;     //���ķ���
    #endregion

    #region Unity Callback Functions
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        player = GetComponentInParent<Player>();
        core = player.GetComponentInChildren<Core>();   //�ȵ���Player�����壬Ȼ���ٴӸ�������Ѱ��Core������
    }

    protected virtual void Start()
    {
        weaponInventoryFlip = new Flip(transform.parent.transform);     //������������깹��Flip�ű�
    }

    protected virtual void Update()
    {
        PointToMouse();
    }
    #endregion

    #region Other Functions
    public virtual void EnterWeapon()
    {
        animator.SetBool("Attack", true);
    }

    public virtual void ExitWeapon()
    {
        animator.SetBool("Attack", false);

        OnExit?.Invoke();
    }




    protected virtual void PointToMouse()
    {
        mousePosition = (player.InputHandler.ProjectedMousePos - new Vector2(transform.parent.position.x, transform.parent.position.y));    //������Ҫ�������ķ���

        transform.parent.right = mousePosition.normalized;   //��һ���󣬸���������ĳ��򣬶�����������
        weaponInventoryFlip.FlipX(player.FacingNum);       //ʵʱ��ת��������ֹ��ҷ�תʱ����Ҳ����ת
    }

    protected virtual void PlayAudioSound()     //��������������Ч
    {
        if (audioSource != null && audioSource.clip != null)
        {
            //audioSource.clip = WeaponAudio;

            //audioSource.volume = 1f;  //��������
            audioSource.Play();
        }
    }
    #endregion

    #region Animation Events
    protected virtual void AnimationActionTrigger() { }
    private void AnimationFinishTrigger()
    {
        ExitWeapon();
    }
    #endregion
}