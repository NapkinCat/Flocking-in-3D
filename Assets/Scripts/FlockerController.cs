using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockerController : MonoBehaviour
{
    //Contain our list of all existing flockers
    public List<GameObject> flockers;
    public GameObject flockerPrefab;
    public int numberOfFlockersToSpawn;
    public int pressToSpawnAmount;

    private Vector3 spawnPos;
    
    // Start is called before the first frame update
    void Start()
    {
        while(numberOfFlockersToSpawn > 0)
        {
            SpawnFlocker();
            numberOfFlockersToSpawn--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            numberOfFlockersToSpawn = pressToSpawnAmount;
            while (numberOfFlockersToSpawn > 0)
            {
                SpawnFlocker();
                numberOfFlockersToSpawn--;
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            foreach(GameObject flocker in flockers)
            {
                flocker.GetComponent<Flocker>().SetBehavior(Color.red);
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            foreach (GameObject flocker in flockers)
            {
                flocker.GetComponent<Flocker>().SetBehavior(Color.green);
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (GameObject flocker in flockers)
            {
                flocker.GetComponent<Flocker>().SetBehavior(Color.blue);
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            foreach (GameObject flocker in flockers)
            {
                flocker.GetComponent<Flocker>().SetBehavior(Color.white);
            }
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            foreach (GameObject flocker in flockers)
            {
                flocker.GetComponent<Flocker>().SetBehavior(Color.black);
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            foreach (GameObject flocker in flockers)
            {
                Destroy(flocker);
            }
            flockers.Clear();
        }
    }

    //return an arbitrarily assigned vector 3 for spawn position.
    Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
    }

    void SpawnFlocker()
    {
        spawnPos = GetRandomSpawnPosition();
        flockers.Add(Instantiate(flockerPrefab, spawnPos, flockerPrefab.transform.rotation));
    }
}
