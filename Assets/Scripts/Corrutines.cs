using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corrutines : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Bucle());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Contador()
    {
        Debug.Log(0);

        yield return new WaitForSeconds(1);
        Debug.Log(1);

        yield return new WaitForSeconds(2);
        Debug.Log(2);

        yield return null;
    }

    IEnumerator Bucle()
    {
        int contador = 0;

        while (contador < 18)
        {
            Debug.Log(contador);
            yield return new WaitForSeconds(1);
            contador++;
        }
    }
}
