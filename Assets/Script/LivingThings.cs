using System;
using UnityEngine;

// 생명체로서 동작할 게임 오브젝트들을 위한 뼈대를 제공
// 체력, 데미지 받아들이기, 사망 기능, 사망 이벤트를 제공
public class LivingThings : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }

    public event Action onDeath;

    // 생명체가 활성화될때 상태를 리셋
    protected virtual void OnEnable()
    {
        dead = false;

        health = startingHealth;
    }

    // 데미지를 입는 기능
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    // 체력을 회복하는 기능
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            return;
        }
        health += newHealth;
    }

    // 사망 처리
    public virtual void Die()
    {
        Debug.Log("dead");
        if (onDeath != null)
        {
            Debug.Log("S_ondeath");
            onDeath();
            Debug.Log("E_ondeath");
        }
        dead = true;
    }
}