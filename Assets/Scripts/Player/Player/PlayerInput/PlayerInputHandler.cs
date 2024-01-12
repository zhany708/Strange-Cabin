using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }       //防止手柄轻微移动时玩家速度缓慢（强制让0-1之间的小数设为1）

    public bool[] AttackInputs { get; private set; }    //用于检测主武器和副武器


    private void Start()
    {
        int count = Enum.GetValues(typeof(CombatInputs)).Length;        //返回二，也就是两种攻击武器

        AttackInputs = new bool[count];
    }




    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>().normalized;   //(0,1) (0,-1) (1,0) (-1,0)四种向量表示方向
    }



    public void OnPrimaryAttackInput(InputAction.CallbackContext context) 
    {
        if (context.started)    //按下鼠标左键时
        {
            AttackInputs[(int)CombatInputs.primary] = true;
        }

        if (context.canceled)   //松开鼠标左键时
        {
            AttackInputs[(int)CombatInputs.primary] = false;
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)    //按下鼠标右键时
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
