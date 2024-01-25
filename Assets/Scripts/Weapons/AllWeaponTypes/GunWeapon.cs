using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhangYu.Utilities;

public class GunWeapon : Weapon
{
    public GameObject BulletPrefab;
    public SO_GunData GunData {  get; private set; }



    protected Transform muzzlePos;



    Flip m_GunFlip;





    protected override void Awake()
    {
        base.Awake();

        if (WeaponData.GetType() == typeof(SO_GunData))
        {
            GunData = (SO_GunData)WeaponData;     //如果当前WeaponData与GunWeaponData相同，则用父类中的通用WeaponData赋值此脚本中的枪械武器数据
        }
        else
        {
            Debug.LogError("Wrong data for the weapon");
        }
    }

    protected override void Start()
    {
        base.Start();

        m_GunFlip = new Flip(transform);

        muzzlePos = transform.Find("Muzzle");
    }



    public override void EnterWeapon()
    {
        base.EnterWeapon();

        Fire();
    }



    protected override void PointToMouse()
    {
        base.PointToMouse();

        int flipNum = player.InputHandler.ProjectedMousePos.x < transform.position.x ? -1 : 1;    //当鼠标位于枪械左侧时，翻转枪械的Y值

        m_GunFlip.FlipY(flipNum);
    }




    private void Fire()
    {
        GameObject bullet = ParticlePool.Instance.GetObject(BulletPrefab);

        bullet.transform.position = muzzlePos.position;     //生成子弹后更改位置于枪口位置
        bullet.GetComponent<PlayerBullet>().SetWeapon(this);
        bullet.GetComponent<PlayerBullet>().SetSpeed(mousePosition);     //使子弹向鼠标位置移动（不能用武器脚本中的mousePosition，否则玩家切换武器后的第一发子弹移动方向会出错）
    }
}
