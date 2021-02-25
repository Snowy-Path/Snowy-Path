using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public SphereCollider collider;
    public Transform NorthPole;
    public Transform Needle;
    public float speed = 20.0f;
    public float JammingRange;

    private float jammingchangetime = 1f;
    private float jammingtimer = 0f;
    private bool isjaming;
    private float number;


    private void Start()
    {
        collider = GetComponent<SphereCollider>();
        collider.radius = JammingRange;
    }
    // Update is called once per frame
    void Update()
    {


        if (isjaming == false)
        {
            jammingtimer = 0;
            ChangeNorthDirection();
        }

        else
        {
            jammingtimer += Time.deltaTime;
            if (jammingtimer >= jammingchangetime)
            {
                jammingtimer = 0;
                NumberGen();
            }
            Jamming();
        }

    }
    public void ChangeNorthDirection()
    {

        {
            //Vector3 lookPos = target.position - transform.position;
            Needle.LookAt(NorthPole);
            Needle.localEulerAngles = new Vector3(0, Needle.localEulerAngles.y, 0);
            //Vector3 lookPos = NorthLayer.position - transform.position;
            //transform.up = Vector3.Slerp(transform.up, lookPos, Time.deltaTime * speed);

        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ennemy"))
        {
            isjaming = true;

        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ennemy"))
        {

            isjaming = false;
            Needle.localPosition=Vector3.Slerp(Needle.localEulerAngles, new Vector3(0, Needle.localEulerAngles.y, 0), Time.deltaTime * speed);

        }
        

    }

    public void Jamming()
    {
        if (number > 0)
            Needle.localRotation *= Quaternion.Euler(0, speed, 0);
        else
            Needle.localRotation *= Quaternion.Inverse(Quaternion.Euler(0, speed, 0));
    }

    private void NumberGen()
    {
        number = (Random.Range(0, 2));

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(NorthPole.position, new Vector3(0.5f, 4f, 0.5f));
    }

}




