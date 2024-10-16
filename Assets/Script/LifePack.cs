using UnityEngine;

// ü���� ȸ���ϴ� ������
public class LifePack : MonoBehaviour, IItem
{
    public float health = 50;

    public void Use(GameObject target)
    {
        LivingThings life = target.GetComponent<LivingThings>();

        if (life != null)
        {
            SoundManager.instance.PlaySFX("life");
            
            life.RestoreHealth(health);
        }
        Destroy(gameObject);
        
    }

    
}