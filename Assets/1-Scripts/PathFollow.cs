using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathFollow : MonoBehaviour,IStartGameObserver
{
    public PathCreator pathCreator;
    public float speed = 5;
    float distanceTravelled;
    //[SerializeField] Transform cameraTarget;

    private void Start()
    {
        GameManager.Instance.Add_StartObserver(this);
    }
    public void StartGame()
    {
        GameEvents.characterEvent.RemoveAllListeners();
        GameEvents.characterEvent.AddListener(_Update);

    }
    void _Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0);

        //cameraTarget.position= pathCreator.path.GetPointAtDistance(distanceTravelled-5);
    }
}
