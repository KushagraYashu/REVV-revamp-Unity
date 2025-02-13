using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CamBehaviourMainMenu : MonoBehaviour
{
    public CinemachineFreeLook cmFreeLook;

    public bool lockState;

    public Transform[] carParents;

    int i = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !lockState)
        {
            Cursor.lockState = CursorLockMode.Locked;
            lockState = true;
        }
        else if (Input.GetButtonDown("Jump") && lockState)
        {
            Cursor.lockState = CursorLockMode.Confined;
            lockState = false;
        }

    }

    public void SwitchCarAhead()
    {
        
        if(i<carParents.Length-1)
        {
            i++;
            cmFreeLook.LookAt = carParents[i];
            cmFreeLook.Follow = carParents[i];
            
        }
        else
        {
            i = 0;
            cmFreeLook.LookAt = carParents[i];
            cmFreeLook.Follow = carParents[i];
        }
    }
    public void SwitchCarBack()
    {
        
        if(i>0)
        {
            i--;
            cmFreeLook.LookAt = carParents[i];
            cmFreeLook.Follow = carParents[i];
            
        }
        else
        {
            i = carParents.Length-1;
            cmFreeLook.LookAt = carParents[i];
            cmFreeLook.Follow = carParents[i];
        }
    }

}
