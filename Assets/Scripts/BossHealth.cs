using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Fungus;


public class BossHealth : MonoBehaviour
{

    public int health = 20, enrageHp = 10;

    public GameObject deathEffect, weakPointPrefab;
    [SerializeField] private AudioClip BossDieSound;
    [SerializeField] private string blockName;


    public bool isInvulnerable = false;
    public Flowchart Flowchart;




    public GameObject[] weakPoints;

    int activeWeakPointCount;

    private void Update()
    {
        if (health <= 0)
        {
            return;
        }
        activeWeakPointCount = 0;
        foreach (var weakPoint in weakPoints)
        {
            BossWeakPointController bossWeakPointController = weakPoint.GetComponent<BossWeakPointController>();
            if (bossWeakPointController.isActive)
            {
                activeWeakPointCount++;
            }
        }
        if (activeWeakPointCount <= 0)
        {
            int i = Random.Range(0, weakPoints.Length);
            Instantiate(weakPointPrefab, weakPoints[i].transform);
            BossWeakPointController bossWeakPointController = weakPoints[i].GetComponent<BossWeakPointController>();
            bossWeakPointController.isActive = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
            return;

        health -= damage;
        if (health > 0)
        {
            GetComponent<Animator>().SetTrigger("Hit");
        }
        if (health <= enrageHp)
        {
            GetComponent<Animator>().SetBool("IsEnraged", true);
        }

        if (health <= 0)
        {
            if (Flowchart != null && !string.IsNullOrEmpty(blockName))
            {
                Flowchart.ExecuteBlock(blockName);
            }
            Die();
            SoundManager.instance.PlaySound(BossDieSound);


        }
        Debug.Log(health);
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}

