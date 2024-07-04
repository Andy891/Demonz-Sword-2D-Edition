using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyController : MonoBehaviour
{
    public float speed = 5f;
    public int maxHP = 10;
    public float sightPlayerRange = 4f;
    public GameObject enemyBulletPrefab;

    public GameObject deathEffect;
    [SerializeField] private AudioClip gotdamage;
    [SerializeField] private AudioClip DieSound;
    [SerializeField] private AudioClip ClawSound;
    [SerializeField] private AudioClip EnemyShootSound;
    [SerializeField] private AudioClip ShieldSound;

    Rigidbody2D rigidbody2d;
    float faceDirection;
    float currentSpeed;
    int currentHP;
    bool onGround;
    GameObject player;
    bool sightPlayer = false;
    Animator animator;
    PlayerController playerController;
    [SerializeField] GameObject coinPrefab;
    EnemyShieldUp enemyShieldUp;
    [HideInInspector] public int weakPointCount;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentSpeed = speed;
        faceDirection = 1f;
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        animator.SetFloat("Direction", faceDirection);
        playerController = player.GetComponent<PlayerController>();
        BoxCollider2D[] weakPointsArray = GetComponentsInChildren<BoxCollider2D>();
        weakPointCount = CountWeakPoints();
        enemyShieldUp = GetComponent<EnemyShieldUp>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D RightEdgeDetect = Physics2D.Raycast(GetBottomRight(), Vector2.down, 0.05f, LayerMask.GetMask("Platform") + LayerMask.GetMask("Default"));
        RaycastHit2D LeftEdgeDetect = Physics2D.Raycast(GetBottomLeft(), Vector2.down, 0.05f, LayerMask.GetMask("Platform") + LayerMask.GetMask("Default"));
        RaycastHit2D RightWallDetect = Physics2D.Raycast(GetBottomRight(), Vector2.right, 0.05f, LayerMask.GetMask("Platform") + LayerMask.GetMask("Default"));
        RaycastHit2D LeftWallDetect = Physics2D.Raycast(GetBottomLeft(), Vector2.left, 0.05f, LayerMask.GetMask("Platform") + LayerMask.GetMask("Default"));
        RaycastHit2D onGroundDetect = Physics2D.Raycast(GetBottomPoint(), Vector2.down, 0.05f, LayerMask.GetMask("Platform") + LayerMask.GetMask("Default"));
        if (onGroundDetect)
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
        if (!sightPlayer)
        {
            if (faceDirection > 0 && (!RightEdgeDetect || RightWallDetect) && onGround)
            {
                currentSpeed = -speed;
                faceDirection = -1f;
                animator.SetFloat("Direction", faceDirection);
            }
            if (faceDirection < 0 && (!LeftEdgeDetect || LeftWallDetect) && onGround)
            {
                currentSpeed = speed;
                faceDirection = 1f;
                animator.SetFloat("Direction", faceDirection);
            }
        }
        if (Vector2.Distance(transform.position, player.transform.position) < sightPlayerRange)
        {
            sightPlayer = true;
            animator.SetBool("Attacking", true);
            if (player.transform.position.x < transform.position.x)
            {
                faceDirection = -1;
                currentSpeed = -speed;
                animator.SetFloat("Direction", faceDirection);
            }
            if (player.transform.position.x > transform.position.x)
            {
                faceDirection = 1;
                currentSpeed = speed;
                animator.SetFloat("Direction", faceDirection);
            }
        }
        else
        {
            sightPlayer = false;
            animator.SetBool("Attacking", false);
        }
        int temp = weakPointCount;
        weakPointCount = CountWeakPoints();
        if (weakPointCount != temp)
        {
            if (weakPointCount <= 0 && enemyShieldUp != null)
            {
                enemyShieldUp.ShieldDown();
            }
            ChangeHP();
        }
    }

    void FixedUpdate()
    {
        if (enemyShieldUp != null)
        {
            if (enemyShieldUp.shieldUp)
            {
                rigidbody2d.velocity = Vector2.zero;
                return;
            }
        }
        if (onGround && !sightPlayer)
        {
            rigidbody2d.velocity = new Vector2(currentSpeed, rigidbody2d.velocity.y);
        }
        else if (onGround)
        {
            rigidbody2d.velocity = Vector2.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        PlayerController player = other.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHP();
        }
    }


    public void ChangeHP()
    {
        weakPointCount = CountWeakPoints();
        if (weakPointCount <= 0)
        {
            animator.SetTrigger("Die");
            SoundManager.instance.PlaySound(DieSound);

            Instantiate(deathEffect, transform.position, Quaternion.identity);



            rigidbody2d.gravityScale = 0;
            BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = true;
            rigidbody2d.velocity = Vector2.zero;
            //StartCoroutine(DropReward());
            this.enabled = false;
        }
        else
        {
            animator.SetTrigger("Hit");

            //SoundManager.instance.PlaySound(gotdamage);
            if (player.transform.position.x < rigidbody2d.position.x)
            {
                faceDirection = -1;
                currentSpeed = -speed;
            }
            else
            {
                faceDirection = 1;
                currentSpeed = speed;
            }
            animator.SetFloat("Direction", faceDirection);
        }
    }

    public void KnockBack(float direction, float force)
    {
        if (weakPointCount > 0)
        {
            rigidbody2d.AddForce(Vector2.up * 250);
            if (direction == 1)
            {
                rigidbody2d.AddForce(Vector2.right * force);
            }
            else if (direction == -1)
            {
                rigidbody2d.AddForce(Vector2.left * force);
            }
        }
    }

    public void ShootBullt()
    {
        Vector2 direction = player.transform.position - transform.position;
        GameObject bulletObject = Instantiate(enemyBulletPrefab, rigidbody2d.position, Quaternion.identity);
        EnemyBullet bullet = bulletObject.GetComponent<EnemyBullet>();
        bullet.Shoot(direction, 100);
    }
    void MeleeAttack()
    {
        RaycastHit2D meleeHit = default;
        if (faceDirection == 1)
        {
            meleeHit = Physics2D.CircleCast(GetRight(), 1.12f, Vector2.right, 1.53f, LayerMask.GetMask("Player"));
        }
        else if (faceDirection == -1)
        {
            meleeHit = Physics2D.CircleCast(GetLeft(), 1.12f, Vector2.left, 1.53f, LayerMask.GetMask("Player"));
        }
        if (meleeHit)
        {
            PlayerController player = meleeHit.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ChangeHP();
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
    private Vector2 GetRight()
    {
        Collider2D collider = rigidbody2d.GetComponent<Collider2D>();
        Vector2 colliderSize = collider.bounds.size;
        Vector2 right = new Vector2(rigidbody2d.position.x + colliderSize.x * 0.5f, rigidbody2d.position.y);

        return right;
    }

    private Vector2 GetLeft()
    {
        Collider2D collider = rigidbody2d.GetComponent<Collider2D>();
        Vector2 colliderSize = collider.bounds.size;
        Vector2 left = new Vector2(rigidbody2d.position.x - colliderSize.x * 0.5f, rigidbody2d.position.y);

        return left;
    }

    private void Claw()
    {

        SoundManager.instance.PlaySound(ClawSound);
        return;
    }

    private void EnemyShoot()
    {

        SoundManager.instance.PlaySound(EnemyShootSound);
        return;
    }

    private void Shield()
    {

        SoundManager.instance.PlaySound(ShieldSound);
        return;
    }
    IEnumerator DropReward()
    {
        int stageNumber = 1;
        if (PlayerPrefs.HasKey("stageNumber"))
        {
            stageNumber = PlayerPrefs.GetInt("stageNumber");
        }
        for (int i = 0; i < 5; i++)
        {
            GameObject drop = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            drop.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.1f);
        }
    }

    int CountWeakPoints()
    {
        int count = 0;

        // Iterate through the child objects
        for (int i = 0; i < transform.childCount; i++)
        {
            // Assuming the weak point has a specific component attached (e.g., WeakPointComponent)
            WeakPoint weakPoint = transform.GetChild(i).GetComponent<WeakPoint>();

            // If the child object has the weak point component, increment the count
            if (weakPoint != null)
            {
                count++;
            }
        }
        return count;
    }
}
