using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : MonoBehaviour
{
    [SerializeField] GameObject prefabBand;
    [SerializeField] GameObject[] clothes;
    [SerializeField] GameObject[] oldClothes;
    [SerializeField] GameObject[] particles;
    [SerializeField] GameObject clothesTarget;
    Animator animator;
    Rigidbody playerRigidbody;
    [Range(0, 50)] [SerializeField] public float followSpeed, walkSpeed, rotSpeed, startFollowSpeed;
    float normalWalkSpeed;
    [HideInInspector] public AudioSource audioSource;
    bool catRunActive;
    bool firstHitActive = true;
    int[] rotSerchDirect;
    int rotIndex;
    int rotateDirect = 0;

    float humanRotation;
    public float moveSpeed;
    private LineRenderer lineRenderer;
    [SerializeField] public Transform player;

    public enum States { vacuum, StartFollowing, idle, caught }
    public States currentBehaviour;
    public bool hitActive = true;
    bool mouseCaughtActive = false;
    bool first = true;
    bool second = true;
    public bool follow = true;
    string[] idleAnim = new string[2];
    string[] runAnim = new string[2];
    string[] angryAnim = new string[4];
    int idleSlection;
    int runSelection;
    int angrySelection;
    public bool catching = false;

    //void draw()
    //{
    //    lineRenderer = GetComponent<LineRenderer>();
    //    Vector3[] positions = new Vector3[3] { new Vector3(0, 0, 0), new Vector3(-1, 1, 0), new Vector3(1, 1, 0) };
    //    DrawTriangle(positions);
    //}

    //void DrawTriangle(Vector3[] vertexPositions)
    //{

    //    lineRenderer.positionCount = 3;
    //    lineRenderer.SetPositions(vertexPositions);
    //}
    void Start()
    {
        //GameManager.Instance.Add_WinObserver(this);
        animator = GetComponent<Animator>();
        idleAnim[0] = "idle1";
        idleAnim[1] = "idle2";

        runAnim[0] = "run1";
        runAnim[1] = "run2";

        angryAnim[0] = "angry1";
        angryAnim[1] = "angry2";
        angryAnim[2] = "angry3";
        angryAnim[3] = "angry4";
        //GameManager.Instance.Add_WinObserver(this);
        //GameManager.Instance.Add_LoseObserver(this);
        idleSlection = Random.Range(0, 2);
        animator.SetTrigger(idleAnim[idleSlection]);

        playerRigidbody = GetComponent<Rigidbody>();
    }
 
    public void startMove()
    {

    }
    //IEnumerator startUp()
    //{
    //    while (!Globals.startActive) { yield return null; }
    //    normalWalkSpeed = walkSpeed;
    //    moveSpeed = walkSpeed;
    //    currentBehaviour = States.searching;
    //    rotSerchDirect = new int[2];
    //    rotSerchDirect[0] = -1;
    //    rotSerchDirect[1] = 1;

    //    catRunActive = true;
    //    //Globals.aiControlActive = true;
    //    animator.SetBool("Idle", false);


    //}
    //public void WinScenario()
    //{
    //    //Debug.Log("cat losee");
    //    //animator.SetTrigger("sad");
    //    currentBehaviour = States.caught;
    //    transform.GetComponent<ManRagdoll>().humansRagdoll(true);
    //}
    public void LoseScenario()
    {
        //Debug.Log("cat winn");
        //animator.SetBool("Idle", true);

    }
    // Update is called once per frame
    void Update()
    {
        //if (Globals.isGameActive)
        //{
            switch (currentBehaviour)
            {
                case States.vacuum:
                    {
                        //vacuumClothes();
                    }
                    break;
                case States.StartFollowing:
                    {
                        Move2();
                    }
                    break;

                case States.caught:
                    {
                        Move3();
                    }
                    break;

                case States.idle:

                    break;
            }


        //}
    }
    IEnumerator vacuumClothes(int clothesCount)
    {
        for (int i = 0; i < clothesCount; i++)
        {
            float counter = 0;
            oldClothes[i].SetActive(false);
            clothes[i].SetActive(true);
            
            if (particles.Length > i)
            {
                particles[i].SetActive(true);
            }
            //while (clothes[i].transform.localScale.z < 4)
            //{
            //    
            //    clothes[i].transform.localScale = Vector3.Lerp(clothes[i].transform.localScale, new Vector3(0.5f, 0.5f, 4), 50 * Time.deltaTime);
            //    yield return null;
            //}
            while (Vector3.Distance(clothes[i].transform.position, clothesTarget.transform.position) > 0.5f)
            {
                counter += Time.deltaTime;
                clothes[i].transform.localScale = Vector3.Lerp(clothes[i].transform.localScale, new Vector3(0.3f, 0.3f, 2), 50 * Time.deltaTime);

                clothes[i].transform.LookAt(clothesTarget.transform);
                clothes[i].transform.position = Vector3.MoveTowards(clothes[i].transform.position, clothesTarget.transform.position,(1+counter * 5) * 5 * Time.deltaTime);
                yield return null;
            }

            clothes[i].transform.parent = clothesTarget.transform;
            clothes[i].SetActive(false);
            //Destroy(clothes[i]);
            clothesTarget.transform.parent.GetComponent<VacuumMachine>().setScale();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy" || collision.transform.tag == "Player")
        {
            StartCoroutine(enemyRigidbodySet());
        }
        if (collision.transform.tag == "right" || collision.transform.tag == "left")
        {
            if (transform.position.x - collision.transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 10, transform.eulerAngles.z);
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, -10, transform.eulerAngles.z);
            }
            Debug.Log(transform.name);
        }
    }
    IEnumerator enemyRigidbodySet()
    {
        transform.GetComponent<Collider>().enabled = false;
        //transform.GetComponent<Rigidbody>().useGravity = false;
        //transform.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(0.5f);
        transform.GetComponent<Collider>().enabled = true;
        //transform.GetComponent<Rigidbody>().useGravity = true;
        //transform.GetComponent<Rigidbody>().isKinematic = false;
    }
    public void _firstMoving()
    {
        
        StartCoroutine(firstMove());
        StartCoroutine(vacuumClothes(clothes.Length));
    }

    private void mouseFollowing(Vector3 target)
    {

        Vector3 relativeVector = transform.InverseTransformPoint(target);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude);
        transform.Rotate(0, newSteer * Time.deltaTime * 50 * rotSpeed, 0);
    }
    IEnumerator firstMove()
    {
        if (first)
        {
            angrySelection = Random.Range(0, 4);
            animator.SetTrigger(angryAnim[angrySelection]);
        }
        moveSpeed = startFollowSpeed;
        Debug.Log("clothes");
        float counter = 0;
        // yakalanmak icin counter = 0.6f
        while (counter < 0.8f)
        {
            counter += Time.deltaTime;
            mouseFollowing(player.position);
            if(counter > 0.4f)
            {
                moveSpeed = followSpeed;
            }
            yield return null;
        }
        rotSpeed = 4;

        currentBehaviour = States.StartFollowing;

        GetComponent<Collider>().enabled = true;


        counter = 0;
        //yakalanmak icin counter = 0.3f
        while (counter < 0.4f)
        {
            counter += Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z), Time.deltaTime * 100);
            yield return null;
        }
        catching = true;
        //for (int i = 0; i < particles.Length; i++)
        //{
        //    particles[i].SetActive(false);
        //}
        if (second)
        {
            runSelection = Random.Range(0, 2);
            animator.SetTrigger(runAnim[runSelection]);
            second = false;
        }
    }
    public void Move1()
    {
        if (first)
        {
            rotSpeed = 10;

            StartCoroutine(firstMove());
            first = false;
        }
        //catRigidbody.velocity = transform.forward * moveSpeed;
        if (player.transform.position.z - transform.position.z > 1 && player.transform.position.z - transform.position.z < 20)
        {
            if (player.transform.position.z - transform.position.z < 6)
            {
                mouseFollowing(player.position);
            }
            else
            {
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z), Time.deltaTime * 100);
            }
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z), Time.deltaTime * 100);
            //transform.position = Vector3.Lerp(transform.position,new Vector3(transform.position.x, transform.position.y, player.transform.position.z), moveSpeed * Time.deltaTime);
            //playerRigidbody.velocity = transform.forward * moveSpeed * 350 * Time.deltaTime;
            //mouseFollowing(player.position);
            transform.Translate(transform.forward * Time.deltaTime * moveSpeed * 6.7f);
            //transform.position = 
        }
        else
        {
            //playerRigidbody.velocity = Vector3.zero;

        }
        //else if (Vector3.Distance(transform.position, player.transform.position) > 1)
        //{
        //    mouseFollowing(player.position);

        //    rotSpeed = 8;
        //    transform.position = Vector3.Lerp(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        //}

        //if (Vector3.Distance(transform.position, player.transform.position) > 1)
        //{
        //    mouseFollowing(player.position);
        //}
        //if (catRunActive)
        //{
        //    catRigidbody.velocity = transform.forward * moveSpeed;
        //}
        //else
        //{
        //    catRigidbody.velocity = Vector3.zero;
        //}
    }

    public void Move2()
    {
        //float speedFactor;
        if (first)
        {
            first = false;

            rotSpeed = 20;

            StartCoroutine(firstMove());
        }
   

        transform.Translate(transform.forward * Time.deltaTime * moveSpeed );

        if (player.transform.position.z - transform.position.z < 5)
        {
            mouseFollowing(player.position);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z), Time.deltaTime * 100);
        }

        if (Vector3.Distance(player.transform.position, transform.position) < 2f && catching)
        {
            //transform.GetComponent<Rigidbody>().AddForce(transform.up * 50000);
            Debug.Log("yakalandin");
            //animator.SetTrigger("jump");
            currentBehaviour = States.caught;
            StartCoroutine(caughting());
            FindObjectOfType<PathFollow>().speed = 0;
            GetComponent<Collider>().isTrigger = false;
            //transform.GetComponent<ManRagdoll>().humansRagdoll2(true);
            catching = false;
            if (!Globals.finish)
            {
                player.GetComponent<ManRagdoll>().playerRagdoll(true);

                GameManager.Instance.Notify_LoseObservers();
            }
            Globals.finish = true;
        }
    }
    IEnumerator caughting()
    {
        rotSpeed = 20;
        moveSpeed = 5;
        float counter = 0f;
        while(counter <1f)
        {
            counter += Time.deltaTime;
            moveSpeed = 5 -  5 * counter;
            animator.speed = moveSpeed / 5;
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
            yield return null;
        }
        animator.speed = 1;
        angrySelection = Random.Range(0, 4);
        animator.SetTrigger(angryAnim[angrySelection]);
    }

    public void Move3()
    {
        mouseFollowing(clothesTarget.transform.position);
    }
}
