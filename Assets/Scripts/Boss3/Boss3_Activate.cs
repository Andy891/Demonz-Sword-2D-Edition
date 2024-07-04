using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3_Activate : MonoBehaviour {
    [SerializeField] GameObject boss3;
    Transform player;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update() {
        if (Vector2.Distance(player.position, transform.position) <= 10)
            if (boss3 != null)
                boss3.SetActive(true);
    }
}
