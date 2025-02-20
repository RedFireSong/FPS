using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float walkSpeed; //行走速度
    public float runSpeed; //奔跑速度
    public float crouchSpeed; //下蹲移动速度
    public Animator animator;

    private float speed;
    private float jumpForce;//跳跃的力
    private float fallForce;//下落的力

    private Vector3 moveDirction; //任务移动方向
    private KeyCode runName = KeyCode.LeftShift;
    private KeyCode jumpName = KeyCode.Space;
    private KeyCode crouchName = KeyCode.LeftControl;
    private CollisionFlags collisionFlags;
    private CharacterController controller;
    public bool isAiming = true;

    public bool isRun;
    public bool isJump;
    public bool isGround = true;//是否在地面上
    private bool isCrouch;
    private bool isRunAnim;
    private bool isWalkAnim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        speed = walkSpeed;
        walkSpeed = 4f;
        runSpeed = 6f;
        crouchSpeed = 2f;
        jumpForce = 0;
        fallForce = 10;
        isGround = true;
    }

    void Update()
    {
        if (animator == null) return;
        Jump();
        Move();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
      
        isRun = Input.GetKey(runName);
        isCrouch = Input.GetKey(crouchName);
        if (Input.GetMouseButton(0))
        {
            animator.SetBool("Run", false);
            animator.SetBool("WalkorRun", false);
            animator.SetBool("RunorWalk", false);
            animator.SetBool("Walk", true);
        }
        else
        {
            if (h > 0.1f || h < 0 || v > 0.1f || v < 0)
            {
                if (isRun && isGround && isAiming)
                {
                    animator.SetBool("Run", true);
                    animator.SetBool("WalkorRun", true);
                    animator.SetBool("RunorWalk", false);
                    animator.SetBool("Walk", false);

                }
                else if (isGround)
                {
                    animator.SetBool("Run", false);
                    animator.SetBool("WalkorRun", false);
                    animator.SetBool("RunorWalk", true);
                    animator.SetBool("Walk", true);
                }
            }
            else
            {
                animator.SetBool("Run", false);
                animator.SetBool("Walk", false);
                animator.SetBool("WalkorRun", false);
                animator.SetBool("RunorWalk", false);

            }
        }
        if (isRun && isGround)
        {
            speed = runSpeed;
        }
        else if (isCrouch)
        {
                speed = crouchSpeed;
        }
        else if(isGround)
        {
            speed = walkSpeed;
        }
        moveDirction = (transform.right * h + transform.forward * v).normalized;
        controller.Move(moveDirction* speed * Time.deltaTime);
    }

    void Jump()
    {
        isJump = Input.GetKeyDown(jumpName);
        if (isJump && isGround)
        {
            isGround = false;
            jumpForce = 5;
        }else if (!isJump && isGround)
        {
            isGround = false;
        }

        //判断是否在地面上
        if (!isGround)
        {
            jumpForce = jumpForce- fallForce * Time.deltaTime;
            Vector3 jump = new Vector3(0, jumpForce * Time.deltaTime, 0);
            collisionFlags = controller.Move(jump);

            //判断在不在地面上,在的话就能跳跃
            if (collisionFlags == CollisionFlags.Below)
            {
                isGround = true;
                jumpForce = -2;
            }
            //if (isGround && collisionFlags == CollisionFlags.None)
            //{
            //    isGround = false;
            //}
        }
    }
}
