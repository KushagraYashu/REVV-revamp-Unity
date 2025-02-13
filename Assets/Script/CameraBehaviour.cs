using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook cmFreeLook;

    
    // Start is called before the first frame update
    void Start()
    {
        cmFreeLook.LookAt = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        cmFreeLook.Follow = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        cmFreeLook.m_Orbits = new CinemachineFreeLook.Orbit[]
        {
            new CinemachineFreeLook.Orbit(GameObject.FindGameObjectWithTag("Player").GetComponent<CarBehaviour>().topRig, 3.3f),
            new CinemachineFreeLook.Orbit(GameObject.FindGameObjectWithTag("Player").GetComponent<CarBehaviour>().midRig, 7.7f),
            new CinemachineFreeLook.Orbit(GameObject.FindGameObjectWithTag("Player").GetComponent<CarBehaviour>().bottomRig, 3.3f),
        };
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
