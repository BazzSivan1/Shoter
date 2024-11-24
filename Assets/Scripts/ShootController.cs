using UnityEngine;
using TMPro;

public class ShootController : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] float shootRate = 10f;
    [SerializeField] float range = 100f;

    [SerializeField] Camera fpsCamera;
    [SerializeField] ParticleSystem fireFX;
    [SerializeField] GameObject impactFX;

    [SerializeField] TextMeshProUGUI text;

    private float nextShootTime = 0f;
    private float shootNumber = 0;

    private void Start()
    {
        text.SetText(shootNumber.ToString());
    }

    private void Update()
    {
        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * range, Color.red);
    }

    public void Shoot()
    {
        if (Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + 1 / shootRate;
            PerformShoot();
            shootNumber++;
            text.SetText(shootNumber.ToString());
        }
    }

    private void PerformShoot()
    {
        if (fireFX != null)
            fireFX.Play();
            //AUDIO FX

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.distance <= range)
            {
                //IMPACTE
                if (impactFX != null)
                {
                    GameObject impact = Instantiate(impactFX, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact, 2f);
                }
                
            }

        }
    }
}
