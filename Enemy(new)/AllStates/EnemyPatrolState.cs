using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    Vector2 m_RandomPosition;       //�������
    float m_PatrolTimer;

    public EnemyPatrolState(Enemy enemy, EnemyStateMachine stateMachine, SO_EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //�����������
        m_RandomPosition = new Vector2(Random.Range(enemy.GetLeftDownPos().x, enemy.GetRightTopPos().x), Random.Range(enemy.GetLeftDownPos().y, enemy.GetRightTopPos().y));
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        m_PatrolTimer += Time.deltaTime;
        Movement.SetAnimationDirection(m_RandomPosition, enemy.transform.position);      //����Ѳ�ߵ�ķ���

        if (enemy.Parameter.Target != null && !enemy.CheckOutside())
        {
            stateMachine.ChangeState(enemy.ChaseState);     //Ѳ��ʱ�����⵽������л�Ϊ׷��״̬
        }

        else if (Vector2.Distance(enemy.transform.position, m_RandomPosition) < 0.1f)     //������Ŀ��Ѳ�ߵ��㹻��ʱ
        {
            stateMachine.ChangeState(enemy.IdleState);
        }

        else if (m_PatrolTimer >= 5f)
        {
            stateMachine.ChangeState(enemy.IdleState);      //���5��������û�е���Ѳ�ߵ㣨��ס�ˣ�����ǿ��ת��������״̬
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (m_RandomPosition != null)
        {
            //�ƶ���Ŀ���
            enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, m_RandomPosition, enemyData.MoveSpeed * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        base.Exit();

        m_PatrolTimer = 0f;
    }
}