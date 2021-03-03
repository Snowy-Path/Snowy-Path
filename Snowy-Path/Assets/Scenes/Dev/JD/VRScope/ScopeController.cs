using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeController : MonoBehaviour {
    public float rotSpeed = 1;
    public float camRotfactor = 1;
    public float camRotMax = 90;


    public Transform cameraTransform;
    public Transform playerCam;
    public Transform lens;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        MoveScope();
        RotateCam();
        RotateLens();
    }

    private void MoveScope() {
        Vector3 rot = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow)) {
            rot.y++;
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            rot.y--;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            rot.x++;
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            rot.x--;
        }

        transform.Rotate(rot * rotSpeed * Time.deltaTime);
    }

    private void RotateCam() {
        Vector3 finalRot = playerCam.rotation.eulerAngles;
        //finalRot.x = Mathf.Clamp(finalRot.x * camRotfactor, -camRotMax, camRotMax);
        //finalRot.y = Mathf.Clamp(finalRot.y * camRotfactor, -camRotMax, camRotMax);
        //finalRot.z = Mathf.Clamp(finalRot.z * camRotfactor, -camRotMax, camRotMax);

        cameraTransform.eulerAngles = finalRot;
    }

    private void RotateLens() {
        

        Vector3 finalRot = lens.localEulerAngles;
        finalRot.z = 0;

        //lens.localEulerAngles = finalRot;
    }
}
