using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearAndDisappear : MonoBehaviour
{
    public GameObject targetObject; // 指向你想要顯示和隱藏的物體
    public float disappearTime = 2f;
    public float appearTime = 1f;
    public float Offset = 1f;
    public int SetDynamicCode = 0;
    private PlayerController player;
    private Animator playerAnimator;

    private void Start()
    {
        if (targetObject != null)
        {
            GameObject playerObject = GameObject.Find("Player");
            if (playerObject != null)
            {
                playerAnimator = playerObject.GetComponent<Animator>();
                player = playerObject.GetComponent<PlayerController>();
            }
            StartCoroutine(RepeatingToggle());
        }
    }

    private IEnumerator RepeatingToggle()
    {
        // 首先等待初始偏移時間
        yield return new WaitForSeconds(Offset);
        Offset = 0;
        while (true)
        {
            // 隱藏物體
            if (GlobalVar.DynamicCode == SetDynamicCode && GlobalVar.DynamicBlockStuck)
            {
                GlobalVar.DynamicBlockStuck = false;
                player.Freeze();
                playerAnimator.SetBool("WallStuck", false);
                
            }
            targetObject.SetActive(false);
            yield return new WaitForSeconds(disappearTime);

            // 顯示物體
            
            targetObject.SetActive(true);
            yield return new WaitForSeconds(appearTime);
        }
    }

}
