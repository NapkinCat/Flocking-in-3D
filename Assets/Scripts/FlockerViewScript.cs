using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockerViewScript : MonoBehaviour
{
    public GameObject parentObject;
    public int currentlyViewing = 0;
    public int maxViewable = 3;

    // Start is called before the first frame update
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Flocker"))
        {
            parentObject.GetComponent<Flocker>().nearbyFlockers.Add(other.gameObject);
            currentlyViewing++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Flocker"))
        {
            parentObject.GetComponent<Flocker>().nearbyFlockers.Remove(other.gameObject);
            currentlyViewing--;
        }
    }
}
