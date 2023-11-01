using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClip : MonoBehaviour
{
    public float maxDistance = 10f; // the maximum distance the camera can be from the player

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            // a nearby obstacle was detected, adjust the camera's position or movement here
            // get the distance from the camera to the obstacle
            float distanceToHit = Vector3.Distance(transform.position, hit.point);

            // calculate a new position for the camera that is away from the obstacle
            Vector3 newPosition = transform.position + transform.forward * (distanceToHit - 0.1f);

            // move the camera to the new position
            transform.position = newPosition;
        }
    }
}
