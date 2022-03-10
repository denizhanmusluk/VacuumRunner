using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    void Start()
    {
        transform.parent = null;

        StartCoroutine(carForce());
    }
    IEnumerator carForce()
    {

        float counter = 0;
        while (counter < 3)
        {
            counter += Time.deltaTime;

            //car.GetComponent<Rigidbody>().AddForce(car.transform.forward * 50000);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * 10);
            yield return null;
        }
    }
}
