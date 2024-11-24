using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float timeBetweenSpawns = 2f;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private bool hasOffset;
    [SerializeField] private float maxHorizontalOffset;

    private float countdown = 0;
 

    // En el m�tode Update fa un compte enrere a partir de "timeBetweenSpawns"
    // Quan arriba 0 cridem a Spawn
    void Update()
    {
        if (countdown <= 0)
        {
            Spawn();
            countdown = timeBetweenSpawns;
        }

        countdown -= Time.deltaTime;
    }

    private void Spawn()
    {
        // Calculem un offset (despla�ament) des d'on s'he de instanciar el prefab
        Vector3 offset = Vector3.zero;
        if(hasOffset)
        {
            float horizontalOffset = Random.Range(-maxHorizontalOffset, maxHorizontalOffset);
            offset = new Vector3(horizontalOffset, 0, 0);
        }
        
        // Creem una inst�ncia de GameObject en la posici� del transform m�s offset amb la rotaci� del transform
        GameObject objectInstance = Instantiate(objectPrefab, transform.position + offset, transform.rotation);

        // Destruirem la inst�ncia 5 segons despr�s
        //Destroy(objectInstance, 5f);
    }
}
