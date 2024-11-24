using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float radius = 5.0F;
    [SerializeField] private float power = 10.0F;
    [SerializeField] private float upwardsModifier = 0F;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false;
                Vector3 originalPosition = rb.position;
                Quaternion originalRotation = rb.rotation;
                rb.AddExplosionForce(power, explosionPos, radius, upwardsModifier);
                StartCoroutine(ResetPosition(rb, originalPosition, originalRotation, 4));
            }
                
        }
    }

    IEnumerator ResetPosition(Rigidbody rb, Vector3 originalPosition, Quaternion originalRotation, float afterSeconds)
    {
        yield return new WaitForSeconds(afterSeconds);
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.position = originalPosition;
        rb.rotation = originalRotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
