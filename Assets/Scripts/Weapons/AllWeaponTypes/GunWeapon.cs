using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhangYu.Utilities;

public class GunWeapon : Weapon
{
    Flip m_GunFlip;




    protected override void Start()
    {
        base.Start();

        m_GunFlip = new Flip(transform);
    }


    protected override void PointToMouse()
    {
        base.PointToMouse();

        int flipNum = player.InputHandler.ProjectedMousePos.x < transform.position.x ? -1 : 1;    //当鼠标位于枪械左侧时，翻转枪械的Y值

        m_GunFlip.FlipY(flipNum);
    }
}
