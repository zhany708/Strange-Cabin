using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }       //��ֹ�ֱ���΢�ƶ�ʱ����ٶȻ�����ǿ����0-1֮���С����Ϊ1��

    public bool[] AttackInputs { get; private set; }    //���ڼ���������͸�����


    private void Start()
    {
        int count = Enum.GetValues(typeof(CombatInputs)).Length;        //���ض���Ҳ�������ֹ�������

        AttackInputs = new bool[count];
    }




    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>().normalized;   //(0,1) (0,-1) (1,0) (-1,0)����������ʾ����
    }



    public void OnPrimaryAttackInput(InputAction.CallbackContext context) 
    {
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
}


public enum CombatInputs
{ 
    primary,
    secondary
}
