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
 

    // En el mètode Update fa un compte enrere a partir de "timeBetweenSpawns"
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
        // Calculem un offset (desplaçament) des d'on s'he de instanciar el prefab
        Vector3 offset = Vector3.zero;
        if(hasOffset)
        {
            float horizontalOffset = Random.Range(-maxHorizontalOffset, maxHorizontalOffset);
            offset = new Vector3(horizontalOffset, 0, 0);
        }
        
        // Creem una instància de GameObject en la posició del transform més offset amb la rotació del transform
        GameObject objectInstance = Instantiate(objectPrefab, transform.position + offset, transform.rotation);

        // Destruirem la instància 5 segons després
        //Destroy(objectInstance, 5f);
    }
}
