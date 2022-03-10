using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeederEmission : MonoBehaviour
{
    Color FirstColour;
    void Start()
    {
        FirstColour = transform.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        StartCoroutine(collUpd());

    }
    IEnumerator collUpd()
    {
        while (true)
        {
            float counter = 1f;
            while (counter < 5)
            {
                counter += 15 * Time.deltaTime;

                transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", FirstColour * counter);

                yield return null;
            }
            while (counter > 2)
            {
                counter -= 15 * Time.deltaTime;

                transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", FirstColour * counter);

                yield return null;
            }
        }


        //transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", FirstColour);


    }
}
