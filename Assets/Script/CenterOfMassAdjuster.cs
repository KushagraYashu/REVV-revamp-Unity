using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMassAdjuster : MonoBehaviour
{

    [SerializeField] Rigidbody carRb;
    private bool awake;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        carRb.centerOfMass =  this.transform.localPosition;
        carRb.WakeUp();
        awake = !carRb.IsSleeping();
    }
}
