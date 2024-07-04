using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegSkillPickup : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) {
            player.GetLegSkill();
            Destroy(gameObject);
        }
    }
}
