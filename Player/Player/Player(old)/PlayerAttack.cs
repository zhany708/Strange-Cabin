using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    
    //�������
    public InputAction heavySwingAction;
    public InputAction quickSwingAction;
    public float quickSwingSpeed;    //�����ٶ�
    public float heavySwingSpeed;
    public int damage;
    public bool m_IsAttack = false;

    protected CameraShake m_CameraShake;


    /*
    void Start()
    {
        heavySwingAction.Enable();
        quickSwingAction.Enable();
        heavySwingAction.performed += HeavySwing;       //�ص���������
        quickSwingAction.performed += QuickSwing;

        m_CameraShake = FindObjectOfType<CameraShake>();    //��ӵ��CameraShake�ű������
    }



    
    private void HeavySwing(InputAction.CallbackContext context)        //�ع���
    {
        if (!m_IsAttack)
        {
            m_IsAttack = true;
            m_Animator.SetTrigger("Swing");
            m_Animator.SetFloat("SwingType", 0f);
        }
    }

    private void QuickSwing(InputAction.CallbackContext context)        //�ṥ��
    {
        if (!m_IsAttack)
        {
            m_IsAttack = true;
            m_Animator.SetTrigger("Swing");
            m_Animator.SetFloat("SwingType", 1f);
        }
    }

    private void PlayerAttackOver()    //���ڶ����¼����ڲ�ͬ��֡����˺�����
    {
        m_IsAttack = false;
    }



    private void OnTriggerEnter2D(Collider2D other)     //����Ƿ���е���
    {
        if (other.CompareTag("Enemy"))
        {
            //��������
            other.GetComponent<EnemyFSM>().EnemyTakeDamage(damage);
            other.GetComponent<EnemyFSM>().EnemyGetHit(m_MoveDirection);

            //�����
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
