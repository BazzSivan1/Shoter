using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerBulletController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    private Rigidbody rb;
    private Transform target;
    private Vector3 currentDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentDirection = transform.forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            currentDirection = target.position - transform.position;
            currentDirection.y = 0;
        }



        rb.MovePosition(rb.position + currentDirection.normalized * speed * Time.fixedDeltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}
