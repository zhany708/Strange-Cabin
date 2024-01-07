using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/enemy Data/Base Data")]
public class SO_EnemyData : ScriptableObject
{
    [Header("Move State")]
    public float MoveSpeed;
    public float ChaseSpeed;
    public float IdleDuration;      //待机时长
    public float StoppingDistance;      //敌人与玩家的最小距离

    [Header("Attack State")]
    public LayerMask TargetLayer;
    public float AttackArea;        //圆的半径参数
    public float AttackInterval;    //攻击间隔

    [Header("Hit State")]
    public float HitInterval;   //无敌时间
}
