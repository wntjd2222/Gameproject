using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public bool enableSpawn = true;
    public GameObject Enemy;
    public int enemy1count = 0;
    void Spawn_Enemy()
    {
        float randomX = Random.Range(-23, 23);
        if(enableSpawn && enemy1count < 6)
        {
            GameObject enemy = (GameObject)Instantiate(Enemy, new Vector3(randomX, 2.1f, 0), Quaternion.identity);
            enemy1count++;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn_Enemy", 3, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
