using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flocker : MonoBehaviour
{
    #region Public Variables

    //view Variable will store a reference to the spherecollider child which we use to evaluate what the flocker can see and how far
    public GameObject view;
    public GameObject viewMesh;
    public GameObject alignmentVectorLine;
    public GameObject cohesionVectorLine;
    public GameObject seperationVectorLine;
    public float viewDistance;
    public float desiredSeperation;
    //This Flocker's Flocking Behavior where X = Cohesion, Y = Alignment, and Z = Separation;
    public Vector3 flockBehaviorMod;
    //Variables to control our direction of travel and the variance by which we scale our random behavior
    public Vector3 forceDirection;
    public float forceVariance = 1.0f; 
    public float forceModifier = 1.0f;
    public float maxVelocity = 30.0f;
    public float flockBehaviorVariance;

    //setup our AI flocking vectors to control force application, thus direction of movement
    //We should apply these to our forceDirection vector so that
    //  if we are too far away from a neighbor, we tend towards them (cohese)
    //  if we are too close to a neighbor, we tend away from them (seperate)
    //  and we tend towards the direction they are moving (align)
    // our forceDirection vector will be slerped towards the combination of these three vectors so as to tend towards instead of snap towards
    // We should also be multiplying each vector by it's appropriate alignment in our flockBehaviorMod to weigh the vectors based on the individual flocker's behavior.
    public Vector3 alignmentVector;
    public Vector3 seperationVector;
    public Vector3 cohesionVector;

    //Instead of the above
    //Store a list of nearby flockers to consider for flocking behavior
    public List<GameObject> nearbyFlockers;

    #endregion
    #region Private Variables
    //References for Unity Components
    private Rigidbody rb;
    private SphereCollider viewRb;
    private Renderer rend;
    private LineRenderer aVLRend;
    private LineRenderer cVLRend;
    private LineRenderer sVLRend;
    //seekrate should control how quickly the flocker conforms to it's neighbor's behaviors
    private float seekRate;



    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        viewRb = view.GetComponent<SphereCollider>();
        viewRb.radius = viewDistance;
        viewMesh.transform.localScale = new Vector3(viewDistance, viewDistance, viewDistance) * 2;
        //Temporary: assign a random flock behavior mod
        flockBehaviorMod = GetRandomVectorWithRange(flockBehaviorVariance, 1.0f + flockBehaviorVariance);

        //manually assign vector visualization colors
        aVLRend = alignmentVectorLine.GetComponent<LineRenderer>();
        cVLRend = cohesionVectorLine.GetComponent<LineRenderer>();
        sVLRend = seperationVectorLine.GetComponent<LineRenderer>();

        aVLRend.material.SetColor("_Color", new Color(0, 1, 0, 0.2f));
        cVLRend.material.SetColor("_Color", new Color(1, 0, 0, 0.2f));
        sVLRend.material.SetColor("_Color", new Color(0, 0, 1, 0.2f));

        //Initialize willingness to change
        seekRate = Random.Range(0.0f, 1.0f);
        if (seekRate < 0.1f)
        { 
            seekRate = 0;
        }
        //Initialize a random mass & size based on mass
        float randomModifier = Random.Range(-0.25f, 0.25f);
        rb.mass += randomModifier * 20;
        transform.localScale += new Vector3(randomModifier, randomModifier, randomModifier);

        

        //Generate an a random initial direction to move in.
        forceDirection = GetRandomVectorWithRange(0, 0);

        //Randomize metallic and glossiness
        rend.material.SetFloat("_Metallic", Random.Range(0f, 1f));
        rend.material.SetFloat("_Glossiness", Random.Range(0f, 1f));
    }

    // Update is called once per frame prefer visual updates
    void Update()
    {
        SetBehaviorColor();
        ConformWithNeighbors();
        SetVisualizationMeshes();

        


        //Sort Nearby Flockers by distance
        nearbyFlockers = nearbyFlockers.OrderBy(nearbyFlockers => Vector3.Distance(transform.position, nearbyFlockers.gameObject.transform.position)).ToList();
    }
    // FixedUpdate to prefer physics updates
    void FixedUpdate()
    {
        
        //Modify our current direction vector by a random amount scaled by forceVariance and then normalize it (so as not to eventually overflow the vector3)
        forceDirection += GetRandomVectorWithRange(-forceVariance, forceVariance);
        forceDirection.Normalize();
        rb.AddForce(forceDirection * forceModifier, ForceMode.Force);
        rb.AddForce(cohesionVector * flockBehaviorMod.x * forceModifier, ForceMode.Force);
        rb.AddForce(alignmentVector * flockBehaviorMod.y * forceModifier, ForceMode.Force);
        rb.AddForce(seperationVector * flockBehaviorMod.z * forceModifier, ForceMode.Force);

        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    //Return a randomized Vector 3 within the cube-ish parameters
    Vector3 GetRandomVectorWithRange(float x, float y)
    {
        return new Vector3(Random.Range(x, y), Random.Range(x, y), Random.Range(x, y));
    }

    public void SetBehavior(Color color)
    {
        flockBehaviorMod = new Vector3(color.r + flockBehaviorVariance, color.g + flockBehaviorVariance, color.b + flockBehaviorVariance);
    }

    public void SetVisualizationMeshes()
    {
        aVLRend.SetPosition(0, transform.position);
        aVLRend.SetPosition(1, (alignmentVector.normalized * flockBehaviorMod.y * 10) + transform.position);
        aVLRend.startWidth = flockBehaviorMod.y;

        cVLRend.SetPosition(0, transform.position);
        cVLRend.SetPosition(1, (cohesionVector.normalized * flockBehaviorMod.x * 10) + transform.position);
        cVLRend.startWidth = flockBehaviorMod.x;

        sVLRend.SetPosition(0, transform.position);
        sVLRend.SetPosition(1, (seperationVector.normalized * flockBehaviorMod.z * 10) + transform.position);
        sVLRend.startWidth = flockBehaviorMod.z;
    }

    //Function to update flocker's color based on current aiBehavior
    //RGB values here are 0 to 1 floats and we use our flockBehaviorMod which should be 0 to 1 floats.
    public void SetBehaviorColor()
    {
        Color behavColor = new Color(flockBehaviorMod.x - flockBehaviorVariance, flockBehaviorMod.y - flockBehaviorVariance, flockBehaviorMod.z - flockBehaviorVariance, 1);
        rend.material.SetColor("_Color", behavColor);
    }

    //Function to toggle the viewable metric meshes, aka if we can see the view distances of each flocker
    public void SetViewDistanceViewable(bool tempBool)
    {
        viewMesh.SetActive(tempBool);
    }

    public void SetBehaviorViewable(bool tempBool)
    {
        alignmentVectorLine.SetActive(tempBool);
        cohesionVectorLine.SetActive(tempBool);
        seperationVectorLine.SetActive(tempBool);
    }

    //Look at adjacent flockers and average their flocking mod vector with ours
    private void ConformWithNeighbors()
    {
        Vector3 otherBehaviorMod = flockBehaviorMod; //Initialize as our own behavior mod to start the average
        Vector3 averageOtherDirection = new Vector3(0, 0, 0); //Initialize as our own velocity to start the average
        Vector3 averageVectorTooFar = Vector3.zero; //vector to accumulate average which points toward all too far away neighbors
        Vector3 averageVectorTooClose = Vector3.zero; //vector to accumulate average which points away from all too close neighbors
        //Look at all appropriate neighbers and accumulate averages of all the information we need from each
        //we should end up receiving an alignmentVector, cohesionVector, and seperationVector;
        //Instead of each flocker in nearby, look at only first 6, should be closest 6 if we have sorted them properly by distance to us.
        for (int i = 0; i < 6 && i < nearbyFlockers.Count; i++)
        {
            otherBehaviorMod = (otherBehaviorMod + nearbyFlockers[i].GetComponent<Flocker>().flockBehaviorMod) * 0.5f;
            averageOtherDirection = (averageOtherDirection + nearbyFlockers[i].GetComponent<Flocker>().rb.velocity) * 0.5f;
            averageVectorTooFar = (nearbyFlockers[i].transform.position - transform.position) * 0.5f;
            if (Vector3.Distance(nearbyFlockers[i].transform.position, transform.position) < desiredSeperation)
            {
                averageVectorTooClose += transform.position - nearbyFlockers[i].transform.position;
            }
        }
        cohesionVector = averageVectorTooFar;
        alignmentVector = averageOtherDirection;
        seperationVector = averageVectorTooClose;

        //Tend towards neighbor's behavior modifiers at seekrate
        flockBehaviorMod = Vector3.Slerp(flockBehaviorMod, otherBehaviorMod, Time.deltaTime * seekRate);
    }

    private void GetFood()
    {

    }





    /*How about we get the 3 closest neighbors within a seek distance
     * what do we need from these three neighbors?
     * - average position of the 3 in 3d space. this should be
     *  STOP, Implement first: get above info and move towards average position
     * 
     */
}
