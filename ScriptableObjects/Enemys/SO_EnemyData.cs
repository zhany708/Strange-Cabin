using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/enemy Data/Base Data")]
public class SO_EnemyData : ScriptableObject
{
    [Header("Move State")]
    public float MoveSpeed;
    public float ChaseSpeed;
    public float IdleDuration;      //����ʱ��
    public float StoppingDistance;      //��������ҵ���С����

    [Header("Attack State")]
    public LayerMask TargetLayer;
    public float AttackArea;        //Բ�İ뾶����
    public float AttackInterval;    //�������

    [Header("Hit State")]
    public float HitInterval;   //�޵�ʱ��
}
