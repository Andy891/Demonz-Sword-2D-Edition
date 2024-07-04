using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    // Start is called before the first frame update
    Rigidbody2D rigidbody2d;
    float timer = 5f;
    Animator animator;
    void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        timer -= Time.deltaTime;
        if (timer < 0) {
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector2 direction, float force) {
        rigidbody2d.AddForce(direction.normalized * force);

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the transform
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));e
        transform.Rotate(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        PlayerController player = other.collider.GetComponent<PlayerController>();
        if (player != null) {
            player.ChangeHP();
        }
        if (animator != null) {
            animator.SetTrigger("Destroy");
            BoxCollider2D boxCollider2d = GetComponent<BoxCollider2D>();
            boxCollider2d.enabled = false;
            rigidbody2d.velocity = Vector2.zero;
        } else {
            DestroyGameObject();
        }
    }

    void DestroyGameObject() {
        Destroy(gameObject);
    }
}
