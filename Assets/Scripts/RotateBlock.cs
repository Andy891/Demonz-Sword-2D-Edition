using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlock : MonoBehaviour
{
    public float rotationSpeed = 45f;
    public bool clockwise = true;
    private PlayerController player;
    private Animator playerAnimator;
    void Start()
    {
        
    }

    // Update is called once per frame
    

    private void Update()
    {
        if (clockwise)
        {
            transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
        }
        else {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            clockwise = !clockwise;
        }
    }
}
