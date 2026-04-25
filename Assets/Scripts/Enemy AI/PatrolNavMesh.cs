using UnityEngine;
using UnityEngine.AI;
public class PatrolNavMesh : MonoBehaviour
{
    [Header("Waypoint Parent")]
    public Transform waypointParent;

    [Header("Settings")]
    public bool usePingPong = true;

    private Transform[] waypoints;
    private NavMeshAgent agent;
    private int currentIndex = 0;
    private bool goingForward = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        int count = waypointParent.childCount;
        waypoints = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            waypoints[i] = waypointParent.GetChild(i);
        }

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentIndex].position);
        }
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoToNextWaypoint();
        }
    }

    void GoToNextWaypoint()
    {
        if (usePingPong)
        {
            if (goingForward)
            {
                currentIndex++;

                if (currentIndex >= waypoints.Length - 1)
                {
                    currentIndex = waypoints.Length - 1;
                    goingForward = false;
                }
            }
            else
            {
                currentIndex--;

                if (currentIndex <= 0)
                {
                    currentIndex = 0;
                    goingForward = true;
                }
            }
        }
        else
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }

        agent.SetDestination(waypoints[currentIndex].position);
    }

    public void Stop()
    {
        agent.isStopped = true;
    }

    public void Resume()
    {
        agent.isStopped = false;
        agent.SetDestination(waypoints[currentIndex].position);
    }
}