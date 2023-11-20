using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    Drive ds;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        ds = target.GetComponent<Drive>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }


    void Pursue()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;

        float relativeHeading = Vector3.Angle(this.transform.forward, this.transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(this.transform.forward, this.transform.TransformVector(targetDir));

        if(toTarget > 90 && relativeHeading > 20 || ds.currentSpeed < 0.01f)
        {
            Seek(target.transform.position);
            return;
        }


        float lookAhead = targetDir.magnitude/(agent.speed + ds.currentSpeed);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    void Evade()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float lookAhead = targetDir.magnitude / (agent.speed + ds.currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead);



    }

    Vector3 wanderTarget = Vector3.zero;

    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 20;
        float waderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * waderJitter, 0, Random.Range(-1.0f, 1.0f) * waderJitter);

        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targerLocal = wanderTarget+ new Vector3(0,0,wanderDistance);

        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targerLocal);

        Seek(targetWorld);

    }

    void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for(int i = 0; i < World.Instace.GetHiddingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instace.GetHiddingSpots()[i].transform.position- target.transform.position;
            Vector3 hidePos = World.Instace.GetHiddingSpots()[i].transform.position + hideDir.normalized * 10;


            if(Vector3.Distance(target.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(target.transform.position, hidePos);


            }

        }

        Seek(chosenSpot);
    }

    // Update is called once per frame
    void Update()
    {
        Hide();
    }
}
