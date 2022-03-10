using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreParticleDestroy : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(destObject());
    }

    IEnumerator destObject()
    {
        yield return new WaitForSeconds(2f);
        gameObject.transform.parent = null;
        Destroy(gameObject);
    }
}
