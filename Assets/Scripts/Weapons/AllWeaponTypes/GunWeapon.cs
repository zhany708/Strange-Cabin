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
            GunData = (SO_GunData)WeaponData;     //�����ǰWeaponData��GunWeaponData��ͬ�����ø����е�ͨ��WeaponData��ֵ�˽ű��е�ǹе��������
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

        int flipNum = player.InputHandler.ProjectedMousePos.x < transform.position.x ? -1 : 1;    //�����λ��ǹе���ʱ����תǹе��Yֵ

        m_GunFlip.FlipY(flipNum);
    }




    private void Fire()
    {
        GameObject bullet = ParticlePool.Instance.GetObject(BulletPrefab);

        float offsetAngle = Random.Range(-5f, 5f);      //����С��ƫ���ӵ��������ӵ���ȫ������귢�䣩

        bullet.transform.position = muzzlePos.position;     //�����ӵ������λ����ǹ��λ��
        bullet.GetComponent<PlayerBullet>().SetWeapon(this);
        bullet.GetComponent<PlayerBullet>().SetSpeed(Quaternion.AngleAxis(offsetAngle, Vector3.forward) * mousePosition);     //ʹ�ӵ������λ���ƶ�������������ĽǶ�ƫ��

        PlayAudioSound();   //���ſ�ǹ��Ч
    }
}
