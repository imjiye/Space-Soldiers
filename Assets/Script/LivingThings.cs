using System;
using UnityEngine;

// ����ü�μ� ������ ���� ������Ʈ���� ���� ���븦 ����
// ü��, ������ �޾Ƶ��̱�, ��� ���, ��� �̺�Ʈ�� ����
public class LivingThings : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float health { get; protected set; }
    public bool dead { get; protected set; }

    public event Action onDeath;

    // ����ü�� Ȱ��ȭ�ɶ� ���¸� ����
    protected virtual void OnEnable()
    {
        dead = false;

        health = startingHealth;
    }

    // �������� �Դ� ���
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    // ü���� ȸ���ϴ� ���
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            return;
        }
        health += newHealth;
    }

    // ��� ó��
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