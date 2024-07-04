using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPointController : MonoBehaviour {

    public bool isActive;

    private void Update() {
        if (isActive) {
            WeakPoint weakPoint = GetComponentInChildren<WeakPoint>();
            if (weakPoint == null) {
                isActive = false;
                BossHealth bossHealth = GetComponentInParent<BossHealth>();
                bossHealth.TakeDamage(1);
            }
        }
    }
}
