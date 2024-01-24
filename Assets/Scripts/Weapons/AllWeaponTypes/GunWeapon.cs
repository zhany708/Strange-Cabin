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

        int flipNum = player.InputHandler.ProjectedMousePos.x < transform.position.x ? -1 : 1;    //�����λ��ǹе���ʱ����תǹе��Yֵ

        m_GunFlip.FlipY(flipNum);
    }
}
