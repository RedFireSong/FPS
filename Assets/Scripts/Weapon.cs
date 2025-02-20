using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void GunFire();

    public abstract void Reload();
    public abstract void DoReloadAnimation ();
    public abstract void ExpaningCrossUpdate(float expanDegree);//准心开合

    public abstract void AimIn(int val); //瞄准
    public abstract void AimOut(); //退出瞄准
}
