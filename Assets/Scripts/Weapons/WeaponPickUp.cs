using UnityEngine;


public class WeaponPickUp : MonoBehaviour
{
    public GameObject WeaponPreFab;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponentInParent<Player>();    //������ײ������ҵ�combat�����壬���Ҫ��InParent

            player.ChangeWeapon(WeaponPreFab);
            //��Ҫʵ�֣�ͨ��һ�ַ�ʽ�����������������Ǹ�����


            //Debug.Log("You got new weapon!");

            Destroy(gameObject);    //��ɫ�������������
        }
    }
}
