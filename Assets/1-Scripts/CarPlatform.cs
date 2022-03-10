using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPlatform : MonoBehaviour
{
    [SerializeField] CapsuleCollider colliderTriger, collidirCollision;
    public enum States { Xp, Yp, Zp, Xn, Yn, Zn }
    public States RotateAxis;
    Vector3 axis;
    bool hit = true;
    bool shaking = true;
    protected Rigidbody[] childrenRigidbody;

    void Start()
    {
        childrenRigidbody = GetComponentsInChildren<Rigidbody>();

        colliderTriger.enabled = true;
        collidirCollision.enabled = true;
        switch (RotateAxis)
        {
            case States.Xp:
                {
                    axis = new Vector3(1, 0, 0);
                }
                break;
            case States.Yp:
                {
                    axis = new Vector3(0, 1, 0);
                }
                break;
            case States.Zp:
                {
                    axis = new Vector3(0, 0, 1);
                }
                break;
            case States.Xn:
                {
                    axis = new Vector3(-1, 0, 0);
                }
                break;
            case States.Yn:
                {
                    axis = new Vector3(0, -1, 0);
                }
                break;
            case States.Zn:
                {
                    axis = new Vector3(0, 0, -1);
                }
                break;
        }
    }
    //private void OnTriggerEnter(Collider collider)
    //{

    //    if (hit && collider.transform.GetComponent<IHit>() != null)
    //    {
    //        collider.transform.GetComponent<IHit>().poleHit(gameObject);
    //        colliderTriger.enabled = false;
    //        collidirCollision.enabled = true;
    //        transform.GetComponent<Rigidbody>().useGravity = true;
    //        StartCoroutine(rotBal(axis));
    //        hit = false;
    //    }
    //}
    private void OnTriggerEnter(Collider collider)
    {
        if (hit && collider.transform.GetComponent<IHit>() != null)
        {

            collider.transform.GetComponent<IHit>().poleHit(gameObject);
            colliderTriger.enabled = false;
            collidirCollision.enabled = true;
            GameObject car = transform.parent.parent.transform.GetChild(0).gameObject;

            hit = false;
            StartCoroutine(partDest(axis, car));
        }
    }
    IEnumerator partDest(Vector3 vect , GameObject car)
    {


        yield return new WaitForSeconds(0.3f);

        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.GetComponent<Rigidbody>().isKinematic = false;
        StartCoroutine(rotBal(axis));
        transform.parent.transform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        car.GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSeconds(0.3f);

        car.GetComponent<Rigidbody>().AddForce(car.transform.forward * 100000);

        yield return new WaitForSeconds(0.5f);
        //StartCoroutine(carForce(car));
        car.AddComponent<Car>();
        //car.GetComponent<Rigidbody>().AddForce(car.transform.forward * 300000);
        yield return new WaitForSeconds(0.5f);
        //car.GetComponent<Rigidbody>().AddForce(car.transform.forward * 300000);

        transform.parent.transform.GetChild(3).GetComponent<Rigidbody>().isKinematic = false;
        transform.parent.transform.GetChild(4).GetComponent<Rigidbody>().isKinematic = false;
        GameObject column1 = transform.parent.transform.GetChild(3).gameObject;
        GameObject column2 = transform.parent.transform.GetChild(4).gameObject;
        Vector3 firstRot = new Vector3(column1.transform.eulerAngles.x, column1.transform.eulerAngles.y, column1.transform.eulerAngles.z);
        float counter = 0;
        float angle = 0;
        while (counter < Mathf.PI / 2)
        {
            counter += 4f * Time.deltaTime;
            angle = Mathf.Cos(counter);
            angle *= 90;
            angle -= 90;
            column1.transform.rotation = Quaternion.Euler(firstRot.x + angle * vect.x, firstRot.y + angle * vect.y, firstRot.z + angle * vect.z);
            column2.transform.rotation = Quaternion.Euler(firstRot.x + angle * vect.x, firstRot.y + angle * vect.y, firstRot.z + angle * vect.z);

            yield return null;
        }
    }
    IEnumerator carForce(GameObject car)
    {
        car.transform.parent = null;

        float counter = 0;
        while(counter < 6)
        {
            counter += Time.deltaTime;

            //car.GetComponent<Rigidbody>().AddForce(car.transform.forward * 50000);
            car.transform.Translate(car.transform.forward * Time.deltaTime * 10);
            yield return null;
        }
    }

    IEnumerator rotBal(Vector3 vect)
    {
        Vector3 firstRot = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        float counter = 0;
        float angle = 0;
        while (counter < Mathf.PI / 2)
        {
            counter += 4f * Time.deltaTime;
            angle = Mathf.Cos(counter);
            angle *= 90;
            angle -= 90;
            transform.rotation = Quaternion.Euler(firstRot.x + angle * vect.x, firstRot.y + angle * vect.y, firstRot.z + angle * vect.z);

            yield return null;
        }
    }

}