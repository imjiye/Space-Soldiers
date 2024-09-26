using System.Collections;  // IEnumerator를 사용하기 위해 추가
using UnityEngine;

// 총알을 충전하는 아이템
public class BulletPack : MonoBehaviour, IItem
{
    public int ammo = 30; // 충전할 총알 수

    public void Use(GameObject target)
    {
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        if (playerShooter != null && playerShooter.gun != null)
        {
            // SoundManager.instance.PlaySound("BulletUp");
            playerShooter.gun.ammoRemain += ammo;
        }
        Destroy(gameObject);
    }


}
