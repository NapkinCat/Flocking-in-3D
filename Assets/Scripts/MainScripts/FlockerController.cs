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
    public bool behaviorRaysViewable;
    public bool flockerViewSphereViewable;

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
        
        
        //Input to spawn a boid
        if(Input.GetKey(KeyCode.Q))
        {
            numberOfFlockersToSpawn = pressToSpawnAmount;
            while (numberOfFlockersToSpawn > 0)
            {
                SpawnFlocker();
                numberOfFlockersToSpawn--;
            }
        }
        //Change the behavior of all boids on the field to red.
        if(Input.GetKeyDown(KeyCode.R))
        {
            foreach(GameObject flocker in flockers)
            {
                flocker.GetComponent<Flocker>().SetBehavior(Color.red);
            }
        }
        //Change the behavior of all boids on the field to green.
        if (Input.GetKeyDown(KeyCode.G))
        {
            foreach (GameObject flocker in flockers)
            {
                flocker.GetComponent<Flocker>().SetBehavior(Color.green);
            }
        }
        //Change the behavior of all boids on the field to blue.
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (GameObject flocker in flockers)
            {
                flocker.GetComponent<Flocker>().SetBehavior(Color.blue);
            }
        }
        //Change the behavior of all boids on the field to white.
        if (Input.GetKeyDown(KeyCode.T))
        {
            foreach (GameObject flocker in flockers)
            {
                flocker.GetComponent<Flocker>().SetBehavior(Color.white);
            }
        }
        //Change the behavior of all boids on the field to black.
        if (Input.GetKeyDown(KeyCode.Y))
        {
            foreach (GameObject flocker in flockers)
            {
                flocker.GetComponent<Flocker>().SetBehavior(Color.black);
            }
        }
        //Remove all boids from the field.
        if (Input.GetKeyDown(KeyCode.Z))
        {
            foreach (GameObject flocker in flockers)
            {
                Destroy(flocker);
            }
            flockers.Clear();
        }
        //Toggle flocker viewable metrics
        if (Input.GetKeyDown(KeyCode.F))
        {
            behaviorRaysViewable = !behaviorRaysViewable;
            foreach (GameObject flocker in flockers)
            {
                flocker.GetComponent<Flocker>().SetBehaviorViewable(behaviorRaysViewable);
            }
        }
        //Toggle flocker viewable metrics
        if (Input.GetKeyDown(KeyCode.V))
        {
            flockerViewSphereViewable = !flockerViewSphereViewable;
            foreach (GameObject flocker in flockers)
            {
                flocker.GetComponent<Flocker>().SetViewDistanceViewable(flockerViewSphereViewable);
            }
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
