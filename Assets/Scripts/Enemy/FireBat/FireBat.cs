using UnityEngine;


public class FireBat : Enemy
{
    public GameObject FireBallPrefab;


    protected override void Start()
    {
        base.Start();

        AttackState = new FireBatAttackState(this, StateMachine, enemyData, "Attack");    //����ͨ����״̬�ĳɻ����𹥻�״̬
    }




    public void FireBallLaunch(Transform target)
    {
        Vector2 attackX = Core.Animator.GetFloat("MoveX") > Mathf.Epsilon ? Vector2.right : Vector2.left;      //���ݶ�������MoveX�жϵ��˳���
        float deviation = Mathf.Abs(Core.Animator.GetFloat("MoveY")) >= Mathf.Abs(Core.Animator.GetFloat("MoveX")) ? 0f : 0.2f;     //ƫ����������ݵ����泯�������ƫ���첿���٣�
        Vector2 attackPosition = Movement.Rigidbody2d.position + Vector2.up * 0.8f + attackX * deviation;       //��������λ����y����Ӧλ��ͷ����x����Ӧƫ����˵�λ�ã��첿���䣩


        float angle = Mathf.Atan2((target.position.y + 0.5f - attackPosition.y), (target.position.x - attackPosition.x)) * Mathf.Rad2Deg;      //����������������֮��ļн�

        //���ɻ��򣬲������������ת
        GameObject FireBallObject = ParticlePool.Instance.GetObject(FireBallPrefab);
        FireBallObject.transform.position = attackPosition;
        FireBallObject.transform.rotation = Quaternion.Euler(0, 0, angle);



        EnemyBullet fireBall = FireBallObject.GetComponent<EnemyBullet>();        //���û���ű�
        fireBall.SetSpeed(target.position + Vector3.up * 0.5f - FireBallObject.transform.position);        //����ɫ���ķ��������
    }
}
