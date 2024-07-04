using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    public float speed = 5f;  // 移动速度
    public float startDelay = 0f;
    public float destroyDelay = 2f;  // 延迟销毁的时间
    public Transform targetPosition;
    private bool Go = false;

    private void Start()
    {
        Invoke("StartMoving", startDelay);
    }

    private void Update()
    {
        // 移动游戏对象
        if (Go)
        {
            Vector3 direction = (targetPosition.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }

    }
    private void StartMoving()
    {
        Go = true;
        Invoke("DestroyObject", destroyDelay + startDelay);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void DeactivateScript()
    {
        enabled = true;
    }
}
