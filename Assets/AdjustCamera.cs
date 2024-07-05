using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustCamera : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<PlayerController>() != null) {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize = 15;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().yOffset = 3;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.GetComponent<PlayerController>() != null) {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize = 6;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().yOffset = 1;
        }
    }
}
