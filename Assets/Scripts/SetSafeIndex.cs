using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSafeIndex : MonoBehaviour
{
    public int SetIndex = 0;
    public GameObject block;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            GlobalVar.SafePointPosition = SetIndex;
            if (block != null) { 
                block.SetActive(false);
            }
        }
    }
}
