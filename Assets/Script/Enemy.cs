using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Enemy AI ����
public class Enemy : LivingThings
{
    public LayerMask whatIsTarget;

    private LivingThings targetEntity;
    private NavMeshAgent navMeshAgent;

    //public ParticleSystem hitEffect;
    public AudioClip deathSound;
    public AudioClip hitSound; 

    private Animator EnemyAnimator; 
    private AudioSource EnemyAudioPlayer;

    public float damage = 20f;
    public float timeBetAttack = 0.5f;
    private float lastAttackTime;

    // ������ ����� �����ϴ��� �˷��ִ� ������Ƽ
    private bool hasTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        EnemyAnimator = GetComponent<Animator>();
        EnemyAudioPlayer = GetComponent<AudioSource>();
    }

    // Enemy AI�� �ʱ� ������ �����ϴ� �¾� �޼���
    public void Setup(EnemyData enemyData)
    {
        startingHealth = enemyData.health;
        health = enemyData.health;

        damage = enemyData.damage;

        navMeshAgent.speed = enemyData.speed;
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        EnemyAnimator.SetBool("HasTarget", hasTarget);
    }

    // �ֱ������� ������ ����� ��ġ�� ã�� ��� ����
    private IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                navMeshAgent.isStopped = true;

                Collider[] colliders = Physics.OverlapSphere(transform.position, 50f, whatIsTarget);

                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingThings livingThings = colliders[i].GetComponent<LivingThings>();

                    if (livingThings != null && !livingThings.dead)
                    {
                        targetEntity = livingThings;

                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    // �������� �Ծ��� �� ������ ó��
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            //hitEffect.transform.position = hitPoint;
            //hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            //hitEffect.Play();

            EnemyAudioPlayer.PlayOneShot(hitSound);
            EnemyAnimator.SetTrigger("Hit");
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // ��� ó��
    public override void Die()
    {
        base.Die();

        Collider[] zombieColliders = GetComponents<Collider>();
        for (int i = 0; i < zombieColliders.Length; i++)
        {
            zombieColliders[i].enabled = false;
        }
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        EnemyAnimator.SetTrigger("Die");
        EnemyAudioPlayer.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingThings attackTarget = other.GetComponent<LivingThings>();

            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;

                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                EnemyAnimator.SetTrigger("Attack");

                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }
}