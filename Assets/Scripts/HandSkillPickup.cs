using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSkillPickup : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) {
            player.GetHandSkill();
            Destroy(gameObject);
        }
    }
}
