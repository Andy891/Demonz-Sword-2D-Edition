using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float HorizontalSpeed = 1f;
    public float VerticalSpeed = 1f;
    public float upBound = 5f;
    public float downBound = -5f;
    public float leftBound = -5f;
    public float rightBound = 5f; 
    private bool TriggerOn = false;
    private bool isMovingRight = true;
    private bool isMovingUp = true;
    private Vector2 startPosition; // �_�l��m

    void Start()
    {
        // �O���c�l���_�l��m
        startPosition = transform.position;
    }
    void Update()
    {   if (TriggerOn && !GlobalVar.DynamicBlockStuck) {
            TriggerOn = false;
        }

        if (TriggerOn) { 
            GlobalVar.DynamicSpeedHorizontal = HorizontalSpeed;
            GlobalVar.DynamicSpeedVertical = VerticalSpeed;
        }
        // �ھڲ��ʤ�V���ʽc�l
        if (isMovingRight)
        {
            transform.Translate(Vector2.right * HorizontalSpeed * Time.deltaTime);

            // �p�G��F�k�����,�N���ܲ��ʤ�V
            if (transform.position.x >= startPosition.x + rightBound)
            {
                isMovingRight = false;
                HorizontalSpeed = -HorizontalSpeed;
            }
        }
        else
        {
           
            transform.Translate(Vector2.right * HorizontalSpeed * Time.deltaTime);

            // �p�G��F�������,�N���ܲ��ʤ�V
            if (transform.position.x < startPosition.x + leftBound)
            {
                isMovingRight = true;
                HorizontalSpeed = -HorizontalSpeed;
            }
        }

        if (isMovingUp)
        {
            transform.Translate(Vector2.up * VerticalSpeed * Time.deltaTime);

            if (transform.position.y >= startPosition.y + upBound)
            {
                isMovingUp = false;
                 VerticalSpeed = -VerticalSpeed;
            }
        }
        else
        {

            transform.Translate(Vector2.up * VerticalSpeed * Time.deltaTime);

            if (transform.position.y < startPosition.y + downBound)
            {
                isMovingUp = true;
                VerticalSpeed = -VerticalSpeed;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            TriggerOn = true;
        }
    }
}
