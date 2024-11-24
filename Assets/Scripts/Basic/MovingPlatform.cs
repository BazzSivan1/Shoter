using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform waypointGroup;
    [SerializeField] private float moveSpeed;

    private Vector3 currentWaypoint, targetWaypoint;
    public float interpolator;
    public int waypointIndex;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        interpolator = 0;
        waypointIndex = 0;

        currentWaypoint = transform.position;
        targetWaypoint = waypointGroup.GetChild(waypointIndex).position;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetWaypoint == null)
            return;

        interpolator += moveSpeed/10 * Time.deltaTime;
        Vector3 nextPosition = Vector3.Lerp(currentWaypoint, targetWaypoint, Mathf.SmoothStep(0,1,interpolator));

        rb.MovePosition(nextPosition);

        CheckWayPoint();
    }

    private void CheckWayPoint()
    {
        if (interpolator >= 1)
        {
            if (waypointIndex < waypointGroup.childCount -1)
            {
                waypointIndex++;
            } else
            {
                waypointIndex = 0;
            }

            currentWaypoint = targetWaypoint;

            targetWaypoint = waypointGroup.GetChild(waypointIndex).position;
            interpolator = 0;
        }
    }
}
