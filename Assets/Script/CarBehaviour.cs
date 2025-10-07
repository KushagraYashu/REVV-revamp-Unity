using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    public bool handBrake;
}

public class CarBehaviour : MonoBehaviour
{
    private float motorVal;

    //[SerializeField] Rigidbody carRb;

    public bool handBrake = false;
    public bool reverse = false;

    public List<AxleInfo> axleInfos;
    public float downforceValue;
    public float maxMotorTorque;
    public float maxBrakeTorque;
    
    public float maxSteeringAngle;
    public float topSpeed;

    public ParticleSystem[] exhaustEffect;
    public float lifeMax;
    private float initialRate;
    private float initialLife;

    public GameObject[] headLights;

    public GameObject[] tailLights;

    public float topRig;
    public float midRig;
    public float bottomRig;

    public TextMeshProUGUI speedTxt;


    public void Start()
    {
        initialRate = exhaustEffect[0].emissionRate;
        initialLife = exhaustEffect[0].startLifetime;
        speedTxt = GameObject.FindGameObjectWithTag("SpeedTxt").GetComponent<TextMeshProUGUI>();
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach(GameObject go in headLights)
            {
                go.SetActive(!go.activeSelf);
            }
        }

        if (this.handBrake || Input.GetKey(KeyCode.S))
        {
            foreach (GameObject go in tailLights)
            {
                go.SetActive(true);
            }
        }

        else
        {
            foreach (GameObject go in tailLights)
            {
                go.SetActive(false);
            }
        }
    }

    public void FixedUpdate()
    {
        var s = this.gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f; //mph to kph
        speedTxt.text = s.ToString("F1");
        if (s > 30)
        {
            foreach(ParticleSystem p in exhaustEffect)
            {
                p.emissionRate = s / 10;
                p.startLifetime = Mathf.Min(s/100, lifeMax);
                
            }
        }
        else
        {
            foreach (ParticleSystem p in exhaustEffect)
            {
                p.emissionRate = initialRate;
                p.startLifetime = initialLife;
                
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            this.handBrake = true;
        }
        else 
        {
            this.handBrake = false;
        }

        

        motorVal = 0;
        if(Mathf.Abs(Input.GetAxis("Vertical")) > .5f)
        {
            motorVal = maxMotorTorque * Input.GetAxis("Vertical");
        }
        
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor && !this.handBrake)
            {
                Debug.Log(axleInfo.leftWheel.rpm);
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;

                if (motorVal == 0)
                {
                    axleInfo.leftWheel.motorTorque = 0;
                    axleInfo.rightWheel.motorTorque = 0;
                }

                else if(motorVal > 0)
                {
                    reverse = false;

                    axleInfo.leftWheel.motorTorque = (motorVal / 2) ;
                    axleInfo.rightWheel.motorTorque = (motorVal / 2) ;
                }

                else if(motorVal < 0 && (axleInfo.leftWheel.rpm > 20f || axleInfo.rightWheel.rpm > 20f))
                {
                    reverse = false;
                    axleInfo.leftWheel.motorTorque = 0;
                    axleInfo.rightWheel.motorTorque = 0;
                    axleInfo.leftWheel.brakeTorque = maxBrakeTorque;
                    axleInfo.rightWheel.brakeTorque = maxBrakeTorque;
                }
                else
                {
                    reverse = true;
                }
                
                if(motorVal < 0 && this.reverse)
                {
                    axleInfo.leftWheel.brakeTorque = 0;
                    axleInfo.rightWheel.brakeTorque = 0;
                    axleInfo.leftWheel.motorTorque = (motorVal / 2);
                    axleInfo.rightWheel.motorTorque = (motorVal / 2);
                }
            }

            if(axleInfo.motor && this.handBrake)
            {
                axleInfo.leftWheel.motorTorque = 0;
                axleInfo.rightWheel.motorTorque = 0;
            }
            if(axleInfo.handBrake && this.handBrake)
            {
               /* axleInfo.leftWheel.motorTorque = Mathf.Max(0, axleInfo.leftWheel.motorTorque/500);
                axleInfo.rightWheel.motorTorque = Mathf.Max(0, axleInfo.leftWheel.motorTorque / 500);*/
                axleInfo.leftWheel.brakeTorque = 5000;
                axleInfo.rightWheel.brakeTorque = 5000;
            }
            if(!this.handBrake && axleInfo.handBrake)
            {
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
            }
            if(s > topSpeed)
            {
                axleInfo.leftWheel.motorTorque = 0;
                axleInfo.rightWheel.motorTorque = 0;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }

        ApplyDownforce();
    }

    void ApplyDownforce()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(downforceValue * this.gameObject.GetComponent<Rigidbody>().velocity.magnitude * -transform.up);
    }
}