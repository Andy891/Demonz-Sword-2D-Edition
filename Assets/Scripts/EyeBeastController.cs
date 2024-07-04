using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBeastController : MonoBehaviour
{
    [SerializeField] float speed = 3, attackRange = 4, sightRange = 7, attackCooldown = 1f, projectileSpeed = 150;
    [SerializeField] GameObject projectilePrefab, deathEffect;
    [SerializeField] private AudioClip fireballSound;
    [SerializeField] private AudioClip DeadSound;


    Transform player;
    Animator animator;
    bool isFlipped, isDead = false;
    float attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }
        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
        if (Vector2.Distance(transform.position, player.position) < sightRange)
        {
            animator.SetBool("PlayerInSight", true);
        }
        else
        {
            animator.SetBool("PlayerInSight", false);
        }
        if (animator.GetBool("PlayerInSight") && attackTimer <= 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        if (attackTimer >= 0)
        {
            attackTimer -= Time.deltaTime;
        }
        if (Vector2.Distance(transform.position, player.position) <= attackRange && attackTimer <= 0)
        {
            animator.SetTrigger("Attack");
            attackTimer = attackCooldown;
        }
        WeakPoint weakPoint = GetComponentInChildren<WeakPoint>();
        if (weakPoint == null)
        {
            animator.SetTrigger("Death");
            isDead = true;
            SoundManager.instance.PlaySound(DeadSound);

            BoxCollider2D boxCollider2d = GetComponent<BoxCollider2D>();
            boxCollider2d.enabled = false;
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
    }

    void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        EnemyBullet enemyBullet = projectile.GetComponent<EnemyBullet>();
        enemyBullet.Shoot(player.transform.position - transform.position, projectileSpeed);
        SoundManager.instance.PlaySound(fireballSound);

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
            playerController.ChangeHP();
    }
}
