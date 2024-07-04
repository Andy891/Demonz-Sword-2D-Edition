using System.Collections;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    [SerializeField] bool isStun = false;
    [SerializeField] int maxHp = 100;
    [SerializeField] float speed = 5f, attackCooldown = 7, chargeTime = 2;
    [SerializeField] GameObject bulletPrefab, coinPrefab, portal;
    Vector2 originalPosition, chargeDirection;
    Rigidbody2D rigidbody2d;
    public int currentHP, attack1Count = 0;
    bool goLeft, goRight, goUp, goDown, attack1, attack2, chargeToPlayer, DropedReward;
    float attackTimer = 0, chargeTimer = 0, shootTimer = 0;
    Animator animator;
    GameObject healthBar;
    [SerializeField] private AudioClip Boss2DeadSound;
    [SerializeField] private AudioClip FireBallSound;

    [SerializeField] private AudioClip Boss2GotDamageSound;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        originalPosition = rigidbody2d.position;
        currentHP = maxHp;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector2.Distance(rigidbody2d.position, originalPosition) <= 1 || Vector2.Distance(rigidbody2d.position, originalPosition + Vector2.right * 43f) <= 1) && !attack1 && !attack2)
        {
            goLeft = false;
            goRight = false;
            if (rigidbody2d.position.x <= GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().position.x)
            {
                animator.SetFloat("FaceDirection", 1);
            }
            else
            {
                animator.SetFloat("FaceDirection", -1);
            }
            attackTimer += Time.deltaTime;
        }
        else if (!attack1 && !attack2)
        {
            attackTimer = 0;
            if ((Vector2.Distance(rigidbody2d.position, originalPosition) <= Vector2.Distance(rigidbody2d.position, originalPosition + Vector2.right * 43f) && rigidbody2d.position.x > originalPosition.x) || rigidbody2d.position.x > originalPosition.x + 43f)
            {
                goLeft = true;
            }
            else
            {
                goRight = true;
            }
        }
        if (attackTimer >= attackCooldown)
        {
            int randomInt = Random.Range(1, 3);
            //randomInt = 2;
            if (randomInt == 1)
            {
                attack1 = true;
                shootTimer = 0;
            }
            else
            {
                attack2 = true;
            }
            attackTimer = 0;
        }
        if (attack1)
        {
            RaycastHit2D RightWallDetect = Physics2D.Raycast(GetBottomRight(), Vector2.right, 0.05f, LayerMask.GetMask("Default"));
            RaycastHit2D LeftWallDetect = Physics2D.Raycast(GetBottomLeft(), Vector2.left, 0.05f, LayerMask.GetMask("Default"));
            if ((rigidbody2d.position.x < originalPosition.x || LeftWallDetect) && goLeft)
            {
                goLeft = false;
                goRight = true;
                attack1Count++;
            }
            else if ((rigidbody2d.position.x > originalPosition.x + 43f || RightWallDetect) && goRight)
            {
                goRight = false;
                goLeft = true;
                attack1Count++;
            }
            else if (!goLeft && !goRight)
            {
                goLeft = true;
            }
            if (shootTimer <= 0)
            {
                GameObject bulletObject = Instantiate(bulletPrefab, GetBottomPoint(), Quaternion.Euler(0, 0, 90));
                EnemyBullet bullet = bulletObject.GetComponent<EnemyBullet>();
                bullet.Shoot(Vector2.down, 300);
                SoundManager.instance.PlaySound(FireBallSound);

                shootTimer = 0.5f;
            }
            shootTimer -= Time.deltaTime;
            if (attack1Count > 2)
            {
                attack1 = false;
                goLeft = false;
                goRight = false;
                attack1Count = 0;
            }
        }
        if (attack2)
        {
            if (!chargeToPlayer)
            {
                chargeTimer += Time.deltaTime;
                animator.SetBool("Charge", true);
            }
            if (chargeTimer >= chargeTime && !chargeToPlayer)
            {
                Rigidbody2D playerPositionTemp = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
                chargeDirection = playerPositionTemp.position - rigidbody2d.position;
                chargeToPlayer = true;
                if (chargeDirection.x >= 0)
                {
                    animator.SetFloat("FaceDirection", 1);
                }
                else
                {
                    animator.SetFloat("FaceDirection", -1);
                }
                animator.SetBool("Charge", false);
                animator.SetBool("Attack", true);
            }
            RaycastHit2D RightWallDetect = Physics2D.Raycast(GetBottomRight(), Vector2.right, 0.05f, LayerMask.GetMask("Default"));
            RaycastHit2D LeftWallDetect = Physics2D.Raycast(GetBottomLeft(), Vector2.left, 0.05f, LayerMask.GetMask("Default"));
            RaycastHit2D onGroundDetect = Physics2D.Raycast(GetBottomPoint(), Vector2.down, 0.05f, LayerMask.GetMask("Default"));
            if (RightWallDetect || LeftWallDetect || onGroundDetect)
            {
                attack2 = false;
                chargeToPlayer = false;
                chargeTimer = 0;
                animator.SetBool("Attack", false);
                animator.SetTrigger("Stun");
            }
        }
        if (rigidbody2d.position.y < originalPosition.y - 0.5f)
        {
            goUp = true;
        }
        else
        {
            goUp = false;
        }
        if (rigidbody2d.position.y > originalPosition.y + 0.5f)
        {
            goDown = true;
        }
        else
        {
            goDown = false;
        }
        if (goLeft || goRight)
        {
            animator.SetBool("Flying", true);
            if (goLeft)
            {
                animator.SetFloat("FaceDirection", -1);
            }
            if (goRight)
            {
                animator.SetFloat("FaceDirection", 1);
            }
        }
        else
        {
            animator.SetBool("Flying", false);
        }
        if (DropedReward)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (currentHP <= 0)
        {

            return;
        }
        if (isStun)
        {
            rigidbody2d.velocity = Vector2.zero;
            return;
        }
        if (goLeft)
        {
            rigidbody2d.velocity = Vector2.left * speed;
        }
        else if (goRight)
        {
            rigidbody2d.velocity = Vector2.right * speed;
        }
        else
        {
            rigidbody2d.velocity = Vector2.zero;
        }
        if (chargeToPlayer)
        {
            rigidbody2d.velocity = chargeDirection.normalized * speed * 2;
        }
        else if (attack2)
        {
            rigidbody2d.velocity = Vector2.zero;
        }
        if (goUp && !attack2)
        {
            rigidbody2d.velocity = Vector2.up * speed;
        }
        if (goDown)
        {
            rigidbody2d.velocity = Vector2.down * speed;
        }
    }

    private void OnCollisionStay2D(UnityEngine.Collision2D other)
    {
        if (currentHP <= 0)
        {
            return;
        }
        PlayerController player = other.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHP();
        }
    }

    public void ChangeHP(int amount)
    {
        if (currentHP > 0)
        {
            currentHP += amount;
            Debug.Log("Boss HP " + currentHP + "/" + maxHp);
            animator.SetTrigger("Hurt");
            SoundManager.instance.PlaySound(Boss2GotDamageSound);

            if (currentHP <= 0)
            {
                animator.SetTrigger("Death");
                SoundManager.instance.PlaySound(Boss2DeadSound);
                BoxCollider2D boxCollider2d = GetComponent<BoxCollider2D>();
                boxCollider2d.isTrigger = true;
                rigidbody2d.velocity = Vector2.zero;
                rigidbody2d.gravityScale = 0;
                //StartCoroutine(DropReward());
            }
        }
    }
    private Vector2 GetBottomPoint()
    {
        Collider2D collider = rigidbody2d.GetComponent<Collider2D>();
        Vector2 colliderSize = collider.bounds.size;
        Vector2 bottomPoint = new Vector2(rigidbody2d.position.x, rigidbody2d.position.y - colliderSize.y * 0.5f);

        return bottomPoint;
    }
    private Vector2 GetBottomRight()
    {
        Collider2D collider = rigidbody2d.GetComponent<Collider2D>();
        Vector2 colliderSize = collider.bounds.size;
        Vector2 bottomRight = new Vector2(rigidbody2d.position.x + colliderSize.x * 0.5f, rigidbody2d.position.y - colliderSize.y * 0.5f);

        return bottomRight;
    }
    private Vector2 GetBottomLeft()
    {
        Collider2D collider = rigidbody2d.GetComponent<Collider2D>();
        Vector2 colliderSize = collider.bounds.size;
        Vector2 bottomLeft = new Vector2(rigidbody2d.position.x - colliderSize.x * 0.5f, rigidbody2d.position.y - colliderSize.y * 0.5f);

        return bottomLeft;
    }
    IEnumerator DropReward()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject drop = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            drop.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
        DropedReward = true;
    }
}