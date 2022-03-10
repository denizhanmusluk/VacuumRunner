using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManRagdoll : MonoBehaviour, IFinish
{
    protected Animator animator;
    protected Rigidbody _Rigidbody;
    protected Collider playerCollider;
    protected Collider[] childrenCollider;
    protected Rigidbody[] childrenRigidbody;

    //public AudioClip matadorHit;
    //AudioSource audioSource;
    bool ragdoll = false;
    public bool player = false;
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            humansRagdoll(true);
        }
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
        _Rigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        childrenCollider = GetComponentsInChildren<Collider>();
        childrenRigidbody = GetComponentsInChildren<Rigidbody>();
    }
    private void Start()
    {
        GameManager.Instance.Add_FinishObserver(this);
        startRagdoll(false);
        //animator.enabled = false;

        //StartCoroutine(animSet());
    }

    public void startRagdoll(bool active)
    {
        foreach (var rigidb in childrenRigidbody)
        {
            rigidb.detectCollisions = true;
            rigidb.isKinematic = !active;
            rigidb.useGravity = active;
            rigidb.drag = 1;
        }

        //rest
        animator.enabled = !active;
        _Rigidbody.detectCollisions = !active;
        //Rigidbody.isKinematic = !active;
        //Rigidbody.useGravity = !active;
        //playerCollider.enabled = !active;
        if (ragdoll)
        {
            StartCoroutine(physicsSet());
        }
        ragdoll = true;
    }
    public void humansRagdollObstacle(bool active)
    {
      
            foreach (var rigidb in childrenRigidbody)
            {
                rigidb.isKinematic = !active;
                rigidb.useGravity = active;
                rigidb.drag = 0.1f;
                rigidb.mass = 0.1f;
                rigidb.constraints = RigidbodyConstraints.None;
                //rigidb.AddForce((transform.up * 5 + transform.forward) * 1000);
            animator.enabled = !active;
            _Rigidbody.constraints = RigidbodyConstraints.None;
            playerCollider.enabled = !active;
        }
    }
    public void humansRagdoll2(bool active)
    {
        if (!player)
        {
            StartCoroutine(humanRagForce(active));
        }
    }
    IEnumerator humanRagForce(bool active)
    {
        yield return new WaitForSeconds(1);
        GameObject player = GameObject.Find("Players").transform.GetChild(0).GetComponent<PlayerVacuum>().rotatingPart.gameObject;
        Rigidbody pelvis = player.GetComponent<Rigidbody>();
        foreach (var rigidb in childrenRigidbody)
        {
            rigidb.isKinematic = !active;
            rigidb.useGravity = active;
            rigidb.drag = 0f;
            rigidb.mass = 6;
            rigidb.constraints = RigidbodyConstraints.None;
            //rigidb.AddForce((transform.up * 5 + transform.forward) * 1000);

        }
        animator.enabled = !active;
        _Rigidbody.constraints = RigidbodyConstraints.None;
        _Rigidbody.useGravity = false;

        playerCollider.enabled = !active;
        yield return null;
        foreach (var rigidb in childrenRigidbody)
        {
            if (rigidb.transform.name == "Base HumanPelvis")
            {
                pelvis = rigidb;
                pelvis.transform.parent = null;
                //pelvis.AddForce((player.transform.position - pelvis.transform.position).normalized * 10000 + (transform.up * 1.4f) * 10000);
            }
        }
        Destroy(GetComponent<Pursuit>());

    }
    public void humansRagdoll(bool active)
    {
        if (!player)
        {
            StartCoroutine(humanForce(active));
        }
    }
    IEnumerator humanForce(bool active)
    {
        GameObject player = GameObject.Find("Players").transform.GetChild(0).GetComponent<PlayerVacuum>().rotatingPart.gameObject;
        Rigidbody pelvis = player.GetComponent<Rigidbody>();
        foreach (var rigidb in childrenRigidbody)
        {
            rigidb.isKinematic = !active;
            rigidb.useGravity = active;
            rigidb.drag = 0f;
            rigidb.mass = 6;
            rigidb.constraints = RigidbodyConstraints.None;
            //rigidb.AddForce((transform.up * 5 + transform.forward) * 1000);

        }
        animator.enabled = !active;
        _Rigidbody.constraints = RigidbodyConstraints.None;
        _Rigidbody.useGravity = false;

        playerCollider.enabled = !active;
        yield return null;
        foreach (var rigidb in childrenRigidbody)
        {
            if (rigidb.transform.name == "Base HumanPelvis")
            {
                pelvis = rigidb;
                pelvis.transform.parent = null;
                //pelvis.AddForce((player.transform.position - pelvis.transform.position).normalized * 10000 + (transform.up * 1.4f) * 10000);
            }
        }
        Destroy(GetComponent<Pursuit>());

    }
    public void playerRagdoll(bool active)
    {
        foreach (var rigidb in childrenRigidbody)
        {
            //rigidb.detectCollisions = active;
            rigidb.isKinematic = !active;
            rigidb.useGravity = active;
            rigidb.drag = 1;
            rigidb.constraints = RigidbodyConstraints.None;

        }

        //rest
        animator.enabled = false;
        _Rigidbody.constraints = RigidbodyConstraints.None;

        _Rigidbody.detectCollisions = !active;
        _Rigidbody.useGravity = false;
        playerCollider.enabled = false;
        //StartCoroutine(playerRag());
    }
    IEnumerator playerRag()
    {
        yield return new WaitForSeconds(4f);
        foreach (var rigidb in childrenRigidbody)
        {
            rigidb.detectCollisions = false;
            rigidb.isKinematic = true;
            rigidb.useGravity = false;
            rigidb.drag = 1;
        }

        //rest
        animator.enabled = false;
        _Rigidbody.detectCollisions = true;
        _Rigidbody.useGravity = false;
        playerCollider.enabled = false;
    }
    IEnumerator physicsSet()
    {
        yield return null;
        _Rigidbody.useGravity = false;
        playerCollider.enabled = false;
    }
    IEnumerator animSet()
    {
        yield return new WaitForSeconds(0.1f);
        animator.enabled = false;
    }
}
