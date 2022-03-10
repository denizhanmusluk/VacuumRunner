using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollFollowing : MonoBehaviour,IStartGameObserver
{
    // Start is called before the first frame update
    [SerializeField] public GameObject followObject;
    [SerializeField] public GameObject character;
    public float maxSteerAngle = 10f;


    float FollowDistance = 7f;
    float speed;
    float steeringSpeed;
    public bool following = true;
    void Start()
    {
        GameManager.Instance.Add_StartObserver(this);

        steeringSpeed = FindObjectOfType<Control>().steeringSpeed;
        speed = FindObjectOfType<PathFollow>().speed;
        FollowDistance = speed / 5;
        if (Globals.isGameActive)
        {
            StartCoroutine(following2());
        }
    }
    public void StartGame()
    {
        StartCoroutine(following2());
        character.GetComponent<Animator>().SetTrigger("run");
        character.GetComponent<PlayerVacuum>().movemenet();
    }

    IEnumerator following2()
    {
        yield return new WaitForSeconds(0.1f);
        while (following)
        {

            transform.position = Vector3.MoveTowards(transform.position, followObject.transform.position, 0.9f * speed * (Vector3.Distance(transform.position, followObject.transform.position)) / FollowDistance * Time.deltaTime);

            if (Vector3.Distance(transform.position, followObject.transform.position) > 1)
            {
                ApplySteer();
            }

            yield return null;
        }
    }
    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(followObject.transform.position);
        relativeVector /= relativeVector.magnitude;
        float newSteerY = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
  
        transform.Rotate(0, newSteerY * Time.deltaTime * 100f, 0);
  
    }
}
