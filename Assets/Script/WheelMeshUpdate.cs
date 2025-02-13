using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMeshUpdate : MonoBehaviour
{
    public WheelCollider[] colliders;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (var collider in colliders)
        {
            ApplyLocalPositionToVisuals(collider);
        }
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);


        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

}
