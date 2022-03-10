using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassHit : MonoBehaviour
{
    GameObject glass, glassBroken;
    public int id;
    private void Start()
    {
        glass = transform.GetChild(0).gameObject;
        glassBroken = transform.GetChild(1).gameObject;
    }
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.GetComponent<IDamageble>() != null)
    //    {
    //        other.gameObject.GetComponent<IDamageble>().hitGlass(gameObject);
    //        glassHit(other.gameObject);
    //        Globals.bike--;
    //        BicycleCount.Instance.BikeCountSet();
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "machine")
        {
            //collision.gameObject.GetComponent<IDamageble>().hitGlass(gameObject);
            glassHit(other.gameObject);
            transform.GetComponent<Collider>().enabled = false;
            //Globals.bike--;
            //BicycleCount.Instance.BikeCountSet();
            other.transform.parent.GetComponent<Finish>().scoreMagnitude = id;
        }
    }
    void glassBreak()
    {
        glass.SetActive(false);
        glassBroken.SetActive(true);

    }
    public void glassHit(GameObject man)
    {
        glass.SetActive(false);
        glassBroken.SetActive(true);

        StartCoroutine(steerObstacles(man));
        transform.GetComponent<BoxCollider>().enabled = false;

        for (int i = 0; i < glassBroken.transform.childCount; i++)
        {
            Vector3 obsPos = new Vector3(-1 / glassBroken.transform.GetChild(i).transform.localPosition.y, 1, -1 / (glassBroken.transform.GetChild(i).transform.localPosition.y));
            glassBroken.transform.GetChild(i).transform.GetComponent<Rigidbody>().useGravity = true;
            glassBroken.transform.GetChild(i).transform.GetComponent<Rigidbody>().AddForce(obsPos * 500);
        }

        //StartCoroutine(obstacleDest(obs));
        //StartCoroutine(CS.Shake());
    }
    IEnumerator steerObstacles(GameObject man)
    {
        float _time = 0;
        while (_time < 0.9f)
        {
            for (int i = 0; i < glassBroken.transform.childCount; i++)
            {
                glassBroken.transform.GetChild(i).transform.Rotate((glassBroken.transform.GetChild(i).transform.position.x - man.gameObject.transform.position.x) / 4,(glassBroken.transform.GetChild(i).transform.position.y - man.gameObject.transform.position.y) / 2, (glassBroken.transform.GetChild(i).transform.position.x - man.gameObject.transform.position.x) / 2);

            }
            _time += Time.deltaTime;
            yield return null;
        }
    }
}
