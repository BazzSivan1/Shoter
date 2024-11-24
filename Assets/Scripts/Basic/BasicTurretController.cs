using UnityEngine;

public class BasicTurretController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float range = 10f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private Transform turretPivot;
    [SerializeField] private bool lookAt;

    [Header("Shooting Settings")]
    [SerializeField] float shootPower = 10f;
    [SerializeField] private float shootCadency = 1;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem shotEffect;

    private float nextShootTime = 0f;

    void Update()
    {
        if (target == null)
            return;

        if (lookAt)
        {
            turretPivot.LookAt(target.position);
        } 
        else
        {
            Vector3 lookDirection = target.position - turretPivot.position;
            lookDirection.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            turretPivot.rotation = Quaternion.RotateTowards(turretPivot.rotation, lookRotation, rotateSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, target.position) < range && Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + 1 / shootCadency;
            Shoot();
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

        Destroy(bullet, 6);
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
