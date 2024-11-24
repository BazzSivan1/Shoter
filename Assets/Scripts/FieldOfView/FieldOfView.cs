// Script FieldOfView
using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;
    public Transform target;
    public bool canSeePlayer;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        while (true)
        {
            FieldOfViewCheck();
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Método de comprobación manual
    public void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length > 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                canSeePlayer = !Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask);
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }
    }
}
