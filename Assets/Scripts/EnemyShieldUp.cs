using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyShieldUp : MonoBehaviour
{
    public bool shieldUp = false;
    Animator animator;
    Rigidbody2D rigidbody2d;
    EnemyController enemyController;
    [SerializeField] private AudioClip ShieldSound;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();
    }

    public void ShieldUp()
    {
        if (enemyController.weakPointCount > 0)
        {
            animator.SetTrigger("ShieldUp");
            SoundManager.instance.PlaySound(ShieldSound);

            shieldUp = true;
        }
    }

    public void ShieldDown()
    {
        animator.ResetTrigger("ShieldUp");
        shieldUp = false;
    }
}
