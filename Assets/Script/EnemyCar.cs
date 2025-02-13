using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCar : MonoBehaviour
{
    public float downforceValue;
    public float maxMotorTorque; //should be changed based on car behaviour on the track
    public float maxBrakeTorque; //should be changed based on car behaviour on the track
    public float minBrakeTorque; //It is the small braking amount, should be changed based on car behaviour on the track
    public float maxSpeed;
    [SerializeField] float curSpeed;
    public float maxSteer;
    public WheelCollider FL;
    public WheelCollider FR;
    public WheelCollider[] drivingColliders;
    public WheelCollider[] brakingColliders;
    
    public Path path;
    public List<Transform> nodes;
    public int curNode = 0;

    float newSteer;
    
    // Start is called before the first frame update
    void Start()
    {
        nodes = new List<Transform>();
        nodes = path.nodes;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWaypointDist();
    }

    void ApplySteer()
    {
        Vector3 relVector = transform.InverseTransformPoint(nodes[curNode].position);
        newSteer = (relVector.x / relVector.magnitude) * maxSteer;
        FL.steerAngle = newSteer;
        FR.steerAngle = newSteer;
    }

    void Drive()
    {
        this.GetComponent<Rigidbody>().AddForce(downforceValue * this.gameObject.GetComponent<Rigidbody>().velocity.magnitude * -transform.up);

        curSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;

        if (curSpeed < maxSpeed && FL.steerAngle < maxSteer/3)
        {
            foreach (WheelCollider coll in drivingColliders)
            {
                coll.motorTorque = maxMotorTorque * Mathf.Clamp(Vector3.Distance(transform.position, nodes[curNode].position)/10, .4f, 1);
                if(coll.motorTorque == maxMotorTorque * .4f)
                {
                    Debug.LogWarning(coll.motorTorque);
                }
            }
            foreach (WheelCollider coll in brakingColliders)
            {
                coll.brakeTorque = 0f;
            }
        }
        else
        {
            foreach (WheelCollider coll in drivingColliders)
            {
                coll.motorTorque = 0f;
            }
            foreach (WheelCollider coll in brakingColliders)
            {
                coll.brakeTorque = maxBrakeTorque;
            }
            
        }
    }

    void CheckWaypointDist()
    {
        if(Vector3.Distance(transform.position, nodes[curNode].position) < 3f) //change this dist, based on track and car behaviour near the checkpoint/node.
        {
            if(curNode == nodes.Count - 1)
            {
                curNode = 0;
            }
            else
            {
                curNode++;
            }
        }else if(Vector3.Distance(transform.position, nodes[curNode].position) < 4f && curNode != 0)
        {
            foreach (WheelCollider coll in drivingColliders)
            {
                coll.motorTorque = 0f;
            }
            foreach (WheelCollider coll in brakingColliders)
            {
                coll.brakeTorque = minBrakeTorque + (maxBrakeTorque * (1/Vector3.Distance(transform.position, nodes[curNode].position)));
            }
            Debug.Log("Brake");
        }
        else
        {
            foreach (WheelCollider coll in brakingColliders)
            {
                coll.brakeTorque = 0;
            }
        }
    }

}
