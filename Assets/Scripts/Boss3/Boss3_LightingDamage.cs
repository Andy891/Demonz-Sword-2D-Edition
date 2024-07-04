using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3_LightingDamage : MonoBehaviour
{

    [SerializeField] private AudioClip LightingDamageSound;

    void LightingDamage()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 2, Vector2.down, 14);
        SoundManager.instance.PlaySound(LightingDamageSound);

        foreach (RaycastHit2D hit in hits)
        {
            PlayerController player = hit.transform.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ChangeHP();
            }
        }
    }
}
