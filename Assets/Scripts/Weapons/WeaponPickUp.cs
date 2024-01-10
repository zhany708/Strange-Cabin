using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public GameObject WeaponPreFab;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponentInParent<Player>();

            player.ChangeWeapon(WeaponPreFab);
            //��Ҫʵ�֣�ͨ��һ�ַ�ʽ�����������������Ǹ�����


            Debug.Log("You got new weapon!");
        }
    }
}
