using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{

    //public Vector3 NorthDirection;
    //public Transform Player;
    //public Quaternion MissionDirection;
    public float JammingRange;
    public Transform NorthLayer;
    public Transform target;
    private bool isjaming;
    public float speed = 1.0f;
    

    // Update is called once per frame
    void Update()
    {
        if (isjaming == false)
            ChangeNorthDirection();
        else
            Jamming();
    }
    public void ChangeNorthDirection()
    {
        
        {
            Vector3 lookPos = target.position - transform.position;
            //Vector3 lookPos = target.position - NorthLayer.position;
            transform.up = Vector3.Slerp(transform.up, lookPos, Time.deltaTime * speed);

        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ennemy"))
        {
            Debug.Log("CA JAM");
            isjaming = true;

        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ennemy"))
        {
            Debug.Log("CA JAM PLUS");
            isjaming = false;
        }
        
    }

    public void Jamming()
    {
        transform.localRotation*= Quaternion.Euler(0, 10, 0);
    }
}




