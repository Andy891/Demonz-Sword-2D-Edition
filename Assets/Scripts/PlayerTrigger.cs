using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    PlayerController player;
    public bool RecoveryAll = true;
    public bool Addlife = true;
    public bool RecoverywithAmount = false;
    public int Amount = 1;
    [SerializeField] private AudioClip hpUPSound;

    public bool Hit = false;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (!Hit)
                {
                    if (RecoveryAll)
                    {
                        player.HPRecoverAll();


                    }
                    if (Addlife)
                    {
                        player.AddLife();
                        SoundManager.instance.PlaySound(hpUPSound);

                    }
                    if (RecoverywithAmount)
                    {
                        player.HPRecover(Amount);
                    }

                    Destroy(gameObject);
                }
                else
                {
                    player.ChangeHP();

                }

            }
        }
    }
}
