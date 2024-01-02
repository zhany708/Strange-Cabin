using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    
    //攻击相关
    public InputAction heavySwingAction;
    public InputAction quickSwingAction;
    public float quickSwingSpeed;    //补偿速度
    public float heavySwingSpeed;
    public int damage;
    public bool m_IsAttack = false;

    protected CameraShake m_CameraShake;


    /*
    void Start()
    {
        heavySwingAction.Enable();
        quickSwingAction.Enable();
        heavySwingAction.performed += HeavySwing;       //回调攻击函数
        quickSwingAction.performed += QuickSwing;

        m_CameraShake = FindObjectOfType<CameraShake>();    //找拥有CameraShake脚本的组件
    }



    
    private void HeavySwing(InputAction.CallbackContext context)        //重攻击
    {
        if (!m_IsAttack)
        {
            m_IsAttack = true;
            m_Animator.SetTrigger("Swing");
            m_Animator.SetFloat("SwingType", 0f);
        }
    }

    private void QuickSwing(InputAction.CallbackContext context)        //轻攻击
    {
        if (!m_IsAttack)
        {
            m_IsAttack = true;
            m_Animator.SetTrigger("Swing");
            m_Animator.SetFloat("SwingType", 1f);
        }
    }

    private void PlayerAttackOver()    //用于动画事件（在不同的帧插入此函数）
    {
        m_IsAttack = false;
    }



    private void OnTriggerEnter2D(Collider2D other)     //检测是否击中敌人
    {
        if (other.CompareTag("Enemy"))
        {
            //敌人受伤
            other.GetComponent<EnemyFSM>().EnemyTakeDamage(damage);
            other.GetComponent<EnemyFSM>().EnemyGetHit(m_MoveDirection);

            //相机震动
            float shakeIntensity = m_Animator.GetFloat("SwingType") == 1 ? 1.5f : 2f;
            float shakeDuration = m_Animator.GetFloat("SwingType") == 1 ? 0.3f : 0.5f;

            if (m_CameraShake != null)
            {
                m_CameraShake.ShakeCamera(shakeIntensity, shakeDuration);
            }
        }
    }
    */
}
