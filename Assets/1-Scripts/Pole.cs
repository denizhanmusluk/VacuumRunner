using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    [SerializeField] CapsuleCollider colliderTriger, collidirCollision;
    public enum States { Xp, Yp, Zp, Xn, Yn, Zn }
    public States RotateAxis;
    Vector3 axis;
    bool hit = true;
    bool pouring = true;
    float touchTime = 0f;
    bool shaking = true;
    protected Rigidbody[] childrenRigidbody;
    [SerializeField] GameObject exp;
    void Start()
    {
        childrenRigidbody = GetComponentsInChildren<Rigidbody>();

        colliderTriger.enabled = true;
        collidirCollision.enabled = false;
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

    private void OnTriggerStay(Collider collider)
    {
        if (collider.transform.GetComponent<IHit>() != null)
        {
            touchTime += Time.deltaTime;
            if (hit)
            {

           
                StartCoroutine(shake(axis));
                hit = false;
            }
            collider.transform.GetComponent<IHit>().poleHit(gameObject);
 

            if (touchTime > 0.4f && pouring)
            {
                pouring = false;
                foreach (var rigidb in childrenRigidbody)
                {
                    rigidb.detectCollisions = true;
                    rigidb.isKinematic = false;
                    rigidb.useGravity = true;
                    rigidb.drag = 1;
                    GameObject particles = rigidb.gameObject;
                    particles.transform.parent = null;
                    StartCoroutine(partDest(particles));
                    if (gameObject.name != rigidb.transform.name)
                    {
                        rigidb.AddForce(new Vector3(axis.z, axis.y, axis.x) * 3000);
                        rigidb.AddForce(new Vector3(0, 0, 1) * 6500);
                    }
                }
            }

            if (touchTime > 0.8f && shaking)
            {
                shaking = false;
                colliderTriger.enabled = false;
                collidirCollision.enabled = true;
                transform.GetComponent<Rigidbody>().useGravity = true;
                StartCoroutine(rotBal(axis));
       
            }
        }
    }
    IEnumerator partDest(GameObject part)
    {
        yield return new WaitForSeconds(4f);
        Destroy(part);
    }
    IEnumerator shake(Vector3 vect)
    {
        while (shaking)
        {
            float counter = 0;
            float angle = 0;
            while (counter < Mathf.PI)
            {
                counter += 20 * Time.deltaTime;
                angle = Mathf.Cos(counter);
                angle *= 3;

                transform.rotation = Quaternion.Euler(angle * vect.x, angle * vect.y, angle * vect.z);

                yield return null;
            }

            while (counter > 0)
            {
                counter -= 20 * Time.deltaTime;
                angle = Mathf.Cos(counter);
                angle *= 3;

                transform.rotation = Quaternion.Euler(angle * vect.x, angle * vect.y, angle * vect.z);
                yield return null;
            }
        }
    }
    IEnumerator rotBal(Vector3 vect)
    {

        float counter = 0;
        float angle = 0;
        while (counter < Mathf.PI / 2)
        {
            counter += 1.5f * Time.deltaTime;
            angle = Mathf.Cos(counter);
            angle *= 90;
            angle -= 90;
            transform.rotation = Quaternion.Euler(angle * vect.x, -vect.z * 20, angle * vect.z);

            yield return null;
        }
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        exp.SetActive(true);
    }
   
}