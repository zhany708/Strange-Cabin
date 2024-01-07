using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitboxToWeapon : MonoBehaviour       //���ڽ�AggressiveWeapon�ű��еĺ���ת����Weapon�ű�
{
    AggressiveWeapon m_Weapon;

    private void Awake()
    {
        m_Weapon = GetComponentInParent<AggressiveWeapon>();
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_Weapon.AddToDetected(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_Weapon.RemoveFromDetected(collision);
    }
}
