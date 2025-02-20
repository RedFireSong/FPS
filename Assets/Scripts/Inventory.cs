using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
   public List<GameObject> weapons = new List<GameObject>();
   private int curWeaponsID;
   public bool isGun;//外面不要填,这个字段是用来判断是不是狙击枪
    void Start()
    {
        curWeaponsID = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ChargeCurrentWeaponlD();
    }

    public void ChargeCurrentWeaponlD()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //下一把武器
            isGun = false;
            if (curWeaponsID + 1 > weapons.Count-1)
            {
                curWeaponsID = 0;
            }
            else
            {
                curWeaponsID += 1;

            }
            ChargeWeapon(curWeaponsID);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //上一把武器
            isGun = false;
            if (curWeaponsID - 1 < 0)
            {
                curWeaponsID = weapons.Count-1;
            }
            else
            {
                curWeaponsID -= 1;
            }
            ChargeWeapon(curWeaponsID);
        }
    }

    public void ChargeWeapon(int Id)
    {
        if (weapons.Count == 0) return;
        curWeaponsID = Id;
      
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(Id == i);
        }
        isGun = Id == 1;
    }
}
