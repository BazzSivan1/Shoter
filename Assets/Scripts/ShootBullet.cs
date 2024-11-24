using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _shootingDistance;
    [SerializeField] private Transform _shootingPosition;
    [SerializeField] private float _shootingTime;
    [SerializeField] private float _shootingSpeed;
    private float _timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (Vector3.Distance(transform.position, _target.position) <= _shootingDistance)
        {
            if (_timer >= _shootingTime)
            {
                GameObject bullet = Instantiate(_bullet, _shootingPosition.position, _shootingPosition.rotation);
                _timer = 0;
                StartCoroutine(MoveAndDestroyBullet(bullet, 5));
            }
        }
    }

    private IEnumerator MoveAndDestroyBullet(GameObject bullet, float destroyTime)
    {
        float timer = 0;

        while (timer < destroyTime)
        {
            bullet.transform.Translate(Vector3.forward * _shootingSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(bullet);
    }
}
