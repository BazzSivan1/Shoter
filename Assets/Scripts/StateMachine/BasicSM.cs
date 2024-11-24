using UnityEngine;
using UnityEngine.AI;

public class BasicSM : MonoBehaviour
{
    [SerializeField] private float patrolRange;
    [SerializeField] private float detectionRange = 15;
    [SerializeField] private Transform prepoint;
    [SerializeField] private Transform point;

    private enum State
    {
        patrol,
        chaseTarget
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
        target = GameObject.FindGameObjectWithTag("Player");
        state = State.patrol;
        patrolPosition = GetPatrolPosition();
    }

    private void Update()
    {
        Debug.Log(state);
        switch(state)
        {
            default:
            case State.patrol:
                agent.SetDestination(patrolPosition);

                if (agent.remainingDistance < 0.5f)
                    patrolPosition = GetPatrolPosition();

                FindTarget();
                break;

            case State.chaseTarget:
                agent.SetDestination(target.transform.position);
                if (Vector3.Distance(transform.position, target.transform.position) > detectionRange)
                {
                    //agent.SetDestination(initialPosition);
                    state = State.patrol;
                }
                    
                break;
        }
    }

    private Vector3 GetPatrolPosition()
    {
        Vector3 pointToPatrol = initialPosition;
        Vector3 proposedPoint = initialPosition + Random.insideUnitSphere * patrolRange;

        prepoint.position = proposedPoint;

        if (NavMesh.SamplePosition(proposedPoint, out NavMeshHit navMeshHit, 5f, NavMesh.AllAreas)){

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
        {
            state = State.chaseTarget;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(initialPosition, patrolRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}