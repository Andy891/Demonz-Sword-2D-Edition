using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour {
    [SerializeField] int damage = 1;
    bool stopDamage = false;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerStay2D(Collider2D other) {
        if (stopDamage) {
            return;
        }
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) {
            player.ChangeHP();
        }
    }
    public void SetDamage(int value) {
        if (value <= 0) {
            damage = 1;
        } else {
            damage = value;
        }
    }
    void SetStopDamage() {
        stopDamage = true;
    }
    void DestroyGameObject() {
        Destroy(gameObject);
    }
}
