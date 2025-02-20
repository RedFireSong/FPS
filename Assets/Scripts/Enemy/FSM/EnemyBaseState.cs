using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于扩展实现敌人的一些基础状态
/// </summary>
public abstract class EnemyBaseState : MonoBehaviour
{
    public abstract void EnemyState(Enemy enemy);//首次进入状态
    public abstract void OnUpdate(Enemy enemy);//状态持续更新
}
