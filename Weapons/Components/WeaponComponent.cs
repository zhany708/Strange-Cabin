using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZhangYu.Weapons
{
    public abstract class WeaponComponent : MonoBehaviour
    {
        protected Weapon weapon;

        protected virtual void Awake()
        {
            weapon = GetComponent<Weapon>();
        }
    }
}
