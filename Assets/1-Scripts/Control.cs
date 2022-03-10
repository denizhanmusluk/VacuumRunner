using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Control : MonoBehaviour
{
    private float m_previousX;
    private float m_previousY;
    [SerializeField] Vector3 bounding;
    public float dX;
    private float dY;
    public float dX_Sum;
    [Range(0.0f, 10.0f)]
    [SerializeField] float Controlsensivity;

    //[Range(0.0f, 50.0f)]
    //[SerializeField] float Steeringsensivity;

    [Range(0.0f, 50.0f)]
    //[SerializeField] public float HorizontalSens;

    public float acceleration = 15;
    //public BullControl cn;
    [SerializeField] public float steeringSpeed = 180;

    public float Xmove, Steer, Speed;
    [SerializeField] Transform left, right;
    [SerializeField] GameObject playerParents;
    //[SerializeField] CinemachineVirtualCamera cam;
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "doll")
        {
            //StartCoroutine(jumpFunct(other.GetComponent<JumpPoint>().jump, other.GetComponent<JumpPoint>().jumpSpeed));

            //childrenBehaviour = playerParents.GetComponentsInChildren<PlayerBehaviour>();
            //foreach (var bhvr in childrenBehaviour)
            //{
            //    bhvr.jumpTarget = other.gameObject;
            //    //GameEvents.characterEvent.AddListener(bhvr.jumpPlayer);
            //}
        }
    }
    IEnumerator jumpFunct(float _jump, float _jumpSpeed)
    {
        while (transform.localPosition.y < _jump)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y + _jump, transform.localPosition.z), Time.deltaTime * 50 * _jumpSpeed);
            yield return null;
        }
        //yield return new WaitForSeconds(0.5f);
        while (transform.localPosition.y > 0)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y - _jump, transform.localPosition.z), Time.deltaTime * 50 * _jumpSpeed);
            yield return null;
        }
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
    }
    private void Awake()
    {
        //cn = cn.GetComponent<BullControl>();
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    childrenBehaviour = playerParents.GetComponentsInChildren<PlayerBehaviour>();
        //    foreach (var bhvr in childrenBehaviour)
        //    {
        //        GameEvents.characterEvent.AddListener(bhvr.jumpPlayer); 
        //    }

        //}


        if (Globals.isGameActive && !Globals.finish)
        {

            gameUpdate();
            Move(Xmove, Steer, Speed);
            
        }
    }
    float preX = 0;
    float delta = 0;
    private void gameUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_previousX = Input.mousePosition.x;
            m_previousY = Input.mousePosition.y;
            dX = 0f;
            dY = 0f;
            dX_Sum = 0f;
            delta = 0;
            //preX = cam.Follow.transform.localPosition.x;
        }
        if (Input.GetMouseButton(0))
        {
            dX = (Input.mousePosition.x - m_previousX) / 10f;
            dY = (Input.mousePosition.y - m_previousY) / 100f;
            dX_Sum += dX;

            //delta = cam.Follow.transform.localPosition.x - preX;

            //var transposer = cam.GetCinemachineComponent<CinemachineTransposer>();
            //transposer.m_FollowOffset.x = -transform.localPosition.x / 20;

            //preX = cam.Follow.transform.localPosition.x;

            m_previousX = Input.mousePosition.x;
            m_previousY = Input.mousePosition.y;
           
        }
        if (Input.GetMouseButtonUp(0))
        {
            dX_Sum = 0f;
            dX = 0f;
            dY = 0f;
            delta = 0;
        }
        Xmove = Controlsensivity * dX / (Time.deltaTime * 25);
        //Steer = Steeringsensivity * dX / (Time.deltaTime * 25);
        Speed = acceleration;
        //cn.Move(Xmove, Steer, Speed);

    }
    public void moveReset()
    {
        Xmove = 0;
        Steer = 0;
    }
    public void Move(float _swipe, float _steering, float _speed)
    {
        if (_swipe > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, right.position, Time.deltaTime * Mathf.Abs(_swipe));
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, -45), steeringSpeed * Time.deltaTime);

        }
        if (_swipe < 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, left.position, Time.deltaTime * Mathf.Abs(_swipe));
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 45), steeringSpeed * Time.deltaTime);
        }
        if (_swipe == 0)
        {
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0), 3 * steeringSpeed * Time.deltaTime);
        }
    }
}
