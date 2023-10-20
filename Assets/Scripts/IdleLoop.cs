using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleLoop : MonoBehaviour
{
    // adjust this to change movement speed
    float speed = 5f;

    // adjust this to change how high the object can jump
    float height = 0.3f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // get the objects current position and put it in a variable so we can access it later with less code
        Vector3 pos = transform.localPosition;

        // calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed) + 12;

        // set the object's Y to the new calculated Y
        transform.localPosition = new Vector3(pos.x, newY * height, pos.z);
    }
}
