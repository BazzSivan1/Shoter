using UnityEngine;
using UnityEngine.AI;

public class DemoSM : MonoBehaviour
{
    [SerializeField] private float patrolRange = 20;
    [SerializeField] private float detectionRange = 15;
    [SerializeField] private Transform prepoint;
    [SerializeField] private Transform point;

    private enum State
    {
        Patrol,
        ChaseTarget
    }

    private State state;

    private GameObject target;
    
    private NavMeshAgent agent;

    private Vector3 initialPosition;
    private Vector3 patrolPosition;

    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        state = State.Patrol;
        target = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
        patrolPosition = GetPatrolPosition();
    }

    private void Update()
    {
        Debug.Log(state);
        switch(state)
        {
            default:
            case State.Patrol:
                agent.SetDestination(patrolPosition);
                if (agent.remainingDistance < 0.5f)
                    patrolPosition = GetPatrolPosition();

                FindTarget();
                break;

            case State.ChaseTarget:
                agent.SetDestination(target.transform.position);

                if (Vector3.Distance(transform.position, target.transform.position) > detectionRange)
                    state = State.Patrol;
                break;
        }
    }

    private Vector3 GetPatrolPosition()
    {
        Vector3 pointToPatrol = initialPosition;
        Vector3 proposedPoint = initialPosition + Random.insideUnitSphere * patrolRange;

        prepoint.position = proposedPoint;

        if (NavMesh.SamplePosition(proposedPoint, out NavMeshHit navMeshHit, 10f, NavMesh.AllAreas)){

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(navMeshHit.position, path);
            bool canReachPoint = path.status == NavMeshPathStatus.PathComplete;

            if (canReachPoint)
                pointToPatrol = navMeshHit.position;
        }

        point.position = pointToPatrol;
        return pointToPatrol;
    }

    private void FindTarget()
    {
        if (target == null)
            return;

        if (Vector3.Distance(transform.position, target.transform.position) <= detectionRange)
            state = State.ChaseTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}