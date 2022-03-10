using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.GetComponent<IHit>() != null)
        {


            collider.transform.GetComponent<IHit>().humanHit(gameObject);
            transform.GetComponent<Collider>().isTrigger = false;
        }
    }
}
