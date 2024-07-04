using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class KatanaDetect : MonoBehaviour
{
    public Animator Player;
    private PlayerController player;
    private int Default, Enemy, EnemyWeakPoint, Trap, Dynamic;

    void Awake()
    {
        player = Player.GetComponentInParent<PlayerController>();
        Default = LayerMask.NameToLayer("Default");
        Enemy = LayerMask.NameToLayer("Enemy");
        EnemyWeakPoint = LayerMask.NameToLayer("EnemyWeakPoint");
        Trap = LayerMask.NameToLayer("Trap");
        Dynamic = LayerMask.NameToLayer("Dynamic");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.handSkillActive && collision.CompareTag("Breakable"))
        {
            Destroy(collision.gameObject);
        }
        else
        {
            if (collision.gameObject.layer == EnemyWeakPoint)
            {
                Light2D light2d = collision.GetComponentInChildren<Light2D>();
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.layer == Enemy)
            {
                Debug.Log("Enemy");
                EnemyShieldUp enemyShieldUp = collision.GetComponent<EnemyShieldUp>();
                if (enemyShieldUp != null)
                {
                    enemyShieldUp.ShieldUp();
                }
            }
            else if (collision.gameObject.layer == Dynamic)
            {
                GlobalVar.DynamicBlockStuck = true;
                Player.SetBool("WallStuck", true);
            }
            else if (collision.gameObject.layer == Default || collision.gameObject.layer == Trap)
            {
                Player.SetBool("WallStuck", true);
            }
        }
    }
}