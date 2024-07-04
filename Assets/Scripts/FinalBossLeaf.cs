using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossLeaf : MonoBehaviour {
    [SerializeField] int attackPower = 3, maxHP = 100;
    [SerializeField] float speed = 5.2f, attackCooldown = 2f;
    [SerializeField] GameObject arrowPrefab, arrowRainPrefab, laserPrefab, coinPrefab, portal;
    Rigidbody2D rigidbody2d;
    GameObject player;
    Vector2 playerDirection;
    int currentHP, staggerCount = 0;
    bool escape, attacking = false, chase, staggering, rightWall, leftWall, onGround, start = false;
    float attackCooldownTimer;
    [SerializeField] private AudioClip Boss3DeadSound;
    [SerializeField] private AudioClip Boss3GotDamageSound;

    [SerializeField] private AudioClip ShootSound;
    [SerializeField] private AudioClip ArrowRainSound;

    [SerializeField] private AudioClip ArrowShortATKSound;

    [SerializeField] private AudioClip BowReadySound;

    [SerializeField] private AudioClip LaserSound;
    Animator animator;
    // Start is called before the first frame update
    void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        attackCooldownTimer = attackCooldown;
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update() {
        if (!start) {
            if (Vector2.Distance(transform.position, player.transform.position) <= 10) {
                start = true;
            }
            return;
        }
        playerDirection = (player.transform.position - transform.position).normalized;
        if (Vector2.Distance(transform.position, player.transform.position) <= 7) {
            escape = true;
        } else {
            escape = false;
        }
        if (Vector2.Distance(transform.position, player.transform.position) >= 9) {
            chase = true;
        } else {
            chase = false;
        }
        if (Vector2.Distance(transform.position, player.transform.position) <= 3) {
            if (!attacking) {
                animator.SetTrigger("Attack1");
                SetFaceDirection();
                attacking = true;
            }
        }
        if (!escape) {
            attackCooldownTimer -= Time.deltaTime;
        }
        if (!attacking && attackCooldownTimer <= 0) {
            attackCooldownTimer = attackCooldown;
            attacking = true;
            int randomInt = Random.Range(1, 6);
            if (randomInt == 3) {
                animator.SetTrigger("SpecialAttack");
            } else {
                int i = Random.Range(1, 4);
                if (i == 1) {
                    animator.SetTrigger("Attack3");
                } else {
                    animator.SetTrigger("Attack2");
                }
            }
            SetFaceDirection();
        }
        if (!attacking) {
            if (escape) {
                if (playerDirection.x >= 0) {
                    animator.SetFloat("FaceDirection", -1);
                } else {
                    animator.SetFloat("FaceDirection", 1);
                }
            } else {
                if (playerDirection.x >= 0) {
                    animator.SetFloat("FaceDirection", 1);
                } else {
                    animator.SetFloat("FaceDirection", -1);
                }
            }
        }
        if ((escape || chase) && !attacking) {
            animator.SetBool("Run", true);
        } else {
            animator.SetBool("Run", false);
        }
        RaycastHit2D RightWallDetect = Physics2D.Raycast(GetBottomRight(), Vector2.right, 0.05f, LayerMask.GetMask("Platform") + LayerMask.GetMask("Default"));
        if (RightWallDetect) {
            rightWall = true;
        }
        RaycastHit2D LeftWallDetect = Physics2D.Raycast(GetBottomLeft(), Vector2.left, 0.05f, LayerMask.GetMask("Platform") + LayerMask.GetMask("Default"));
        if (LeftWallDetect) {
            leftWall = true;
        }
        RaycastHit2D onGroundDetect = Physics2D.Raycast(GetBottomPoint(), Vector2.down, 0.05f, LayerMask.GetMask("Platform") + LayerMask.GetMask("Default"));
        if (onGroundDetect) {
            onGround = true;
        } else {
            onGround = false;
        }
    }

    private void FixedUpdate() {
        if (onGround) {
            if (!attacking) {
                if (escape) {
                    rigidbody2d.velocity = new Vector2(-playerDirection.x * speed, rigidbody2d.velocity.y);
                }
                if (chase) {
                    rigidbody2d.velocity = new Vector2(playerDirection.x * speed, rigidbody2d.velocity.y);
                }
            } else {
                rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
            }
            if (!escape && !chase) {
                rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
            }
        }
        if (rightWall) {
            rigidbody2d.AddForce(Vector2.up * 15f + Vector2.left * 30, ForceMode2D.Impulse);
            rightWall = false;
        }
        if (leftWall) {
            rigidbody2d.AddForce(Vector2.up * 15f + Vector2.right * 30, ForceMode2D.Impulse);
            leftWall = false;
        }
    }
    public void ChangeHP(int amount) {
        if (currentHP <= 0) {
            return;
        }
        currentHP += amount;
        if (currentHP <= 0) {
            animator.SetTrigger("Death");
            //SoundManager.instance.PlaySound(Boss3DeadSound);
            StartCoroutine(DropReward());
            rigidbody2d.gravityScale = 0;
            GetComponent<BoxCollider2D>().isTrigger = true;
            if (portal != null) {
                portal.SetActive(true);
            }
            return;
        }
        if (amount < 0) {
            staggerCount++;
        }
        if (staggerCount >= 5) {
            animator.SetTrigger("Hurt");
            //SoundManager.instance.PlaySound(Boss3GotDamageSound);
            staggerCount = 0;
            staggering = true;
        }
    }
    void SetFaceDirection() {
        if (playerDirection.x >= 0) {
            animator.SetFloat("FaceDirection", 1);
        } else {
            animator.SetFloat("FaceDirection", -1);
        }
    }
    void Attack1HitBox() {
        RaycastHit2D hit2D;
        if (animator.GetFloat("FaceDirection") >= 0) {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.right * 0.49f, 0.84f, Vector2.right, 0.38f, LayerMask.GetMask("Player"));
        } else {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.left * 0.49f, 0.84f, Vector2.left, 0.38f, LayerMask.GetMask("Player"));
        }
        if (hit2D) {
            PlayerController player = hit2D.collider.GetComponent<PlayerController>();
            if (player != null) {
                player.ChangeHP();
            }
        }
    }

    void Attack2Shoot() {
        GameObject arrowObject;
        if (animator.GetFloat("FaceDirection") >= 0) {
            arrowObject = Instantiate(arrowPrefab, rigidbody2d.position + new Vector2(1.16f, 0.25f), Quaternion.identity);
        } else {
            arrowObject = Instantiate(arrowPrefab, rigidbody2d.position + new Vector2(-1.16f, 0.35f), Quaternion.identity);
        }
        Arrow arrow = arrowObject.GetComponent<Arrow>();
        if (arrow != null) {
            arrow.Shoot(new Vector2(playerDirection.x, 0), 600);
            if (animator.GetFloat("FaceDirection") >= 0) {
                arrow.SetFaceDirection(1);
            } else {
                arrow.SetFaceDirection(-1);
            }
        }
    }
    void Attack3HitBox() {
        GameObject arrowRain = Instantiate(arrowRainPrefab, new Vector3(player.transform.position.x, GetBottomPoint().y, 0), Quaternion.identity);
        DamageZone damageZone = arrowRain.GetComponent<DamageZone>();
        if (damageZone != null) {
            damageZone.SetDamage(attackPower - 1);
        }
    }
    void attack3Combo() {
        int randomInt = Random.Range(1, 5);
        if (randomInt == 1) {
            animator.SetTrigger("SpecialAttack");
            SetFaceDirection();
        }
    }
    void SpecialAttackHitBox() {
        RaycastHit2D hit2D;
        if (animator.GetFloat("FaceDirection") >= 0) {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + new Vector2(1.1f, 0), 0.64f, Vector2.right, 18.53f, LayerMask.GetMask("Player"));
        } else {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + new Vector2(-1.1f, 0), 0.64f, Vector2.left, 18.53f, LayerMask.GetMask("Player"));
        }
        if (hit2D) {
            PlayerController player = hit2D.collider.GetComponent<PlayerController>();
            if (player != null) {
                player.ChangeHP();
            }
        }
    }

    void ResetAttack() {
        attacking = false;
    }
    private Vector2 GetBottomPoint() {
        Collider2D collider = rigidbody2d.GetComponent<Collider2D>();
        Vector2 colliderSize = collider.bounds.size;
        Vector2 bottomPoint = new Vector2(rigidbody2d.position.x, rigidbody2d.position.y - colliderSize.y * 0.5f);

        return bottomPoint;
    }
    void StopStagger() {
        staggering = false;
    }
    private Vector2 GetBottomRight() {
        Collider2D collider = rigidbody2d.GetComponent<Collider2D>();
        Vector2 colliderSize = collider.bounds.size;
        Vector2 bottomRight = new Vector2(rigidbody2d.position.x + colliderSize.x * 0.5f, rigidbody2d.position.y - colliderSize.y * 0.5f);

        return bottomRight;
    }
    private Vector2 GetBottomLeft() {
        Collider2D collider = rigidbody2d.GetComponent<Collider2D>();
        Vector2 colliderSize = collider.bounds.size;
        Vector2 bottomLeft = new Vector2(rigidbody2d.position.x - colliderSize.x * 0.5f, rigidbody2d.position.y - colliderSize.y * 0.5f);

        return bottomLeft;
    }
    IEnumerator DropReward() {
        int stageNumber = 1;
        if (PlayerPrefs.HasKey("stageNumber")) {
            stageNumber = PlayerPrefs.GetInt("stageNumber");
        }
        for (int i = 0; i < 5; i++) {
            GameObject drop = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            drop.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }

    private void Shoot() {

        //SoundManager.instance.PlaySound(ShootSound);
        return;
    }
    private void ArrowRain() {

        //SoundManager.instance.PlaySound(ArrowRainSound);
        return;
    }

    private void ArrowShortATK() {

        //SoundManager.instance.PlaySound(ArrowShortATKSound);
        return;
    }

    private void BowReady() {

        //SoundManager.instance.PlaySound(BowReadySound);
        return;
    }

    private void Laser() {

        //SoundManager.instance.PlaySound(LaserSound);
        return;
    }
}
