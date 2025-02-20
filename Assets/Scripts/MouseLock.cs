using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MouseLock : MonoBehaviour
{

    public float sensitivity = 400f; //灵敏度
    private Transform playerTf; //玩家文字
    private float sensitivityRotation = 0;//摄像机上下旋转的数值
    private bool isMove;
    void Start()
    {
        sensitivityRotation = 0;
        playerTf = transform.GetComponentInParent<Player>().transform;
        Cursor.lockState = CursorLockMode.Locked; //锁定鼠标
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        var y = Input.GetAxis("Mouse Y");
        if (!isMove && y != 0)
        {
            isMove = true;
        }
        if (isMove)
        {
            float mouseY = y * sensitivity * Time.deltaTime;
            sensitivityRotation -= mouseY;
            sensitivityRotation = Mathf.Clamp(sensitivityRotation, -60, 60);
        }
        transform.localRotation = Quaternion.Euler(sensitivityRotation, 0, 0);
        playerTf.Rotate(Vector3.up * mouseX);
    }
}
