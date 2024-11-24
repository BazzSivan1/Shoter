using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMuvment : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Move(hit.point);
                }
            }
        }
        else
        {
            Move(_target.position);
        }
    }

    private void Move(Vector3 targetPosition)
    {
        _agent.SetDestination(targetPosition);
    }
}
