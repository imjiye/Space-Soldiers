using UnityEngine;
using UnityEngine.UI; // UI ���� �ڵ�

// �÷��̾� ĳ������ ����ü�μ��� ������ ���
public class PlayerHealth : LivingThings
{
    public Slider healthSlider; // ü���� ǥ���� UI �����̴�

    //public AudioClip deathClip; // ��� �Ҹ�
    //public AudioClip hitClip; // �ǰ� �Ҹ�
    //public AudioClip itemPickupClip; // ������ ���� �Ҹ�

    //private AudioSource playerAudioPlayer; // �÷��̾� �Ҹ� �����
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����

    public ParticleSystem Life_P;
    public ParticleSystem Bullet_P;
    public ParticleSystem Coin_P;

    private PlayerMove playerMovement; // �÷��̾� ������ ������Ʈ
    private PlayerShooter playerShooter; // �÷��̾� ���� ������Ʈ

    private void Awake()
    {
        // ����� ������Ʈ�� ��������
        playerAnimator = GetComponent<Animator>();
        //playerAudioPlayer = GetComponent<AudioSource>();

        playerMovement = GetComponent<PlayerMove>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        // LivingEntity�� OnEnable() ���� (���� �ʱ�ȭ)
        base.OnEnable();

        // ü�� �����̴� Ȱ��ȭ
        healthSlider.gameObject.SetActive(true);
        // ü�� �����̴��� �ִ��� �⺻ ü�°����� ����
        healthSlider.maxValue = startingHealth;
        // ü�� �����̴��� ���� ���� ü�°����� ����
        healthSlider.value = health;

        // �÷��̾� ������ �޴� ������Ʈ Ȱ��ȭ
        playerMovement.enabled = true;
        playerShooter.enabled = true;

        Life_OffEffect();
        Bullet_OffEffect();
        Coin_OffEffect();
    }

    // ü�� ȸ��
    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity�� RestoreHealth() ���� (ü�� ����)
        base.RestoreHealth(newHealth);
        // ���ŵ� ü������ ü�� �����̴� ����
        healthSlider.value = health;
    }

    // ������ ó��
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (!dead)
        {
            // ������� ���� ��쿡�� ȿ���� ���
            SoundManager.instance.PlaySFX("Hit");
            //playerAudioPlayer.PlayOneShot(hitClip);
        }

        // LivingEntity�� OnDamage() ����(������ ����)
        base.OnDamage(damage, hitPoint, hitDirection);
        // ���ŵ� ü���� ü�� �����̴��� �ݿ�
        healthSlider.value = health;
    }

    // ��� ó��
    public override void Die()
    {
        Debug.Log("Die");
        // LivingEntity�� Die() ����(��� ����)
        base.Die();

        // ü�� �����̴� ��Ȱ��ȭ
        healthSlider.gameObject.SetActive(false);

        // �ִϸ������� Die Ʈ���Ÿ� �ߵ����� ��� �ִϸ��̼� ���
        playerAnimator.SetTrigger("Die");

        // �÷��̾��� ������ �޴� ������Ʈ ��Ȱ��ȭ
        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // �����۰� �浹�� ��� �ش� �������� ����ϴ� ó��
        // ������� ���� ��쿡�� ������ ��� ����
        if (!dead)
        {
            // �浹�� �������κ��� IItem ������Ʈ �������� �õ�
            IItem item = other.GetComponent<IItem>();

            // �浹�� �������κ��� IItem ������Ʈ�� �������� �� �����ߴٸ�
            if (item != null)
            {
                // Use �޼��带 �����Ͽ� ������ ���
                item.Use(gameObject);
                if(other.tag == "Life")
                {
                    Life_P.Play();
                    SoundManager.instance.PlaySFX("Life");
                    Invoke("Life_OffEffect", 3f);
                }
                else if(other.tag == "Bullet")
                {
                    Bullet_P.Play();
                    SoundManager.instance.PlaySFX("Bullet");
                    Invoke("Bullet_OffEffect", 3f);
                }
                else if(other.tag == "Coin")
                {
                    Coin_P.Play();
                    SoundManager.instance.PlaySFX("Coin");
                    Invoke("Coin_OffEffect", 3f);
                }
                
            }
        }
    }

    public void Life_OffEffect()
    {
        Life_P.Stop();
        SoundManager.instance.StopSFX();
    }
        public void Bullet_OffEffect()
    {
        Bullet_P.Stop();
        SoundManager.instance.StopSFX();
    }

    public void Coin_OffEffect()
    {
        Coin_P.Stop();
        SoundManager.instance.StopSFX();
    }
}