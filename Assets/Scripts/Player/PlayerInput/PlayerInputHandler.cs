using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;


public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }       //��ֹ�ֱ���΢�ƶ�ʱ����ٶȻ�����ǿ����0-1֮���С����Ϊ1��
    public Vector2 ProjectedMousePos { get; private set; }      //���ڳ�������������꣨��ʹ��꾲ֹ��


    public bool[] AttackInputs { get; private set; }    //���ڼ���������͸�����


    private Vector2 m_MousePos;






    private void Start()
    {
        int count = Enum.GetValues(typeof(CombatInputs)).Length;        //���ض���Ҳ�������ֹ�������

        AttackInputs = new bool[count];
    }

    private void Update()
    {
        if (m_MousePos != null)
        {
            ProjectedMousePos = Camera.main.ScreenToWorldPoint(m_MousePos);        //�����������������ĳ��������
        }
    }




    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>().normalized;   //(0,1) (0,-1) (1,0) (-1,0)����������ʾ����
    }



    public void OnPrimaryAttackInput(InputAction.CallbackContext context) 
    {
        m_MousePos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());        //�����������������ĳ��������

        if (context.started)    //����������ʱ
        {
            AttackInputs[(int)CombatInputs.primary] = true;
        }

        if (context.canceled)   //�ɿ�������ʱ
        {
            AttackInputs[(int)CombatInputs.primary] = false;
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)    //��������Ҽ�ʱ
        {
            AttackInputs[(int)CombatInputs.secondary] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.secondary] = false;
        }
    }



    public void OnAim(InputAction.CallbackContext context)      //���ڴ����������
    {
        m_MousePos = context.ReadValue<Vector2>();
    }
}


public enum CombatInputs
{ 
    primary,
    secondary
}
