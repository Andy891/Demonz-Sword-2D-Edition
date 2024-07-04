using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3_Attack : MonoBehaviour
{
    public Transform chargePosition;
    public GameObject chargePrefab, lightingPrefab, 危prefab;

    [SerializeField] private AudioClip DangerSound;
    [SerializeField] private AudioClip AttackSound;


    Vector2 player, player1, player2, player3;
    float ultCount = 0, ultCount1 = 0, ultCount3 = 0, ultCount4 = 0; //-30 to 30
    GameObject ultTarget;
    public void AttackLeft()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + new Vector3(-3.28f, 4.51f, 0), 4, Vector2.down, 7.81f);
        SoundManager.instance.PlaySound(AttackSound);

        foreach (RaycastHit2D hit in hits)
        {
            PlayerController player = hit.transform.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ChangeHP();
            }
        }
    }

    public void AttackRight()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + new Vector3(3.28f, 4.51f, 0), 4, Vector2.down, 7.81f);
        SoundManager.instance.PlaySound(AttackSound);


        foreach (RaycastHit2D hit in hits)
        {
            PlayerController player = hit.transform.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ChangeHP();
            }
        }
    }

    public void Charge()
    {
        Instantiate(chargePrefab, chargePosition, false);
        player = GameObject.FindGameObjectWithTag("Player").transform.position;
        LightingOnPlayer();
    }

    public void LightingOnPlayer()
    {
        Instantiate(危prefab, player + Vector2.up * 2.5f, Quaternion.identity);
        SoundManager.instance.PlaySound(DangerSound);

        Invoke("Strike", 0.83333333333f);
    }

    void Strike()
    {
        Instantiate(lightingPrefab, player + Vector2.up * 14.1f, Quaternion.identity);
    }

    public void Charge3Times()
    {
        Instantiate(chargePrefab, chargePosition, false);
        LightingOnPlayer1();
    }

    public void LightingOnPlayer1()
    {
        player1 = GameObject.FindGameObjectWithTag("Player").transform.position;
        Instantiate(危prefab, player1 + Vector2.up * 2.5f, Quaternion.identity);
        Invoke("Strike1", 0.83333333333f);
        Invoke("LightingOnPlayer2", 0.5f);
    }

    public void LightingOnPlayer2()
    {
        player2 = GameObject.FindGameObjectWithTag("Player").transform.position;
        Instantiate(危prefab, player2 + Vector2.up * 2.5f, Quaternion.identity);
        Invoke("Strike2", 0.83333333333f);
        Invoke("LightingOnPlayer3", 0.5f);
    }

    public void LightingOnPlayer3()
    {
        player3 = GameObject.FindGameObjectWithTag("Player").transform.position;
        Instantiate(危prefab, player3 + Vector2.up * 2.5f, Quaternion.identity);
        Invoke("Strike3", 0.83333333333f);
    }

    void Strike1()
    {
        Instantiate(lightingPrefab, player1 + Vector2.up * 14.1f, Quaternion.identity);
    }

    void Strike2()
    {
        Instantiate(lightingPrefab, player2 + Vector2.up * 14.1f, Quaternion.identity);
    }

    void Strike3()
    {
        Instantiate(lightingPrefab, player3 + Vector2.up * 14.1f, Quaternion.identity);
    }

    public IEnumerator UltimateLeft()
    {
        Invoke("InvokeUltimateStrikeLeft", 0.5f);
        while (ultCount < 15)
        {
            Instantiate(危prefab, transform.position + new Vector3((ultCount - 7.5f) * 4, -3f, 0), Quaternion.identity);
            ultCount++;
            yield return new WaitForSeconds(0.15f);
        }

        ultCount = 0;
    }

    IEnumerator UltimateStrikeLeft()
    {
        while (ultCount1 < 15)
        {
            Instantiate(lightingPrefab, transform.position + new Vector3((ultCount1 - 7.5f) * 4, 8.69f, 0), Quaternion.identity);
            ultCount1++;
            yield return new WaitForSeconds(0.15f);
        }

        ultCount1 = 0;
    }

    public void InvokeUltimateStrikeLeft()
    {
        StartCoroutine(UltimateStrikeLeft());
    }

    public IEnumerator UltimateRight()
    {
        Invoke("InvokeUltimateStrikeRight", 0.5f);
        while (ultCount3 < 15)
        {
            Instantiate(危prefab, transform.position + new Vector3((-ultCount3 + 7.5f) * 4, -3f, 0), Quaternion.identity);
            ultCount3++;
            yield return new WaitForSeconds(0.15f);
        }

        ultCount3 = 0;
    }

    IEnumerator UltimateStrikeRight()
    {
        while (ultCount4 < 15)
        {
            Instantiate(lightingPrefab, transform.position + new Vector3((-ultCount4 + 7.5f) * 4, 8.69f, 0), Quaternion.identity);
            ultCount4++;
            yield return new WaitForSeconds(0.15f);
        }

        ultCount4 = 0;
    }

    public void InvokeUltimateStrikeRight()
    {
        StartCoroutine(UltimateStrikeRight());
    }
}
