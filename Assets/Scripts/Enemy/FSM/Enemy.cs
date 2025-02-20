using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// 敌人类
/// 实现状态切换,加载敌人巡逻路线
/// </summary>
public class Enemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Animator animator;
    private AudioSource audioSource;
    [Tooltip("怪物血量")]public float enemyHealth;
    [Tooltip("血量精度条")] public Slider slider;
    public TextMeshProUGUI getDamageText;
    [Tooltip("怪物死亡")]public GameObject dealEffect;


    public GameObject[] wayPointObj;//存放敌人不同路线
    public List<Vector3> wayPoints = new List<Vector3>();//存放巡逻路线里面的每一个巡逻点
    private EnemyBaseState curState;

    public int animState;
    public int index;
    public int nameIndex;
    public Transform targetPoint;

    public PatrolState patrolState;
    public AttackState attackState;
    public GameObject attackParticle01;
    public Transform attackParticle01Postion;
    public AudioClip attackSound;
    public bool isSurveyor; //是否是勘查者
    public bool isWin;

    Vector3 targetPostion;

    public List<Transform> attackList = new List<Transform>();
    [Tooltip("攻击间隔，时间越长攻击频率越慢")]public float attackRate;
    private float nextAttack = 0;//下次攻击时间
    [Tooltip("普通攻击距离")] public float attackRange;
    public bool isDead;


    private void Awake()
    {
        navMeshAgent =GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        patrolState = transform.gameObject.AddComponent<PatrolState>();
        attackState = transform.gameObject.AddComponent<AttackState>();
        index = 0;
    }
    void Start()
    {
        isDead = false;
        slider.minValue = 0;
        slider.maxValue = enemyHealth;
        slider.value = enemyHealth;
        TransitionToState(patrolState);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead) return;
        animator.SetInteger("state", animState);
        curState.OnUpdate(this);
    }

    /// <summary>
    /// 移动到目标点
    /// </summary>
    public void MoveToTarget()
    {
        if (attackList.Count == 0)
        {
            targetPostion = Vector3.MoveTowards(transform.position, wayPoints[index], navMeshAgent.speed);
        }else
        {
            targetPostion = Vector3.MoveTowards(transform.position, attackList[0].position, navMeshAgent.speed);
        }
        
        navMeshAgent.destination = targetPostion;
    }

    //加载路线
    public void LoadPath(GameObject go)
    {
        wayPoints.Clear();
        foreach (Transform T in go.transform)
        {
            wayPoints.Add(T.position);
        }
    }

    public void TransitionToState(EnemyBaseState state)
    {
        curState = state;
        curState.EnemyState(this);
    }

    //敌人受到伤害
    public void Health(float val,Player player)
    {
        if (isDead) return;
        getDamageText.text = Mathf.Round(val).ToString();
        enemyHealth -= val;
        slider.value = enemyHealth;
        if (slider.value <= 0)
        {
            animState = 0;
            animator.SetInteger("state", animState);
            animator.CrossFadeInFixedTime("Attack Mode",0.01f);
            isDead = true;
            animator.SetTrigger("dying");
            attackList.Remove(player.transform);
            if (isWin)
            {
                
            }
        }
        else if (!isDead && attackList.Count <= 0)
        {
            attackList.Add(player.transform);
            MoveToTarget();
        }
    }

    public void AttackAction()
    {
        if (Vector3.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time  > nextAttack)
            {
                animator.SetTrigger("attack");
                nextAttack = Time.deltaTime + attackRate;
            }
        }
    }

    public void PlayerMustatAttackEff()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!attackList.Contains(other.transform) && !isDead && !other.CompareTag("Bullect"))
        {
            attackList.Add(other.transform);
        };
    }

    private void OnTriggerExit(Collider other)
    {
        attackList.Remove(other.transform);
    }
}
