using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int carCount = 0;

    public List<GameObject> carPrefabs;

    private Vector3[] spawnPositions = new Vector3[] {
        new Vector3(90.0f, 0, 190.0f),
        new Vector3(-90.0f, 0, 180.0f)
    };

    private Quaternion[] spawnRotations = new Quaternion[] {
        new Quaternion(0.0f, -1.0f, 0.0f, 1.0f),
        new Quaternion(0.0f, 1.0f, 0.0f, 1.0f)
    };

    private int spawnBuffer = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        carCount = GameObject.FindGameObjectsWithTag("Car").Length;

        if (carCount <= 4 && spawnBuffer < 0)
        {
            SpawnCar();
            spawnBuffer = 1000;
        }
        spawnBuffer--;
    }




    void SpawnCar()
    {
        // select a random car model
        int carID = Random.Range(0, carPrefabs.Count);
        GameObject carPrefab = carPrefabs[carID];

        // select a random spawn position and direction
        int spawnID = Random.Range(0, spawnPositions.Length);

        Vector3 spawPosition = spawnPositions[spawnID];
        Quaternion spawnRotation = spawnRotations[spawnID];

        Instantiate(carPrefab, spawPosition, carPrefab.transform.rotation * spawnRotation);

    }
}
