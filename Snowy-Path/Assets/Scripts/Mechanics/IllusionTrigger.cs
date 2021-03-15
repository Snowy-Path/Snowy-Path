using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionTrigger : MonoBehaviour {

    [Range(1, 90)]
    [SerializeField] float activationAngle;
    [SerializeField] GameObject renderedObject;
    [SerializeField] Camera camera;

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            float angle = Vector3.Angle(other.transform.forward, transform.right);
            renderedObject.SetActive(angle <= activationAngle);
            camera.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        renderedObject.SetActive(false);
        camera.gameObject.SetActive(false);
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector3 point1 = new Vector3(Mathf.Cos(Mathf.Deg2Rad * activationAngle), 0, Mathf.Sin(Mathf.Deg2Rad * activationAngle));
        Vector3 point2 = new Vector3(Mathf.Cos(Mathf.Deg2Rad * -activationAngle), 0, Mathf.Sin(Mathf.Deg2Rad * -activationAngle));

        point1 = transform.TransformDirection(point1);
        point2 = transform.TransformDirection(point2);

        Gizmos.DrawLine(transform.position, transform.position + point1 * 5);
        Gizmos.DrawLine(transform.position, transform.position + point2 * 5);
    }
}
