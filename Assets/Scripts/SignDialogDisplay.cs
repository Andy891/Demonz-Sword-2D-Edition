using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignDialogDisplay : MonoBehaviour {
    GameObject dialogbox;
    Transform playerTransform;
    // Start is called before the first frame update
    void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        dialogbox = GetComponentInChildren<Canvas>().gameObject;
        dialogbox.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (Vector2.Distance(playerTransform.position, transform.position) <= 1f) {
            dialogbox.SetActive(true);
        } else if (dialogbox.activeSelf == true) {
            dialogbox.SetActive(false);
        }
    }
}
