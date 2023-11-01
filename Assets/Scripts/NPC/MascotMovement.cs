using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class MascotMovement : MonoBehaviour
{
    NavMeshAgent nm;
    Rigidbody rb;
    Animator anim;

    // To follow individual
    public Transform Target;

    // To follow multiple points
    public Transform[] WayPoints;

    public int Cur_WayPoint;
    public float speed, stop_distance;
    public float PauseTimer;
    [SerializeField]
    private float cur_timer;
    Vector3 velocity;


    void Start()
    {
        // Initialize variables:
        // nm: the NavMeshAgent (to move the NPC)
        // rb: the Rigidbody (to freeze rotation)
        // anim: the Animator (to play animations)

        nm = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;

        // Initialize target to first waypoint
        Target = WayPoints[Cur_WayPoint];

        // Timer to pause at each waypoint
        cur_timer = PauseTimer;
    }


    void Update()
    {
        nm.acceleration = speed;
        nm.stoppingDistance = stop_distance;

        float distance = Vector3.Distance(transform.position, Target.position);


        // If the NPC is close enough to the target, proceed to the next waypoint
        if (distance > stop_distance && WayPoints.Length > 0)
        {
            Target = WayPoints[Cur_WayPoint];
        }
        // Otherwise, pause at the current waypoint
        else if (distance <= stop_distance && WayPoints.Length > 0)
        {
            if (cur_timer > 0)
            {
                cur_timer -= 0.01f;
            }
            else if (cur_timer <= 0)
            {
                // Once the timer is up, proceed to the next waypoint
                Cur_WayPoint++;

                if (Cur_WayPoint >= WayPoints.Length)
                {
                    // After the final waypoint, return to the first waypoint
                    Cur_WayPoint = 0;
                }
                
                // Re-set target to next waypoint
                Target = WayPoints[Cur_WayPoint];

                // Reset timer
                cur_timer = PauseTimer;
            }
        }

        // Set animation depending of NPC's idle status
        float Velocity = nm.velocity.magnitude;
        anim.SetFloat("Velocity", Velocity);
        if (Velocity == 0)
            anim.SetBool("Idle", true);
        else
            anim.SetBool("Idle", false);

        nm.SetDestination(Target.position);
    }
}
