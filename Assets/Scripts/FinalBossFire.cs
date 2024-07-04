using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossFire : MonoBehaviour {
    [SerializeField] int maxHP = 200;
    [SerializeField] float speed = 5;

    [SerializeField] private AudioClip Boss5GotDamageSound;

    [SerializeField] private AudioClip Boss5DeadSound;

    [SerializeField] private AudioClip SwordSkillSound;

    [SerializeField] private AudioClip firewaitSound;

    [SerializeField] private AudioClip fireSwordSound;

    [SerializeField] private AudioClip SwordSound;

    Rigidbody2D rigidbody2d;
    Animator animator;
    GameObject player;
    Vector2 playerDirection;
    bool playerInSight, attack, spAttack, staggering, start = false;
    int currentHP, staggerCount = 0;
    float randomSpAttackTimer;

    // Start is called before the first frame update
    void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (Vector2.Distance(player.transform.position, transform.position) < 3) {
            playerInSight = true;
            if (!attack && !spAttack) {
                int randomInt = Random.Range(1, 6);
                if (randomInt == 1) {
                    animator.SetTrigger("SpecialAttack");
                    spAttack = true;
                    rigidbody2d.velocity = Vector2.zero;
                } else {
                    animator.SetTrigger("Attack");
                    attack = true;
                    rigidbody2d.velocity = Vector2.zero;
                }
            }
        } else {
            playerInSight = false;
            randomSpAttackTimer -= Time.deltaTime;
            if (randomSpAttackTimer <= 0) {
                if (!attack && !spAttack && Vector2.Distance(player.transform.position, transform.position) < 8) {
                    int randomInt = Random.Range(1, 5);
                    if (randomInt == 1) {
                        animator.SetTrigger("SpecialAttack");
                        spAttack = true;
                        rigidbody2d.velocity = Vector2.zero;
                    }
                }
                randomSpAttackTimer = 1;
            }
        }
        if (playerInSight || attack || spAttack) {
            animator.SetBool("Run", false);
        } else {
            animator.SetBool("Run", true);
            if (playerDirection.x >= 0) {
                animator.SetFloat("FaceDirection", 1);
            } else {
                animator.SetFloat("FaceDirection", -1);
            }
        }
    }
    private void FixedUpdate() {
        if (!spAttack && !attack) {
            if (!playerInSight && !staggering) {
                rigidbody2d.velocity = new Vector2(playerDirection.x * speed, rigidbody2d.velocity.y);
            } else {
                rigidbody2d.velocity = Vector2.zero;
            }
        }
    }

    public void ChangeHP(int amount) {
        if (currentHP <= 0) {
            return;
        }
        currentHP += amount;
        if (currentHP <= 0) {
            animator.SetTrigger("Death");
            //SoundManager.instance.PlaySound(Boss5DeadSound);
            return;
        }
        if (amount < 0) {
            staggerCount++;
        }
        if (staggerCount >= 5) {
            animator.SetTrigger("Hurt");
            //SoundManager.instance.PlaySound(Boss5GotDamageSound);

            staggerCount = 0;
            staggering = true;
        }
    }

    public void StopStagger() {
        staggering = false;
    }
    public void Attack1HitBox() {
        RaycastHit2D hit2D;
        if (animator.GetFloat("FaceDirection") >= 0) {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.right * 0.64f, 1.37f, Vector2.right, 1.44f, LayerMask.GetMask("Player"));
        } else {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.left * 0.64f, 1.37f, Vector2.left, 1.44f, LayerMask.GetMask("Player"));
        }
        if (hit2D) {
            PlayerController player = hit2D.collider.GetComponent<PlayerController>();
            if (player != null) {
                player.ChangeHP();
            }
        }
    }
    public void Attack1CheckInSight() {
        if (Vector2.Distance(player.transform.position, transform.position) < 4.5) {
            if (playerDirection.x >= 0) {
                animator.SetFloat("FaceDirection", 1);
            } else {
                animator.SetFloat("FaceDirection", -1);
            }
            animator.SetTrigger("Attack");
        }
    }
    public void Attack2Move() {
        if (playerDirection.x >= 0) {
            rigidbody2d.velocity = new Vector2(8, rigidbody2d.velocity.y);
            animator.SetFloat("FaceDirection", 1);
        } else {
            rigidbody2d.velocity = new Vector2(-8, rigidbody2d.velocity.y);
            animator.SetFloat("FaceDirection", -1);
        }
    }
    public void Attack2HitBox1() {
        RaycastHit2D hit2D;
        if (animator.GetFloat("FaceDirection") >= 0) {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.left * 0.65f, 1.35f, Vector2.left, 1.82f, LayerMask.GetMask("Player"));
        } else {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.right * 0.65f, 1.35f, Vector2.right, 1.82f, LayerMask.GetMask("Player"));
        }
        if (hit2D) {
            PlayerController player = hit2D.collider.GetComponent<PlayerController>();
            if (player != null) {
                player.ChangeHP();
            }
        }
    }
    public void Attack2HitBox2() {
        RaycastHit2D hit2D;
        if (animator.GetFloat("FaceDirection") >= 0) {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.right * 0.65f, 1.35f, Vector2.right, 1.7f, LayerMask.GetMask("Player"));
        } else {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.left * 0.65f, 1.35f, Vector2.left, 1.7f, LayerMask.GetMask("Player"));
        }
        if (hit2D) {
            PlayerController player = hit2D.collider.GetComponent<PlayerController>();
            if (player != null) {
                player.ChangeHP();
            }
        }
    }
    public void Attack2HitBox3() {
        RaycastHit2D hit2D;
        if (animator.GetFloat("FaceDirection") >= 0) {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.left * 0.65f, 1.35f, Vector2.left, 2.18f, LayerMask.GetMask("Player"));
        } else {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.right * 0.65f, 1.35f, Vector2.right, 2.18f, LayerMask.GetMask("Player"));
        }
        if (hit2D) {
            PlayerController player = hit2D.collider.GetComponent<PlayerController>();
            if (player != null) {
                player.ChangeHP();
            }
        }
    }
    public void Attack2HitBox4() {
        RaycastHit2D hit2D;
        if (animator.GetFloat("FaceDirection") >= 0) {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.right * 0.65f, 1.35f, Vector2.right, 2.16f, LayerMask.GetMask("Player"));
        } else {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.left * 0.65f, 1.35f, Vector2.left, 2.16f, LayerMask.GetMask("Player"));
        }
        if (hit2D) {
            PlayerController player = hit2D.collider.GetComponent<PlayerController>();
            if (player != null) {
                player.ChangeHP();
            }
        }
    }
    public void Attack2CheckInSight() {
        if (playerDirection.x >= 0) {
            animator.SetFloat("FaceDirection", 1);
        } else {
            animator.SetFloat("FaceDirection", -1);
        }
        if (Vector2.Distance(player.transform.position, transform.position) < 5) {
            int randomInt = Random.Range(1, 5);
            if (randomInt == 1) {
                animator.SetTrigger("SpecialAttack");
                spAttack = true;
                attack = false;
            } else {
                animator.SetTrigger("Attack");
            }
        }
    }
    public void Attack3HitBox() {
        RaycastHit2D hit2D;
        if (animator.GetFloat("FaceDirection") >= 0) {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.right * 0.65f, 1.35f, Vector2.right, 3.74f, LayerMask.GetMask("Player"));
        } else {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.left * 0.65f, 1.35f, Vector2.left, 3.74f, LayerMask.GetMask("Player"));
        }
        if (hit2D) {
            PlayerController player = hit2D.collider.GetComponent<PlayerController>();
            if (player != null) {
                player.ChangeHP();
            }
        }
    }
    public void SpecialAttackDash() {
        if (playerDirection.x >= 0) {
            rigidbody2d.velocity = new Vector2(30, rigidbody2d.velocity.y);
            animator.SetFloat("FaceDirection", 1);
        } else {
            rigidbody2d.velocity = new Vector2(-30, rigidbody2d.velocity.y);
            animator.SetFloat("FaceDirection", -1);
        }
    }
    public void SpecialAttackStopDash() {
        rigidbody2d.velocity = Vector2.zero;
    }
    public void SpecialAttackHitBox() {
        RaycastHit2D hit2D;
        if (animator.GetFloat("FaceDirection") >= 0) {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.right * 0.65f, 1.35f, Vector2.right, 2.84f, LayerMask.GetMask("Player"));
        } else {
            hit2D = Physics2D.CircleCast(rigidbody2d.position + Vector2.left * 0.65f, 1.35f, Vector2.left, 2.84f, LayerMask.GetMask("Player"));
        }
        if (hit2D) {
            PlayerController player = hit2D.collider.GetComponent<PlayerController>();
            if (player != null) {
                player.ChangeHP();
            }
        }
    }

    public void ResetAttack() {
        attack = false;
    }

    public void ResetSpAttack() {
        spAttack = false;
    }



    private void SwordSkill() {

        //SoundManager.instance.PlaySound(SwordSkillSound);
        return;
    }
    private void firewait() {

        //SoundManager.instance.PlaySound(firewaitSound);
        return;
    }

    private void fireSword() {

        //SoundManager.instance.PlaySound(fireSwordSound);
        return;
    }

    private void Sword() {

        //SoundManager.instance.PlaySound(SwordSound);
        return;
    }
    void DestroyGameObject() {
        Destroy(gameObject);
    }
}
