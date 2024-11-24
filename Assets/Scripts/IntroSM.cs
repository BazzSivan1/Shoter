// Script IntroSM
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class IntroSM : MonoBehaviour
{
    [SerializeField] private float _patrolRange;
    [SerializeField] private float _detectionRange;

    [SerializeField] private Transform _prepoint;
    [SerializeField] private Transform _point;

    private Vector3 _initialPosition;
    private NavMeshAgent _agent;
    private Transform _target;
    private State _state = State.Patrol;
    private Vector3 _patrolPoint;
    private float _timer = 0;
    private FieldOfView _fieldOfView;
    private bool _wait = true;

    private enum State
    {
        Wait,
        Patrol,
        Chase
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _fieldOfView = GetComponentInChildren<FieldOfView>();
    }

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Target").transform;
        _initialPosition = transform.position;
        _state = State.Wait;
    }

    private void Update()
    {
        Debug.Log(_state);

        StateController();

        switch (_state)
        {
            case State.Wait:
                Wait();
                break;
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
        }

        if (_timer > 5)
        {
            _timer = 0;
        }
    }

    private void StateController()
    {
        // Llamamos a FieldOfViewCheck manualmente para asegurarnos de que canSeePlayer está actualizado
        _fieldOfView.FieldOfViewCheck();

        // Si el objetivo es visible, pasamos al estado Chase
        if (_target != null && _fieldOfView.canSeePlayer)
        {
            _timer = 0;
            _state = State.Chase;
        }
        else if (_point != null && _timer >= 5)
        {
            _timer = 0;
            _patrolPoint = GetPatrolPosition();
            _state = State.Patrol;
        }
        else if (_agent.remainingDistance < 0.2f)
        {
            _wait = true;
            _state = State.Wait;
        }
        else if (_point != null && !_fieldOfView.canSeePlayer)
        {
            _state = State.Patrol;
        }
    }

    private void Wait()
    {
        _agent.isStopped = true;

        if (_wait)
        {
            Debug.Log("Waiting...");
            StartCoroutine(Timer());
            _wait = false; // Evita reiniciar el temporizador constantemente
        }
    }

    private void Patrol()
    {
        Debug.Log("Patroling...");
        _agent.isStopped = false;
        _agent.SetDestination(_patrolPoint);
    }

    private void Chase()
    {
        Debug.Log("Chasing...");
        _agent.isStopped = false;
        _agent.SetDestination(_target.position);
    }

    private Vector3 GetPatrolPosition()
    {
        Vector3 pointToPatrol = _initialPosition;
        Vector3 proposedPoint = _initialPosition + Random.insideUnitSphere * _patrolRange;

        _prepoint.position = proposedPoint;

        if (NavMesh.SamplePosition(proposedPoint, out NavMeshHit navMeshHit, 10f, NavMesh.AllAreas))
        {
            NavMeshPath path = new NavMeshPath();
            _agent.CalculatePath(navMeshHit.position, path);
            bool canReachPoint = path.status == NavMeshPathStatus.PathComplete;

            if (canReachPoint)
                pointToPatrol = navMeshHit.position;
        }

        _point.position = pointToPatrol;
        return pointToPatrol;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_initialPosition, _patrolRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }

    IEnumerator Timer()
    {
        while (_timer < 5)
        {
            yield return new WaitForSeconds(1);
            _timer++;
            Debug.Log(_timer);
        }
    }
}
