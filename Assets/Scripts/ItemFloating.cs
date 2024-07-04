using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemFloating : MonoBehaviour {
    public float height = 0.5f;
    public float duration = 1f;
    Vector2 startTransform, tragetTransform;
    bool up;
    float elapsedTime;
    // Start is called before the first frame update
    void Start() {
        startTransform = transform.position;
        tragetTransform = transform.position;
        tragetTransform += Vector2.up * height;
        up = true;
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update() {
        elapsedTime += Time.deltaTime;
        transform.position = new Vector2(transform.position.x, Mathf.Lerp(startTransform.y, tragetTransform.y, elapsedTime / duration));
        if (up) {
            if (transform.position.y >= tragetTransform.y) {
                elapsedTime = 0;
                up = !up;
                startTransform = transform.position;
                tragetTransform = transform.position;
                tragetTransform += Vector2.down * height * 2;
            }
        } else {
            if (transform.position.y <= tragetTransform.y) {
                elapsedTime = 0;
                up = !up;
                startTransform = transform.position;
                tragetTransform = transform.position;
                tragetTransform += Vector2.up * height * 2;
            }
        }
    }
}
