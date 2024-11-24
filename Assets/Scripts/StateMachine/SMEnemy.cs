using UnityEngine;
using UnityEngine.AI;

public class SMEnemy : MonoBehaviour
{
    [SerializeField] private float patrolMaxRange = 15;
    [SerializeField] private float patrolMinRange = 5;
    [SerializeField] private float targetDetectionRange = 20;
    [SerializeField] private float atackRange = 10;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform prepoint;
    [SerializeField] private Transform point;
    [SerializeField] private Transform visorPoint;

    private enum State
    {
        Patroling,
        ChaseTarget,
        AtackTarget,
        ToInitialPosition
    }

    private State state;

    private NavMeshAgent agent;
    private Vector3 initialPosition;
    private Vector3 patrolPosition;

    private LineRenderer line;

    private GameObject target;

    private float nextTimeToShoot = 0;
    private float shootRate = 2;

    private void Awake()
    {
        line = GetComponentInChildren<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        state = State.Patroling;
        target = GameObject.FindGameObjectWithTag("Player");
        initialPosition = transform.position;
        patrolPosition = GetPatrolPosition();
    }

    private void Update()
    {
        Debug.Log(state);
        switch (state)
        {
            default:
            case State.Patroling:
                RenderLine(false);
                agent.SetDestination(patrolPosition);
                if (agent.remainingDistance < 1f)
                    patrolPosition = GetPatrolPosition();

                FindTarget();
                break;

            case State.ChaseTarget:
                RenderLine(true);
                agent.SetDestination(target.transform.position);
                if (Vector3.Distance(transform.position, target.transform.position) <= atackRange)
                {
                    state = State.AtackTarget;
                }

                if (Vector3.Distance(transform.position, target.transform.position) > targetDetectionRange)
                {
                    state = State.ToInitialPosition;
                }

                break;

            case State.AtackTarget:
                agent.isStopped = true;
                RenderLine(true);
                LookTarget();
                if (Physics.Raycast(visorPoint.position, visorPoint.forward, out RaycastHit hit, atackRange, playerLayer))
                {
                    ShootTimer();
                }
                if (Vector3.Distance(transform.position, target.transform.position) > atackRange)
                {
                    agent.isStopped = false;
                    state = State.ChaseTarget;
                }
                break;

            case State.ToInitialPosition:
                RenderLine(false);
                agent.SetDestination(initialPosition);
                if (agent.remainingDistance < 1f)
                    state = State.Patroling;
                break;
        }
    }

    private Vector3 GetPatrolPosition()
    {
        Vector3 pointToPatrol = initialPosition;
        float randomRange = Random.Range(patrolMinRange, patrolMaxRange);
        Vector3 proposedPoint = initialPosition + Random.insideUnitSphere * randomRange;

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

        if (Vector3.Distance(transform.position, target.transform.position) <= targetDetectionRange)
        {
            state = State.ChaseTarget;
        }
    }

    private void LookTarget()
    {
        Vector3 lookDirection = target.transform.position - transform.position;
        lookDirection.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 90 * Time.deltaTime);
    }

    private void RenderLine(bool enable)
    {
        line.enabled = enable;

        if (!line.enabled)
            return;

        line.SetPosition(0, visorPoint.position);

        if(Physics.Raycast(visorPoint.position, visorPoint.forward, out RaycastHit hit, atackRange))
        {
            line.SetPosition(1, hit.point);
        } else
        {
            line.SetPosition(1, visorPoint.position + visorPoint.forward * atackRange);
        }
        
    }

    private void ShootTimer()
    {
        if (Time.time > nextTimeToShoot)
        {
            PerformShoot();
            nextTimeToShoot = Time.time + 1 / shootRate;
        }


    }

    private void PerformShoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, visorPoint.position, visorPoint.rotation);

        bullet.GetComponent<Rigidbody>().AddForce(visorPoint.forward * 10f, ForceMode.VelocityChange);

        Destroy(bullet, 6f);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);
        Gizmos.DrawWireSphere(transform.position, atackRange);
    }
}