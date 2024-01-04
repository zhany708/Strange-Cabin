using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitboxToWeapon : MonoBehaviour       //用于将AggressiveWeapon脚本中的函数转交给Weapon脚本
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
