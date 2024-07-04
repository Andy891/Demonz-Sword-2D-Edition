using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrap : MonoBehaviour
{
    public GameObject objectPrefab;  // 要生成的游戏对象的预制体
    public float interval = 1f;  // 生成时间间隔
    public float startTime = 0f;  // 开始生成的时间

    private void Start()
    {
        StartCoroutine(GenerateObjects());
    }

    private IEnumerator GenerateObjects()
    {
        yield return new WaitForSeconds(startTime);

        while (true)
        {
            Instantiate(objectPrefab, transform.position, objectPrefab.transform.rotation);

            yield return new WaitForSeconds(interval);
        }
    }
}
