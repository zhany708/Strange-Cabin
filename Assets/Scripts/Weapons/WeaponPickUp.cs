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
            //需要实现：通过一种方式决定更换主武器还是副武器


            Debug.Log("You got new weapon!");
        }
    }
}
