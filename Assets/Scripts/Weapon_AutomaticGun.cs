using System.Collections;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[System.Serializable]
public class SundClips
{
    public AudioClip shootSound;
     public AudioClip reloadShootSound;
}

public class Weapon_AutomaticGun : Weapon
{
    [Tooltip("子弹")] public Transform BulletPrefab;//子弹打出的特效
    [Header("枪械部位")]
    [Tooltip("射击的位置")] public Transform ShootPoint;//射线打出的位置
    [Tooltip("特效的位置")] public Transform BulletShootPoint;//子弹打出的特效

    [Header("枪械属性")]
    [Tooltip("武器射程")] public float range;
    [Tooltip("武器射速")] public float fireRate;
    [Tooltip("弹壳数量")] public int bulletNum;
    [Tooltip("备弹数")] public int bulletLeft;
    public ParticleSystem muzzlePartic;//开火特效
    public SundClips sundClips;
    public TextMeshProUGUI textMesh;
    public Camera gunCamera;
    public GameObject holder;
    public float minDamage;
    public float maxDamage;

    private float originRate;//原始射速
    private float SpreadFactor;//射击的一点偏移量
    private float fireTimer;//计时器 控制武器射速
    private float bulletForce;//子弹发射的力
    private int curBulletNume; //当前子弹数量
    private Vector3 moveDirction; //任务移动方向
    private float muzzleTime;//开火灯光时间
    private bool isReload;//是否在换弹
    private bool isAiming;//判断是否在瞄准
    private bool isGrow;

    private Player player;
    private Animator animator;
    private AudioSource mainAudioSource;
    private AudioSource reloadAudioSource;
    private Vector3 sniperingFiflePosition;//枪默认的位置
    public Vector3 sniperingFifleOnPosition;//开始瞄准的位置
    public Vector3 sniperingFifleOnBull;//开镜之后瞄准的子弹位置
    private Camera mainCamera;
    private Canvas canvas;
    private Inventory inventory;
    private bool isFier;
    public bool isEnemyInjured;
    private bool isAdd;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        mainAudioSource = GetComponent<AudioSource>();
        reloadAudioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        gunCamera = GetComponentInChildren<Camera>();
        canvas = GetComponentInChildren<Canvas>();
        holder = canvas.transform.Find("Crosshair/Holder").gameObject;
        inventory = GetComponentInParent<Inventory>();
    }

    private void Start()
    {
        range = 300;
        fireTimer = 0;
        if (fireRate == 0)
        {
            fireRate = 0.1f;
        }
        bulletNum = 30;
        bulletLeft = bulletNum * 5;
        curBulletNume = bulletNum;
        muzzleTime = 0.2f;
        muzzlePartic.gameObject.SetActive(false);
        bulletForce = 100;
        sniperingFiflePosition = transform.localPosition;
        sniperingFifleOnPosition = new Vector3(-0.13f,-1.66f,-0.141f);
        if (sniperingFifleOnBull == Vector3.zero)
        {
            sniperingFifleOnBull = new Vector3(0.1f, -0.1f, 0.2f);
        }
        UpDateUI();
    }

    private void Update()
    {
        if (isFier ||  fireTimer <= fireRate)
        {
            fireTimer += Time.deltaTime;
        }
        var info = animator.GetCurrentAnimatorStateInfo(0);
        isReload = info.IsName("Recharge");
        if (Input.GetKeyDown(KeyCode.R) && !isReload)
        {
            DoReloadAnimation();
        }
        if (Input.GetMouseButton(1) && !isReload)
        {
            BulletShootPoint.transform.localPosition = sniperingFifleOnBull;
            holder.SetActive(false);
            if (!isGrow)
            {
                AimIn(30);
            }
            isAiming = true;
            animator.SetBool("Aim", isAiming);
            animator.SetBool("Run", !isAiming);
           
            player.isAiming = !isAiming;
            transform.localPosition = sniperingFifleOnPosition;
            isGrow = true;
        }
        else
        {
            BulletShootPoint.transform.localPosition = sniperingFifleOnBull;
            holder.SetActive(true);
            AimIn(60);
            isGrow = false;
            isAiming = false;
            animator.SetBool("Aim", isAiming);
            if (isAiming)
            {
                animator.SetBool("Run", !isAiming);
            }
            player.isAiming = !isAiming;
            transform.localPosition = sniperingFiflePosition;
        }
        if (fireTimer > fireRate && curBulletNume > 0 && !isReload)
        {
           
            if (Input.GetMouseButton(0))
            {
                if (!isAiming)
                {
                   
                    muzzlePartic.transform.localPosition = new Vector3(0.169f, 0, 0);
                    animator.CrossFadeInFixedTime("Singl_Shot", 0.1f);
                }
                else
                {
                  
                    muzzlePartic.transform.localPosition = new Vector3(0, 0, 0);
                    if (inventory.isGun)
                    {
                        animator.CrossFadeInFixedTime("Singl_Shot", 0.1f);
                    }
                    else
                    {
                        animator.CrossFadeInFixedTime("Get", 0.1f);
                    }
                    
                   // animator.Play("Singl_Shot1", 0, 0.1f);
                }
                //开枪射击
                curBulletNume--;
                GunFire();
                UpDateUI();
                isFier = true;
            }
        }
    }

    public override void GunFire()
    {
        StartCoroutine(MuzzleFlash());
        muzzlePartic.Emit(1);
        RaycastHit hit;
        Vector3 shootDirection = ShootPoint.forward;//射击向前方射击
        shootDirection = shootDirection + ShootPoint.TransformDirection(new Vector3(Random.Range(-SpreadFactor, SpreadFactor), Random.Range(-SpreadFactor, SpreadFactor)));
        if (Physics.Raycast(ShootPoint.position, shootDirection, out hit, range))
        {
            // GameObject bull = Instantiate(BulletPrefab, BulletShootPoint.transform.position, BulletShootPoint.transform.rotation);
            GameObject bull = Instantiate(BulletPrefab.gameObject, BulletShootPoint.transform.position, BulletShootPoint.transform.rotation);
            //bull.transform.SetParent(BulletShootPoint);
            bull.GetComponent<Rigidbody>().velocity = (bull.transform.forward + shootDirection)* bulletForce;
            if (hit.transform.gameObject.tag == "Enemy")
            {
                //if (!hit.transform.gameObject.GetComponent<Enemy>().isDead && hit.transform.gameObject.GetComponent<Enemy>().attackList.Count <= 0)
                //{
                //    // !attackList.Contains(other.transform) && !isDead && !other.CompareTag("Bullect")
                //    hit.transform.gameObject.GetComponent<Enemy>().attackList.Add(player.transform);
                //}
                hit.transform.gameObject.GetComponent<Enemy>().Health(Random.Range(minDamage, maxDamage),player);
                //if (hit.transform.gameObject.GetComponent<Enemy>().isDead)
                //{
                //   // hit.transform.gameObject.GetComponent<Enemy>().attackList.Remove(player.transform);
                //    //hit.transform.gameObject.GetComponent<Enemy>().animator.SetTrigger("Walk", false);
                //    hit.transform.gameObject.GetComponent<Enemy>().animState = 0;
                //}
            }
           
           
           Debug.Log(hit.transform.gameObject.name + " 打到了");
        }
        mainAudioSource.clip = sundClips.shootSound;
        mainAudioSource.Play();
        fireTimer = 0;
    }

    public IEnumerator MuzzleFlash()
    {
        muzzlePartic.gameObject.SetActive(true);
        yield return new WaitForSeconds(muzzleTime);
        muzzlePartic.gameObject.SetActive(false);
    }

    public override void DoReloadAnimation()
    {
        if (curBulletNume >= 0 && bulletLeft > 0)
        {
            animator.Play("Recharge",0,0);
           
            reloadAudioSource.clip= sundClips.reloadShootSound;
            reloadAudioSource.Play();
            Reload();
        }
    }

    public override void Reload()
    {
        if (bulletLeft <= 0) return;
        int bulletToLoad = bulletNum - curBulletNume; //换弹夹时计算还差多少颗子弹
        int bulletToReduce = bulletLeft >= bulletToLoad ? bulletToLoad : bulletLeft;//计算备弹扣除的子弹数
        bulletLeft -= bulletToReduce;
        curBulletNume += bulletToReduce;
        UpDateUI();
    }

    public void UpDateUI()
    {
        textMesh.text = curBulletNume + "/" + bulletLeft;
    }

    public override void AimIn(int val)
    {
        float curVelocity = 0;
        mainCamera.fieldOfView = Mathf.SmoothDamp(val, 60, ref curVelocity,0.1f);
    }

    public override void AimOut()
    {
    }

    public override void ExpaningCrossUpdate(float expanDegree)
    {
    }

   
}
