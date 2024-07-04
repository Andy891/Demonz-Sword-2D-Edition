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
    private Vector2 startPosition; // 起始位置

    void Start()
    {
        // 記錄箱子的起始位置
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
        // 根據移動方向移動箱子
        if (isMovingRight)
        {
            transform.Translate(Vector2.right * HorizontalSpeed * Time.deltaTime);

            // 如果到達右側邊界,就改變移動方向
            if (transform.position.x >= startPosition.x + rightBound)
            {
                isMovingRight = false;
                HorizontalSpeed = -HorizontalSpeed;
            }
        }
        else
        {
           
            transform.Translate(Vector2.right * HorizontalSpeed * Time.deltaTime);

            // 如果到達左側邊界,就改變移動方向
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
