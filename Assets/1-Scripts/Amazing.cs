using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Amazing : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(pointUp());
    }

    IEnumerator pointUp()
    {

        float counter = 0;
        float speed = 0;
        while (counter < Mathf.PI / 2)
        {
            counter += 1.5f * Time.deltaTime;
            speed = Mathf.Cos(counter);
            speed *= 10;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 1, 1), Time.deltaTime * speed);
            transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1 - (counter / (Mathf.PI / 2)));

            yield return null;
        }
        Destroy(gameObject);
    }
}
