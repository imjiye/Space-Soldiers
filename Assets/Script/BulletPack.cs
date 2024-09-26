using System.Collections;  // IEnumerator�� ����ϱ� ���� �߰�
using UnityEngine;

// �Ѿ��� �����ϴ� ������
public class BulletPack : MonoBehaviour, IItem
{
    public int ammo = 30; // ������ �Ѿ� ��

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
