using UnityEngine;


[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/enemy Data/Base Data")]
public class SO_EnemyData : ScriptableObject
{
    [Header("Move")]
    public float MoveSpeed = 0;
    public float ChaseSpeed = 0;
    public float IdleDuration = 0;      //����ʱ��
    public float StoppingDistance = 0;      //��������ҵ���С����

    [Header("Attack")]
    public LayerMask TargetLayer = 0;
    public float AttackArea = 0;        //Բ�İ뾶����
    public float AttackInterval = 0;    //�������

    [Header("Hit")]
    public float MaxHealth = 0;
    public float Defense = 0;
    public float HitResistance = 0;
    public float HitInterval = 0;   //�޵�ʱ��
}
