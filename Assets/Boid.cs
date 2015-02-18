using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boid : MonoBehaviour {

    public Vector3 seekTarget;

    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 force;
    public float mass;
    public float maxSpeed;

    public GameObject pursueTarget;

    public bool seekEnabled;
    public bool pursueEnabled;
    public bool pathfollowEnabled;
    public bool arriveEnabled;
    public path path1;
    Vector3 offset;
    List<Vector3> waypoints;
    int currentWaypoint;

    Vector3 OffSetPursue(GameObject offsetTarget) 
    {
        Vector3 targetPos = offsetTarget.transform.TransformPoint(offset);
        Vector3 toTArget = targetPos - transform.position;
        float distance = toTArget.magnitude;
        float time = distance / maxSpeed;
        Vector3  target = offsetTarget.transform.position + offsetTarget.GetComponent<Boid>().velocity * time;
        return target;
    }

    Vector3 arrive(Vector3 arivepoint) {
        Vector3 totarget = arivepoint - transform.position;
        float distance = totarget.magnitude;

        float slowingDistance =10;
        float ramped = sigMoid(distance)*maxSpeed;//distance / slowingDistance * maxSpeed;
        float clamped = Mathf.Min(ramped,maxSpeed);
        Vector3 desired = (totarget/distance) *clamped;
        return(desired);

    }

    
    
    public Boid()
    {
        mass = 1;
        velocity = Vector3.zero;
        force = Vector3.zero;
        acceleration = Vector3.zero;
        maxSpeed = 10.0f;
        currentWaypoint = 0;
        waypoints = new List<Vector3>();
        offset = new Vector3(5, 5, 5);
        path1 = new path();

        waypoints.Add(new Vector3(0,0,0));
        waypoints.Add(new Vector3(0, 50, 0));
        waypoints.Add(new Vector3(0, 0, 100));

        waypoints.Add(new Vector3(100, 0, 100));
        waypoints.Add(new Vector3(50, 0, 100));
    }

	// Use this for initialization
	void Start () {
	
	}

    float sigMoid(float input) 
    {
        return(2/(1+Mathf.Exp(-2*input))-1);
    }

    

    Vector3 FollowPath() {
        // TODO finish writeing thing, currently returns a zero so the program will complile
        Vector3 next = path1.NextWaypoint();
        float dist = (transform.position - next).magnitude;
        return(Vector3.zero);
    }


    Vector3 Seek(Vector3 seekTarget)
    {
        Vector3 desired = seekTarget - transform.position;
        desired.Normalize();
        desired *= maxSpeed;
        return desired - velocity;
    }

    Vector3 SeekWayPoint()
    {
        Vector3 totarget = waypoints[currentWaypoint] - transform.position;
     
        float distance = totarget.magnitude;
        Debug.Log(distance);
        if (distance < 10){
            if (currentWaypoint < (waypoints.Count -1))
            {
                currentWaypoint++;
            }
        }
        totarget.Normalize();
        totarget *= maxSpeed;
        return totarget - velocity;
    }




    Vector3 pursue(GameObject pursueTarget)
    {
        Vector3 toTarget = pursueTarget.transform.position - transform.position;
        float distance = toTarget.magnitude;

        float time = distance / maxSpeed;
        Vector3 target =
            pursueTarget.transform.position +
            pursueTarget.GetComponent<Boid>().velocity * time;
        Debug.DrawLine(target, target + Vector3.forward);
        return Seek(target);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(seekTarget, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (pursueEnabled)
        {
            force += pursue(pursueTarget);
        }
        if (seekEnabled)
        {
            force += Seek(seekTarget);
        }
     
        if(pathfollowEnabled)
        {
            Vector3 toTarget = waypoints[currentWaypoint]- transform.position;
            force += SeekWayPoint();
        }
        
        acceleration =  force / mass;
        velocity += acceleration * Time.deltaTime;
        Vector3.ClampMagnitude(velocity, maxSpeed);
        
        transform.position += velocity * Time.deltaTime;

        if (velocity.magnitude > float.Epsilon)
        {
            transform.forward = velocity.normalized;
        }

        force = Vector3.zero;
	}
}
