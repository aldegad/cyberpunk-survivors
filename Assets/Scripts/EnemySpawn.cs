using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public float radious = 50;
    public int enemyCount;
    private float spawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= spawnTime)
        {
            spawnTime = Time.time + 0.1f;
            EnemyDrop();
        }
        
    }

    void EnemyDrop()
    {
        
        float playerX = player.transform.position.x;
        float playerZ = player.transform.position.z;
        Vector2 spawnPoint = Random.insideUnitCircle * radious + new Vector2(playerX, playerZ);
        
        Instantiate(enemy, new Vector3(spawnPoint.x, 0, spawnPoint.y), Quaternion.identity);
    }
}
