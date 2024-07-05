using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {
    public int lifespan = 0;
    public int Sethp = 4;
    public float speed = 5.0f;
    public float jumpHeight = 30.0f;
    public float dashSpeed = 15f;
    public float dashTime = 0.3f;
    public float dashCooldown = 1.0f;
    public bool onGround;
    public GameObject UpRCollider;
    private UIManager uiManager;

    public GameObject UpLCollider;
    public GameObject MidRCollider;
    public GameObject MidLCollider;
    public GameObject DownRCollider;
    public GameObject DownLCollider;
    Vector2 test = new Vector2(3.4f, 1.2f);
    private BoxCollider2D playerCollider;
    float horizontal;
    float vertical;
    float faceDirection = 1f;
    float dashTimer;
    float dashCooldownTimer;
    float dashDirection;
    float shaking = 0.5f;
    float stuntime = 1f;
    int hp = 0;
    bool jump;
    bool dash;
    bool dashing;
    bool jumpOff;
    bool nodash;
    GameObject SafeCollection;
    int FirstTimeFaceDirectionInitialize;

    UIHealthBar healthBar;

    Animator animator;
    Rigidbody2D rigidbody2d;
    BoxCollider2D boxCollider2d;
    SpriteRenderer PlayerSprite;
    [SerializeField] private AudioClip UseSkillSound;

    [SerializeField] private AudioClip slashSound;
    [SerializeField] private AudioClip JumpSound;
    [SerializeField] private AudioClip DashSound;
    [SerializeField] private AudioClip WalkSound1;
    [SerializeField] private AudioClip changeSkillSound;


    [SerializeField] private AudioClip WalkSound2;

    [SerializeField] private AudioClip GotDamageSound;
    [SerializeField] private AudioClip DieSound;
    [SerializeField] private AudioClip getskillSound;

    // Start is called before the first frame update

    public bool eyeSkill = false, legSkill = false, handSkill = false;

    /*
    0 = no skill
    1 = eye skill
    2 = leg skill
    3 = hand skill
    */
    int currentSkill = 0, jumpCount = 0;
    [HideInInspector] public bool handSkillActive = false;
    bool eyeSkillActive = false, legSkillActive = false;
    [SerializeField] float eyeSkillActiveTime = 30f, eyeSkillCooldown = 15f, legSkillActiveTime = 30f, legSkillCooldown = 15f, handSkillActiveTime = 30f, handSkillCooldown = 15f;
    float eyeSkillActiveTimer, eyeSkillCooldownTimer, legSkillActiveTimer, legSkillCooldownTimer, handSkillActiveTimer, handSkillCooldownTimer;
    IconController iconController;
    UnityEngine.UI.Image iconMask;
    GameObject nightVisionTile;
    void Start() {
        SafeCollection = GameObject.Find("SafePointCollection");
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        PlayerSprite = GetComponent<SpriteRenderer>();
        hp = Sethp;
        uiManager = FindObjectOfType<UIManager>();

        FirstTimeFaceDirectionInitialize = 1;
        playerCollider = GetComponent<BoxCollider2D>();
        healthBar = FindObjectOfType<UIHealthBar>();
        iconController = FindObjectOfType<IconController>();
        iconMask = iconController.iconMask;
        nightVisionTile = GameObject.FindGameObjectWithTag("NightVisionTile");
    }

    // Update is called once per frame
    void Update() {
        //Activate Skills
        Light2D[] lights = FindObjectsOfType<Light2D>();
        if (Input.GetKey(KeyCode.LeftControl)) {

            if (currentSkill == 1 && eyeSkillCooldownTimer <= 0) {
                eyeSkillActive = true;
                SoundManager.instance.PlaySound(UseSkillSound);

                eyeSkillActiveTimer = eyeSkillActiveTime;
                eyeSkillCooldownTimer = eyeSkillCooldown;
            }
            if (currentSkill == 2 && legSkillCooldownTimer <= 0) {
                legSkillActive = true;
                SoundManager.instance.PlaySound(UseSkillSound);

                legSkillActiveTimer = legSkillActiveTime;
                legSkillCooldownTimer = legSkillCooldown;
            }
            if (currentSkill == 3 && handSkillCooldownTimer <= 0) {
                handSkillActive = true;
                SoundManager.instance.PlaySound(UseSkillSound);

                handSkillActiveTimer = handSkillActiveTime;
                handSkillCooldownTimer = handSkillCooldown;
            }
        }

        //handle eye skill activate
        if (eyeSkillActive) {
            foreach (var light in lights) {
                light.enabled = true;
            }
            if (nightVisionTile != null) {
                nightVisionTile.GetComponent<TilemapRenderer>().enabled = true;
            }
            eyeSkillActiveTimer -= Time.deltaTime;
        } else {
            foreach (var light in lights) {
                if (light.CompareTag("WeakPoint")) {
                    light.enabled = false;
                }
            }
            if (nightVisionTile != null) {
                nightVisionTile.GetComponent<TilemapRenderer>().enabled = false;
            }
            eyeSkillCooldownTimer -= Time.deltaTime;
        }
        if (eyeSkillActiveTimer <= 0) {
            eyeSkillActive = false;
        }

        //handle leg skill activate
        if (legSkillActive) {
            legSkillActiveTimer -= Time.deltaTime;
        } else {
            legSkillCooldownTimer -= Time.deltaTime;
            jumpCount = 100;
        }
        if (legSkillActiveTimer <= 0) {
            legSkillActive = false;
        }

        //handle hand skill activate
        if (handSkillActive) {
            handSkillActiveTimer -= Time.deltaTime;
        } else {
            handSkillCooldownTimer -= Time.deltaTime;
        }
        if (handSkillActiveTimer <= 0) {
            handSkillActive = false;
        }

        //handle icon light
        if ((eyeSkillActive && currentSkill == 1) || (legSkillActive && currentSkill == 2) || (handSkillActive && currentSkill == 3)) {
            foreach (var light in lights) {
                if (light.CompareTag("Icon")) {
                    light.intensity = 1.5f;
                }
            }
        } else {
            foreach (var light in lights) {
                if (light.CompareTag("Icon")) {
                    light.intensity = 0.5f;
                }
            }
        }

        //handle icon cooldown mask
        if (currentSkill == 1) {
            if (eyeSkillActive) {
                iconMask.fillAmount = 1 - eyeSkillActiveTimer / eyeSkillActiveTime;
            } else {
                if (eyeSkillCooldownTimer >= 0) {
                    iconMask.fillAmount = eyeSkillCooldownTimer / eyeSkillCooldown;
                } else {
                    iconMask.fillAmount = 0;
                }
            }
        }
        if (currentSkill == 2) {
            if (legSkillActive) {
                iconMask.fillAmount = 1 - legSkillActiveTimer / legSkillActiveTime;
            } else {
                if (legSkillCooldownTimer >= 0) {
                    iconMask.fillAmount = legSkillCooldownTimer / legSkillCooldown;
                } else {
                    iconMask.fillAmount = 0;
                }
            }
        }
        if (currentSkill == 3) {
            if (handSkillActive) {
                iconMask.fillAmount = 1 - handSkillActiveTimer / handSkillActiveTime;
            } else {
                if (handSkillCooldownTimer >= 0) {
                    iconMask.fillAmount = handSkillCooldownTimer / handSkillCooldown;
                } else {
                    iconMask.fillAmount = 0;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {

            if (eyeSkill && legSkill && handSkill) {
                currentSkill += 1;

                if (currentSkill > 3) {
                    currentSkill = 1;
                }
            } else if (eyeSkill && legSkill) {
                currentSkill += 1;
                if (currentSkill > 2) {
                    currentSkill = 1;
                }
            } else if (eyeSkill) {
                currentSkill = 1;
            }
            if (currentSkill != 0) {
                iconController.ChangeIcon(currentSkill);
                SoundManager.instance.PlaySound(changeSkillSound);

            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {

            if (eyeSkill && legSkill && handSkill) {
                currentSkill -= 1;
                if (currentSkill < 1) {
                    currentSkill = 3;
                }
            } else if (eyeSkill && legSkill) {
                currentSkill -= 1;
                if (currentSkill < 1) {
                    currentSkill = 2;
                }
            } else if (eyeSkill) {
                currentSkill = 1;
            }
            if (currentSkill != 0) {
                iconController.ChangeIcon(currentSkill);
                SoundManager.instance.PlaySound(changeSkillSound);

            }
        }

        if (!animator.GetBool("WallStuck")) {

            GlobalVar.DynamicSpeedHorizontal = 0;
            GlobalVar.DynamicSpeedVertical = 0;

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if (FirstTimeFaceDirectionInitialize == 1) {
                horizontal = 1;
                FirstTimeFaceDirectionInitialize++;
            }
            if (horizontal > 0 && !dashing) {
                faceDirection = 1f;

            } else if (horizontal < 0f && !dashing) {
                faceDirection = -1f;
            }
            animator.SetFloat("FaceDirection", faceDirection);
            animator.SetFloat("MoveX", faceDirection);

            if (Input.GetButton("Horizontal")) {
                animator.SetBool("Running", true);
            } else {
                animator.SetBool("Running", false);
            }

            RaycastHit2D onGround1 = Physics2D.Raycast(GetBottomLeft(), Vector2.down, 0.2f, LayerMask.GetMask("Default") + LayerMask.GetMask("Dynamic") + LayerMask.GetMask("NoStuck"));
            RaycastHit2D onGround2 = Physics2D.Raycast(GetBottomPoint(), Vector2.down, 0.2f, LayerMask.GetMask("Default") + LayerMask.GetMask("Dynamic") + LayerMask.GetMask("NoStuck"));
            RaycastHit2D onGround3 = Physics2D.Raycast(GetBottomRight(), Vector2.down, 0.2f, LayerMask.GetMask("Default") + LayerMask.GetMask("Dynamic") + LayerMask.GetMask("NoStuck"));

            if (onGround1 || onGround2 || onGround3) {
                onGround = true;
                animator.SetBool("OnGround", true);
                if (legSkillActive) {
                    jumpCount = 0;
                }
            } else {
                onGround = false;
                animator.SetBool("OnGround", false);

            }

            animator.SetBool("NoDash", false);

            if (Input.GetKeyDown(KeyCode.Space) && (onGround || jumpCount <= 1)) {
                jump = true;
                SoundManager.instance.PlaySound(JumpSound);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && !nodash) {
                SoundManager.instance.PlaySound(DashSound);

                dash = true;
                dashCooldownTimer = dashCooldown;
                dashDirection = faceDirection;
            }
            if (dashing) {
                animator.SetBool("Dashing", true);
                animator.SetFloat("DashDirection", dashDirection);
            }
            if (!dashing) {
                animator.SetBool("Dashing", false);
                dashCooldownTimer -= Time.deltaTime;
            }



            shaking -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Z) && shaking <= 0f && !dashing) {
                animator.SetFloat("AttackDirection", animator.GetFloat("FaceDirection"));
                shaking = 0.5f;
                animator.SetBool("UpAttacking", true);
                animator.SetBool("MidAttacking", false);
                animator.SetBool("DownAttacking", false);
                animator.SetBool("Attacking", true);
                nodash = true;
                Invoke("ableDash", 1f);
                if (faceDirection > 0.1f) {
                    //Invoke("ActivateURKatana", 0.25f);
                    //Invoke("DeactivateURKatana", 0.45f);
                } else if (faceDirection < -0.1f) {
                    //Invoke("ActivateULKatana", 0.25f);
                    //Invoke("DeactivateULKatana", 0.45f);
                }

            } else if (Input.GetKeyDown(KeyCode.X) && shaking <= 0f && !dashing) {
                animator.SetFloat("AttackDirection", animator.GetFloat("FaceDirection"));
                shaking = 0.5f;
                animator.SetBool("UpAttacking", false);
                animator.SetBool("MidAttacking", true);
                animator.SetBool("DownAttacking", false);
                animator.SetBool("Attacking", true);
                nodash = true;
                Invoke("ableDash", 1f);
                if (faceDirection > 0.1f) {
                    //Invoke("ActivateMRKatana", 0.45f);
                    //Invoke("DeactivateMRKatana", 0.65f);
                } else if (faceDirection < -0.1f) {
                    //Invoke("ActivateMLKatana", 0.45F);
                    //Invoke("DeactivateMLKatana", 0.65f);
                }
            } else if (Input.GetKeyDown(KeyCode.C) && shaking <= 0f && !onGround && !dashing) {
                animator.SetFloat("AttackDirection", animator.GetFloat("FaceDirection"));
                shaking = 0.5f;
                animator.SetBool("UpAttacking", false);
                animator.SetBool("MidAttacking", false);
                animator.SetBool("DownAttacking", true);
                animator.SetBool("Attacking", true);
                nodash = true;
                Invoke("ableDash", 1f);
                if (faceDirection > 0.1f) {
                    //Invoke("ActivateDRKatana", 0.25f);
                    //Invoke("DeactivateDRKatana", 0.45f);
                } else if (faceDirection < -0.1f) {
                    //Invoke("ActivateDLKatana", 0.25f);
                    //Invoke("DeactivateDLKatana", 0.45f);
                }
            } else {
                animator.SetBool("UpAttacking", false);
                animator.SetBool("MidAttacking", false);
                animator.SetBool("DownAttacking", false);
            }
            GlobalVar.DynamicCode = 0;
            GlobalVar.DynamicBlockStuck = false;
        } else {
            jumpCount = 0;
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            //Invoke("ColliderOffsetup",0.1f);
            //Invoke("ColliderOffset", 0.2f);

            if (GlobalVar.DynamicBlockStuck) {

                transform.Translate(Vector2.right * GlobalVar.DynamicSpeedHorizontal * Time.deltaTime, Space.Self);
                transform.Translate(Vector2.up * GlobalVar.DynamicSpeedVertical * Time.deltaTime, Space.Self);
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                rigidbody2d.constraints = ~RigidbodyConstraints2D.FreezePosition;
                jump = true;
                SoundManager.instance.PlaySound(JumpSound);

                animator.SetBool("WallStuck", false);
                GlobalVar.DynamicBlockStuck = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                dashDirection = faceDirection;
                dash = true;
                rigidbody2d.constraints = ~RigidbodyConstraints2D.FreezePosition;
                animator.SetBool("WallStuck", false);
                GlobalVar.DynamicBlockStuck = false;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                rigidbody2d.constraints = ~RigidbodyConstraints2D.FreezePosition;
                animator.SetBool("WallStuck", false);
                GlobalVar.DynamicBlockStuck = false;
            }
        }
        stuntime -= Time.deltaTime;
    }

    void FixedUpdate() {
        rigidbody2d.velocity = new Vector2(horizontal * speed, rigidbody2d.velocity.y);

        if (jump) {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpHeight);
            jump = false;
            jumpCount++;
        }

        if (dash) {
            dashTimer = dashTime;
            dashing = true;
        }
        if (dashing) {
            dash = false;
            rigidbody2d.gravityScale = 0f;
            if (dashDirection >= 0f) {
                rigidbody2d.velocity = new Vector2(dashSpeed, 0);
            } else if (dashDirection < 0f) {
                rigidbody2d.velocity = new Vector2(-dashSpeed, 0);
            }
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer < 0) {
                dashing = false;
            }
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        } else {
            rigidbody2d.gravityScale = 3f;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

        }
    }


    bool exitCollision = false;
    private void OnCollisionStay2D(Collision2D other) {
        //Debug.Log("stay collision");
        if (GlobalVar.DynamicBlockStuck && GlobalVar.DynamicSpeedVertical != 0) {
            if (GlobalVar.DynamicSpeedVertical < 0) {
                rigidbody2d.constraints = ~RigidbodyConstraints2D.FreezePosition;
                animator.SetBool("WallStuck", false);
                GlobalVar.DynamicBlockStuck = false;
            } else if (exitCollision) {
                rigidbody2d.constraints = ~RigidbodyConstraints2D.FreezePosition;
                animator.SetBool("WallStuck", false);
                GlobalVar.DynamicBlockStuck = false;
            }
            //Debug.Log("dynamic stuck and ");
            /*
            if (other.gameObject.layer == LayerMask.NameToLayer("Default") || other.gameObject.layer == LayerMask.NameToLayer("Trap")) {
                Debug.Log("colliding with default or trap layer");
                if (animator.GetBool("WallStuck")) {
                    rigidbody2d.constraints = ~RigidbodyConstraints2D.FreezePosition;
                    animator.SetBool("WallStuck", false);
                    GlobalVar.DynamicBlockStuck = false;
                }
            }
            */
        }
        exitCollision = false;
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Default") || other.gameObject.layer == LayerMask.NameToLayer("Trap")) {
            exitCollision = true;
        }
    }

    private void ColliderOffsetup() {
        playerCollider.offset = new Vector2(playerCollider.offset.x, playerCollider.offset.y + 1f);
    }
    private void ColliderOffset() {
        playerCollider.offset = new Vector2(playerCollider.offset.x, playerCollider.offset.y - 1f);
    }
    public void Freeze() {
        rigidbody2d.constraints = ~RigidbodyConstraints2D.FreezePosition;
    }
    public void HPRecover(int amount) {
        hp += amount;
    }

    public void HPRecoverAll() {
        hp = Sethp;
    }

    public void AddLife() {
        lifespan++;
    }
    public void ChangeHP() {
        if (stuntime < 0) {
            if (hp > 0) {
                hp--;
                if (hp > 0) {
                    animator.SetTrigger("Hit");
                    SoundManager.instance.PlaySound(GotDamageSound);

                    stuntime = 1.5f;
                }
                healthBar.SetValue(hp / (float)Sethp);

            }
            if (hp <= 0) {
                animator.SetTrigger("Death");
                if (lifespan > 0) {
                    lifespan--;
                    SoundManager.instance.PlaySound(DieSound);

                    stuntime = 4f;
                    rigidbody2d.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

                    Invoke("Teleport", 4f);
                    hp = Sethp;
                    healthBar.SetValue(hp / (float)Sethp);

                } else if (lifespan <= 0) {
                    uiManager.GameOver();

                    stuntime = 1000f;
                    Debug.Log("Death");

                }
            }
            rigidbody2d.constraints = ~RigidbodyConstraints2D.FreezePosition;
            animator.SetBool("WallStuck", false);
            GlobalVar.DynamicBlockStuck = false;
        }
    }

    public void Teleport() {
        Transform safepointTransform = SafeCollection.transform.GetChild(GlobalVar.SafePointPosition).transform;
        transform.position = safepointTransform.position;
        rigidbody2d.constraints = ~RigidbodyConstraints2D.FreezePosition;
        animator.SetTrigger("Respawn");
    }

    private Vector2 GetBottomPoint() {
        Collider2D collider = rigidbody2d.GetComponent<Collider2D>();
        Vector2 colliderSize = collider.bounds.size;
        Vector2 bottomPoint = new Vector2(rigidbody2d.position.x, rigidbody2d.position.y - colliderSize.y * 0.5f);

        return bottomPoint;
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
    private void ableDash() {
        nodash = false;
    }

    private void ActivateULKatana() {
        UpLCollider.SetActive(true);
    }
    private void DeactivateULKatana() {
        UpLCollider.SetActive(false);
    }
    private void ActivateURKatana() {
        UpRCollider.SetActive(true);
    }
    private void DeactivateURKatana() {
        UpRCollider.SetActive(false);
    }
    private void ActivateMRKatana() {
        MidRCollider.SetActive(true);
    }
    private void DeactivateMRKatana() {
        MidRCollider.SetActive(false);
    }
    private void ActivateMLKatana() {
        MidLCollider.SetActive(true);
    }
    private void DeactivateMLKatana() {
        MidLCollider.SetActive(false);
    }
    private void ActivateDRKatana() {
        DownRCollider.SetActive(true);
    }
    private void DeactivateDRKatana() {
        DownRCollider.SetActive(false);
    }
    private void ActivateDLKatana() {
        DownLCollider.SetActive(true);
    }
    private void DeactivateDLKatana() {
        DownLCollider.SetActive(false);
    }
    private void DeactivateAllKatana() {
        UpLCollider.SetActive(false);
        UpRCollider.SetActive(false);
        MidRCollider.SetActive(false);
        MidLCollider.SetActive(false);
        DownRCollider.SetActive(false);
        DownLCollider.SetActive(false);
    }
    public void GetEyeSkill() {
        SoundManager.instance.PlaySound(getskillSound);

        eyeSkill = true;
        currentSkill = 1;
        iconController.ChangeIcon(currentSkill);
    }

    public void GetLegSkill() {
        SoundManager.instance.PlaySound(getskillSound);

        legSkill = true;
        currentSkill = 2;
        iconController.ChangeIcon(currentSkill);
    }

    public void GetHandSkill() {
        SoundManager.instance.PlaySound(getskillSound);

        handSkill = true;
        currentSkill = 3;
        iconController.ChangeIcon(currentSkill);
    }
    private void Slash1() {
        SoundManager.instance.PlaySound(slashSound);
        return;
    }



    private void Walk1() {
        SoundManager.instance.PlaySound(WalkSound1);
        return;
    }

    private void Walk2() {
        SoundManager.instance.PlaySound(WalkSound2);
        return;
    }
}
