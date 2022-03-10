using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Saw : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveX(transform.position.x + 5f, 2f).SetLoops(-1, LoopType.Yoyo);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 500 * Time.deltaTime, 0);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("engelll");
            other.transform.GetChild(0).GetChild(0).gameObject.GetComponent<IHit>().gameOver(gameObject);
        }
    }
}
