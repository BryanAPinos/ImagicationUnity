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
    public Transform Target; //To follow individual
    public Transform[] WayPoints; //To follow multiple points
    public int Cur_WayPoint;
    public float speed, stop_distance;
    public float PauseTimer;
    [SerializeField]
    private float cur_timer;
    Vector3 velocity;


    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
        Target = WayPoints[Cur_WayPoint];
        cur_timer = PauseTimer;



    }


    void Update()
    {
        nm.acceleration = speed;
        nm.stoppingDistance = stop_distance;
        float Velocity = nm.velocity.magnitude;

        float distance = Vector3.Distance(transform.position, Target.position);

        if(distance > stop_distance && WayPoints.Length > 0)
        {
            Target = WayPoints[Cur_WayPoint];
        }
        else if (distance <= stop_distance && WayPoints.Length > 0)
        {
            if(cur_timer > 0)
            {
                cur_timer -= 0.01f;
            }
            if (cur_timer <= 0)
            {
                Cur_WayPoint++;
                if (Cur_WayPoint >= WayPoints.Length)
                {
                    Cur_WayPoint = 0;
                }
                Target = WayPoints[Cur_WayPoint];
                cur_timer = PauseTimer;
            }
        }
        anim.SetFloat("Velocity", Velocity);
        if(Velocity == 0)
            anim.SetBool("Idle", true);
        else
            anim.SetBool("Idle", false);
        nm.SetDestination(Target.position);
    }
}
