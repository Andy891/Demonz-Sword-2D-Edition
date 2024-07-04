using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    float timer = 5f;
    Animator animator;
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }
    public void Shoot(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction.normalized * force);
    }

    public void SetFaceDirection(int faceDirection)
    {
        animator.SetFloat("FaceDirection", faceDirection);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHP();
            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            animator.SetTrigger("Hit");
            BoxCollider2D boxCollider2d = GetComponent<BoxCollider2D>();
            boxCollider2d.isTrigger = true;
        }
    }
}
