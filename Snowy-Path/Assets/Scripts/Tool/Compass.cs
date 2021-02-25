using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    private SphereCollider jammingcollider;
    public Transform NorthPole; //Position of north pole
    public Transform Needle;
    public float jammingspeed = 20.0f;
    public float JammingRange;

    private float jammingchangetime = 1f;
    private float jammingtimer = 0f;
    private bool isjamming;
    private float number;


    private void Start()
    {
        jammingcollider = GetComponent<SphereCollider>();
        jammingcollider.radius = JammingRange;
    }
    // Update is called once per frame
    void Update()
    {

        //If no ennemy in range
        if (isjamming == false)
        {
            //Classic behavior
            jammingtimer = 0;
            ChangeNorthDirection();
        }

        else
        {
            //Generate random number every time jammingtimer reach jammingchangetime 
            jammingtimer += Time.deltaTime;
            if (jammingtimer >= jammingchangetime)
            {
                jammingtimer = 0;
                NumberGen();
            }
            //Jamming behavior
            Jamming();
        }

    }
    /// <summary>
    /// Point the needle at north direction 
    /// </summary>
    public void ChangeNorthDirection()
    {

        {
            Needle.LookAt(NorthPole);
            //Disable rotation out of the compass housing
            Needle.localEulerAngles = new Vector3(0, Needle.localEulerAngles.y, 0);

        }

    }

    /// <summary>
    /// If an ennemy collide with the compass collider, set isjamming
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ennemy"))
        {
            isjamming = true;

        }

    }
    /// <summary>
    ///  If an ennemy stop colliding with the compass collider, unset isjamming
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ennemy"))
        {

            isjamming = false;
            //Smoothing transition
            Needle.localEulerAngles=Vector3.Slerp(Needle.localEulerAngles, new Vector3(0, Needle.localEulerAngles.y, 0), Time.deltaTime * jammingspeed);

        }
        

    }

    /// <summary>
    /// Jamming Behavior, the needle turns on itself and changes direction of rotation according to the value of number
    /// 
    /// </summary>
    public void Jamming()
    {
        if (number > 0)
            Needle.localRotation *= Quaternion.Euler(0, jammingspeed, 0);
        else
            Needle.localRotation *= Quaternion.Inverse(Quaternion.Euler(0, jammingspeed, 0));
    }

    /// <summary>
    /// Generate random number
    /// </summary>
    private void NumberGen()
    {
        number = (Random.Range(0, 2));

    }

    /// <summary>
    /// Displaying NorthPole
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(NorthPole.position, new Vector3(0.5f, 4f, 0.5f));
    }

}




