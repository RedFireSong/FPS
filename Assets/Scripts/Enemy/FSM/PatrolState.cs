using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人进入巡逻状态
/// </summary>
public class PatrolState : EnemyBaseState
{
    public override void EnemyState(Enemy enemy)
    {
        enemy.animState = 0;
        enemy.LoadPath(enemy.wayPointObj[WayPointMager.Instance.usingIndex[enemy.nameIndex]]);
    }

    public override void OnUpdate(Enemy enemy)
    {
        if (!enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            enemy.animState = 1;
            enemy.MoveToTarget();
        }

        //计算敌人和导航点的距离
        float distance = Vector3.Distance(enemy.transform.position, enemy.wayPoints[enemy.index]);
        if (distance <= 1f)
        {
            enemy.animState = 0;
            enemy.animator.Play("Idle");
            enemy.index++;
            enemy.index = Mathf.Clamp(enemy.index,0, enemy.wayPoints.Count - 1);
            if (Vector3.Distance(enemy.transform.position, enemy.wayPoints[enemy.wayPoints.Count - 1]) <= 1)
            {
                enemy.index = 0;
            }
        }

        if (enemy.attackList.Count > 0)
        {
            enemy.TransitionToState(enemy.attackState);
        }
    }
}
