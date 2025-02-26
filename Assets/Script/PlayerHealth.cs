using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingThings
{
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더

    //public AudioClip deathClip; // 사망 소리
    //public AudioClip hitClip; // 피격 소리
    //public AudioClip itemPickupClip; // 아이템 습득 소리

    //private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    public ParticleSystem Life_P;
    public ParticleSystem Bullet_P;
    public ParticleSystem Coin_P;

    private PlayerMove playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

    private void Awake()
    {
        // 사용할 컴포넌트를 가져오기
        playerAnimator = GetComponent<Animator>();
        //playerAudioPlayer = GetComponent<AudioSource>();

        playerMovement = GetComponent<PlayerMove>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        // 체력 슬라이더 활성화
        healthSlider.gameObject.SetActive(true);
        // 체력 슬라이더의 최댓값을 기본 체력값으로 변경
        healthSlider.maxValue = startingHealth;
        // 체력 슬라이더의 값을 현재 체력값으로 변경
        healthSlider.value = health;

        // 플레이어 조작을 받는 컴포넌트 활성화
        playerMovement.enabled = true;
        playerShooter.enabled = true;

        Life_OffEffect();
        Bullet_OffEffect();
        Coin_OffEffect();
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);
        // 갱신된 체력으로 체력 슬라이더 갱신
        healthSlider.value = health;
    }

    // 데미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (!dead)
        {
            // 사망하지 않은 경우에만 효과음 재생
            SoundManager.instance.PlaySFX("Hit");
            //playerAudioPlayer.PlayOneShot(hitClip);
        }

        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage, hitPoint, hitDirection);
        // 갱신된 체력을 체력 슬라이더에 반영
        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die()
    {
        Debug.Log("Die");
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        // 체력 슬라이더 비활성화
        healthSlider.gameObject.SetActive(false);

        // 애니메이터의 Die 트리거를 발동시켜 사망 애니메이션 재생
        playerAnimator.SetTrigger("Die");

        // 플레이어의 조작을 받는 컴포넌트 비활성화
        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        // 사망하지 않은 경우에만 아이템 사용 가능
        if (!dead)
        {
            // 충돌한 상대방으로부터 IItem 컴포넌트 가져오기 시도
            IItem item = other.GetComponent<IItem>();

            // 충돌한 상대방으로부터 IItem 컴포넌트를 가져오는 데 성공했다면
            if (item != null)
            {
                // Use 메서드를 실행하여 아이템 사용
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