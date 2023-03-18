using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingRedSpawnScript : MonoBehaviour
{
    public GameObject building;
    public float spawnRate = 6;
    private float timer = 0;
    public float heightOffset = 2;
    public float randomMax = 3;
    public float randomFactor;

    // Start is called before the first frame update
    void Start()
    {
        randomFactor = Random.Range(0, randomMax);
        spawnBuilding();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate + randomFactor){
            timer += Time.deltaTime;
        } else {
            spawnBuilding();
            timer = 0;
        }
    }

    void spawnBuilding() {
        float lowestPoint = transform.position.y - heightOffset/2;
        float highestPoint = transform.position.y + heightOffset;

        Instantiate(building, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
        
        randomFactor = Random.Range(0, randomMax);
    }
}
