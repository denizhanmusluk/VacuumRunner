using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHit : MonoBehaviour
{
    Color FirstColour;
    float firstSpeed;
    bool speeder = true;
    [SerializeField] public float speed;
    void Start()
    {
        FirstColour = transform.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");
        //StartCoroutine(collUpd());

    }

    // Update is called once per frame
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "followobject" && speeder)
    //    {
    //        speeder = false;
    //        firstSpeed = other.transform.parent.GetComponent<PathFollow>().speed;
    //        other.transform.parent.GetComponent<PathFollow>().speed = speed;
    //        transform.parent.GetChild(0).GetComponent<Collider>().enabled = false;
    //        transform.parent.GetChild(1).GetComponent<Collider>().enabled = false;
    //        StartCoroutine(speedReset(other.gameObject));
    //    }
    //}
    IEnumerator speedReset(GameObject followObj)
    {
        yield return new WaitForSeconds(3);
        followObj.transform.parent.GetComponent<PathFollow>().speed = firstSpeed;

    }
    IEnumerator collUpd()
    {
        while (true)
        {
            float counter = 1f;
            while (counter < 4)
            {
                counter += 5 * Time.deltaTime;

                transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", FirstColour * counter);

                yield return null;
            }
            while (counter > 1)
            {
                counter -= 5 * Time.deltaTime;

                transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", FirstColour * counter);

                yield return null;
            }
        }


        //transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", FirstColour);


    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("engelll");
            other.transform.GetChild(0).GetChild(0).gameObject.GetComponent<IHit>().obstacleHit(gameObject, speed);
            transform.parent.GetChild(0).GetComponent<Collider>().enabled = false;
            transform.parent.GetChild(1).GetComponent<Collider>().enabled = false;

        }
    }
}
