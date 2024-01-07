using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBat : Enemy
{
    public GameObject FireBallPrefab;


    protected override void Start()
    {
        base.Start();

        AttackState = new FireBatAttackState(this, StateMachine, enemyData, "Attack");    //将普通攻击状态改成火蝙蝠攻击状态
    }




    public void FireBallLaunch(Transform target)
    {
        Vector2 attackX = Core.Animator.GetFloat("MoveX") > Mathf.Epsilon ? Vector2.right : Vector2.left;      //根据动画参数MoveX判断敌人朝向
        float deviation = Mathf.Abs(Core.Animator.GetFloat("MoveY")) >= Mathf.Abs(Core.Animator.GetFloat("MoveX")) ? 0f : 0.2f;     //偏离参数（根据敌人面朝方向决定偏离嘴部多少）
        Vector2 attackPosition = Movement.Rigidbody2d.position + Vector2.up * 0.8f + attackX * deviation;       //火球生成位置在y轴上应位于头部，x轴上应偏离敌人的位置（嘴部发射）


        float angle = Mathf.Atan2((target.position.y + 0.5f - attackPosition.y), (target.position.x - attackPosition.x)) * Mathf.Rad2Deg;      //计算火球与玩家中心之间的夹角

        GameObject FireBallObject = Instantiate(FireBallPrefab, attackPosition, Quaternion.Euler(0, 0, angle));      //生成火球

        FireBall fireBall = FireBallObject.GetComponent<FireBall>();        //调用火球脚本
        fireBall.Launch(target.position + Vector3.up * 0.5f - FireBallObject.transform.position, 150);        //朝角色中心方向发射火球
    }
}
