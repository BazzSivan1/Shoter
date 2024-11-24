using System.Collections;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private float range = 10f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private Transform turretPivot;

    [Header("Shooting Settings")]
    [SerializeField] float shootPower = 10f;
    [SerializeField] private float shootCadency = .25f;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem shotEffect;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetTarget());
        StartCoroutine(CoShoot());
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        Collider targetCollider = target.GetComponent<Collider>();
        
        if (targetCollider != null)
        {
            turretPivot.LookAt(targetCollider.bounds.center);
        } else
        {
            Vector3 lookDirection = target.position - turretPivot.position;
            lookDirection.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            turretPivot.rotation = Quaternion.RotateTowards(turretPivot.rotation, lookRotation, rotateSpeed * Time.deltaTime);
        }

    }

    IEnumerator GetTarget()
    {
        while (gameObject.activeSelf)
        {
            float shortestDistance = Mathf.Infinity;
            GameObject nearestTarget = null;
            GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(targetTag);
            foreach (GameObject pTarget in potentialTargets)
            {
                float pTargetDistance = Vector3.Distance(transform.position, pTarget.transform.position);
                if (pTargetDistance < shortestDistance)
                {
                    shortestDistance = pTargetDistance;
                    nearestTarget = pTarget;
                }
            }

            if (nearestTarget != null && shortestDistance <= range)
            {
                target = nearestTarget.transform;
            } 
            else
            {
                target = null;
            }

            yield return new WaitForSeconds(.25f);
        }
    }

    IEnumerator CoShoot()
    {
        while (gameObject.activeSelf)
        {
            if (target != null)
            {
                Shoot();
            }
            yield return new WaitForSeconds(shootCadency);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        if (bulletPrefab.name.Equals("SeekerBullet"))
        {
            bullet.GetComponent<SeekerBulletController>().SetTarget(target);
        }
        else
        {
            bullet.GetComponent<Rigidbody>().AddForce(shootPoint.forward * shootPower, ForceMode.VelocityChange);
        }

        if (shotEffect != null)
            shotEffect.Play();

        Destroy(bullet, 4);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
