using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Boss4 : MonoBehaviour {
    public int maxHP = 100;
    [HideInInspector]
    public bool isClone = false;
    [SerializeField]
    float speed = 4f, dashSpeed = 15f, dashTime = 0.2f, dashCooldown = 1f, cloneCooldown = 10f;
    [SerializeField]
    GameObject bulletPrefab, coinPrefab, portal;
    public int currentHP;
    float currentSpeed, shootCooldownTimer, dashTimer = 0f, dashDirection, faceDirection, dashCooldownTimer, cloneCooldownTimer, hitAnimationCooldownTimer;
    bool inRangeAttackRange, inMeleeRange, i = false, start;

    [SerializeField] private AudioClip Boss4DeadSound;
    [SerializeField] private AudioClip Boss4GotDamageSound;

    [SerializeField] private AudioClip ShootSound;
    [SerializeField] private AudioClip ArrowRainSound;

    [SerializeField] private AudioClip ArrowShortATKSound;

    [SerializeField] private AudioClip BowReadySound;

    [SerializeField] private AudioClip LaserSound;

    Rigidbody2D rigidbody2d;
    Transform playerTransform;
    Animator animator;
    GameObject healthBar;

    // Start is called before the first frame update
    void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        currentSpeed = speed;
        currentHP = maxHP;
        animator = GetComponent<Animator>();
        faceDirection = 1;
        cloneCooldownTimer = cloneCooldown;
        start = false;
    }

    // Update is called once per frame
    void Update() {
        if (Vector3.Distance(transform.position, playerTransform.position) < 10 && !start) {
            start = true;
            healthBar.SetActive(true);
        }
        if (!start) {
            return;
        }
        if (playerTransform.position.x > rigidbody2d.position.x) {
            faceDirection = 1;
        } else {
            faceDirection = -1;
        }
        if (Vector2.Distance(rigidbody2d.position, playerTransform.position) < 5f) {
            currentSpeed = -speed;
            inRangeAttackRange = false;
            animator.SetFloat("FaceDirection", -faceDirection);
            animator.SetBool("Walk", true);
        } else if (Vector2.Distance(rigidbody2d.position, playerTransform.position) > 8f) {
            currentSpeed = speed;
            inRangeAttackRange = false;
            animator.SetFloat("FaceDirection", faceDirection);
            animator.SetBool("Walk", true);
        } else {
            currentSpeed = 0f;
            inRangeAttackRange = true;
            animator.SetFloat("FaceDirection", faceDirection);
            animator.SetBool("Walk", false);
        }
        if (Vector2.Distance(rigidbody2d.position, playerTransform.position) < 3f) {
            inMeleeRange = true;
            animator.SetBool("Walk", false);
            animator.SetFloat("FaceDirection", faceDirection);
            animator.SetBool("MeleeAttack", true);
        } else {
            inMeleeRange = false;
            animator.SetBool("MeleeAttack", false);
        }
        if (inRangeAttackRange) {
            animator.SetBool("RangeAttack", true);
        } else {
            animator.SetBool("RangeAttack", false);
        }
        RaycastHit2D RightWallDetect = Physics2D.Raycast(GetBottomRight() - Vector2.right * 0.1f, Vector2.right, 0.2f, LayerMask.GetMask("Platform") + LayerMask.GetMask("Default"));
        RaycastHit2D LeftWallDetect = Physics2D.Raycast(GetBottomLeft() - Vector2.left * 0.1f, Vector2.left, 0.2f, LayerMask.GetMask("Platform") + LayerMask.GetMask("Default"));
        if ((RightWallDetect || LeftWallDetect) && !inRangeAttackRange && !inMeleeRange) {
            dashTimer = dashTime;
            if (playerTransform.position.x > rigidbody2d.position.x) {
                dashDirection = 1f;
            } else {
                dashDirection = -1f;
            }
        } else if (dashTimer < 0) {
            if (playerTransform.position.x > rigidbody2d.position.x) {
                dashDirection = 1f;
            } else {
                dashDirection = -1f;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0) {
            dashCooldownTimer = dashCooldown;
            dashTimer = dashTime;
        }
        if (dashTimer >= 0) {
            Dash();
        }
        dashTimer -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        shootCooldownTimer -= Time.deltaTime;
        if (currentHP <= 50 && cloneCooldownTimer <= 0) {
            GameObject clone = Instantiate(gameObject, rigidbody2d.position, Quaternion.identity);
            clone.GetComponent<Boss4>().maxHP = 5;
            clone.GetComponent<Boss4>().isClone = true;
            GameObject[] allBoss = GameObject.FindGameObjectsWithTag("Boss");
            int cloneCount = 0;
            foreach (GameObject boss in allBoss) {
                if (boss.GetComponent<Boss4>().isClone == true) {
                    cloneCount++;
                }
            }
            for (int i = 0; i < cloneCount; i++) {
                //clone.GetComponentInChildren<EnemyHealthBar>().gameObject.GetComponent<RectTransform>().position += Vector3.up * 76f;
            }
            Color tmp = clone.GetComponent<SpriteRenderer>().color;
            tmp.a = 0.7f;
            clone.GetComponent<SpriteRenderer>().color = tmp;
            cloneCooldownTimer = cloneCooldown;
        }
        cloneCooldownTimer -= Time.deltaTime;
        hitAnimationCooldownTimer -= Time.deltaTime;
    }

    private void FixedUpdate() {
        if (!start) {
            return;
        }
        if (currentHP <= 0) {
            rigidbody2d.velocity = Vector2.zero;
            return;
        }
        if (!inMeleeRange && dashTimer < 0) {
            rigidbody2d.position = Vector2.MoveTowards(rigidbody2d.position, playerTransform.position, currentSpeed * Time.fixedDeltaTime);
        }
        if (dashTimer > 0) {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Boss"), LayerMask.NameToLayer("Player"), true);
            Dash();
            i = true;
        } else if (i == true) {
            i = false;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Boss"), LayerMask.NameToLayer("Player"), false);
        } else {
            rigidbody2d.velocity = new Vector2(0f, rigidbody2d.velocity.y);
        }
    }

    void RangeAttack() {
        if (shootCooldownTimer < 0) {
            Vector2 startPoint = default;
            if (faceDirection > 0) {
                startPoint = new Vector2(1, 1);
            } else {
                startPoint = new Vector2(-1, 1);
            }
            Vector2 direction = new Vector2(playerTransform.position.x, playerTransform.position.y) - (rigidbody2d.position + startPoint);
            GameObject bulletObject = Instantiate(bulletPrefab, rigidbody2d.position + startPoint, Quaternion.identity);
            EnemyBullet bullet = bulletObject.GetComponent<EnemyBullet>();
            bullet.Shoot(direction, 200);
            shootCooldownTimer = 0.5f;
        }
    }
    void MeleeAttack() {
        RaycastHit2D meleeHit = default;
        if (faceDirection == 1) {
            meleeHit = Physics2D.CircleCast(GetRight(), 1.21f, Vector2.right, 2.5f, LayerMask.GetMask("Player"));
        } else if (faceDirection == -1) {
            meleeHit = Physics2D.CircleCast(GetLeft(), 1.21f, Vector2.left, 2.5f, LayerMask.GetMask("Player"));
        }
        if (meleeHit) {
            PlayerController player = meleeHit.collider.GetComponent<PlayerController>();
            if (player != null) {
                player.ChangeHP();
            }
        }
    }

    void Dash() {
        if (dashDirection > 0) {
            rigidbody2d.velocity = new Vector2(dashSpeed, 0);
        } else {
            rigidbody2d.velocity = new Vector2(-dashSpeed, 0);
        }
    }

    public void ChangeHP(int amount) {
        if (!start) {
            return;
        }
        if (currentHP > 0) {
            currentHP += amount;
            //SoundManager.instance.PlaySound(Boss4GotDamageSound);
            Debug.Log("Boss HP " + currentHP + "/" + maxHP);
            if (hitAnimationCooldownTimer <= 0) {
                animator.SetTrigger("Hit");
                //SoundManager.instance.PlaySound(Boss4GotDamageSound);


                hitAnimationCooldownTimer = 3f;
            }
            if (dashCooldownTimer <= 0) {
                dashTimer = dashTime;
                dashCooldownTimer = dashCooldown;
            }
            if (currentHP <= 0) {
                animator.SetTrigger("Death");
                //SoundManager.instance.PlaySound(Boss4DeadSound);
                BoxCollider2D boxCollider2d = GetComponent<BoxCollider2D>();
                boxCollider2d.isTrigger = true;
                rigidbody2d.velocity = Vector2.zero;
                rigidbody2d.gravityScale = 0;
                if (!isClone) {
                    StartCoroutine(DropReward());
                    if (portal != null) {
                        portal.SetActive(true);
                    }
                }
                if (isClone) {
                    ClearBody();
                }
            }
        }
    }

    public void ClearBody() {
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(UnityEngine.Collision2D other) {
        if (currentHP <= 0) {
            return;
        }
        PlayerController player = other.collider.GetComponent<PlayerController>();
        if (player != null) {
            player.ChangeHP();
        }
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
    private Vector2 GetRight() {
        Collider2D collider = rigidbody2d.GetComponent<Collider2D>();
        Vector2 colliderSize = collider.bounds.size;
        Vector2 right = new Vector2(rigidbody2d.position.x + colliderSize.x * 0.5f, rigidbody2d.position.y);

        return right;
    }

    private Vector2 GetLeft() {
        Collider2D collider = rigidbody2d.GetComponent<Collider2D>();
        Vector2 colliderSize = collider.bounds.size;
        Vector2 left = new Vector2(rigidbody2d.position.x - colliderSize.x * 0.5f, rigidbody2d.position.y);

        return left;
    }

    IEnumerator DropReward() {
        for (int i = 0; i < 5; i++) {
            GameObject drop = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            drop.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }

    private void BossShoot() {

        //SoundManager.instance.PlaySound(ShootSound);
        return;
    }
}
