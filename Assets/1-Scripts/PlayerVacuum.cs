using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerVacuum : MonoBehaviour, IHit
{
    // Start is called before the first frame update
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.transform.tag == "human")
    //    {
    //        collision.transform.GetComponent<Pursuit>().currentBehaviour = Pursuit.States.StartFollowing;

    //    }
    //}
    [SerializeField] GameObject pointPrefab;
    TextMeshProUGUI scoreText;
    [SerializeField] GameObject speedEffect;
    [SerializeField] Transform target;
    [SerializeField] Transform noneTarget;
    //Vector3 noneTarget;
    [SerializeField] public Transform rotatingPart;
    [Range(0, 50)]
    [SerializeField]
    public float rotSpeed;
    float normalSpeed;
    Animator anim;
    [SerializeField] GameObject[] humansParticles, playerParticles;
    int playerChilCount;
    [SerializeField] int totalClothes;
    public AudioSource audio;
    [SerializeField] AudioClip walk;
    public bool walkSound = true;
    public float walkSpeed;
    private void Start()
    {
        Globals.score = 0;
        audio = GetComponent<AudioSource>();
        audio.clip = walk;

        Globals.maxScore = totalClothes;
        scoreText = GameObject.Find("ScoreText").transform.GetComponent<TextMeshProUGUI>();

        speedEffect.SetActive(false);
        playerChilCount = transform.childCount;

        anim = transform.GetComponent<Animator>();
        target.position = noneTarget.position;
    }
    public void movemenet()
    {
        StartCoroutine(walking());
    }
    IEnumerator walking()
    {
        while (walkSound)
        {
            Debug.Log("walk");
            audio.Play();

            yield return new WaitForSeconds(1);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "human")
        {
            //if (other.GetComponent<Pursuit>().follow)
            //{
                
            //    other.transform.GetComponent<Pursuit>().currentBehaviour = Pursuit.States.vacuum;
            //    other.transform.GetComponent<Pursuit>()._firstMoving();
            //    //other.transform.GetComponent<Pursuit>().vacuumClothes();
            //    other.GetComponent<Pursuit>().follow = false;
            //    StartCoroutine(setTarget(other.transform));
            //}
            //else
            //{
            //    //yakalandigi durum
            //}

        }
      
    }
    //private void Update()
    //{
    //    playerRotation();
    //}
    IEnumerator setTarget(Transform _target, float timing)
    {
        float counter = 0;
        while (counter < timing)
        {
            target = _target;

            counter += Time.deltaTime;
            yield return null;
        }
        target = noneTarget;
    }
    private void Update()
    {
        playerRotation();
    }
    void playerRotation()
    {
        Vector3 relativeVector = rotatingPart.transform.InverseTransformPoint(target.position);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.z / relativeVector.magnitude);
        rotatingPart.transform.Rotate(newSteer * Time.deltaTime * 50 * rotSpeed, 0, 0);
    }
    public void obstacleHit(GameObject obs, float spd)
    {
        StartCoroutine(obsHit(spd));
    }
    IEnumerator obsHit(float spd)
    {
        int timing = 0;
        if (spd > 10)
        {
            speedEffect.SetActive(true);
            timing = 3;
        }
        else
        {
            timing = 2;
        }
        float animFirstSpeed = anim.speed;
        anim.speed = animFirstSpeed * spd / 10;
        normalSpeed = FindObjectOfType<PathFollow>().speed;
        FindObjectOfType<PathFollow>().speed = spd;
        walkSpeed = spd;
        yield return new WaitForSeconds(timing);
        anim.speed = animFirstSpeed;
        FindObjectOfType<PathFollow>().speed = normalSpeed;
        walkSpeed = normalSpeed;
        speedEffect.SetActive(false);
    }
    public void humanHit(GameObject human)
    {
        if (human.GetComponent<Pursuit>().follow)
        {
            Globals.score++;
            scoreText.text = Globals.score.ToString() + "/" + totalClothes.ToString();
            Instantiate(pointPrefab, human.transform.position + new Vector3(0, 0, 2), Quaternion.identity);

            human.transform.GetComponent<Pursuit>().currentBehaviour = Pursuit.States.vacuum;
            human.transform.GetComponent<Pursuit>()._firstMoving();
            //other.transform.GetComponent<Pursuit>().vacuumClothes();
            human.GetComponent<Pursuit>().follow = false;
            StartCoroutine(setTarget(human.transform, 0.5f));

            int selectionHumanEmoji = Random.Range(0, humansParticles.Length);
            GameObject emoji = Instantiate(humansParticles[selectionHumanEmoji], human.transform.position, Quaternion.identity);

            emoji.transform.parent = human.transform;
            emoji.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            emoji.transform.position += new Vector3(0, 2.8f, 0);
            if (playerChilCount == transform.childCount)
            {
                int selectionPlayerEmoji = Random.Range(0, playerParticles.Length);
                GameObject emojiPlayer = Instantiate(playerParticles[selectionPlayerEmoji], transform.position, Quaternion.identity);

                emojiPlayer.transform.parent = transform;
                emojiPlayer.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                emojiPlayer.transform.position += new Vector3(0, 2.8f, 0);
            }
        }
        else
        {
            //yakalandigi durum
        }
    }
    public void gameOver(GameObject obs)
    {
        FindObjectOfType<PathFollow>().speed = 0;
        transform.GetComponent<ManRagdoll>().playerRagdoll(true);
        GameManager.Instance.Notify_LoseObservers();
    }
    public void finish(GameObject obs)
    {
        GameManager.Instance.Notify_WinObservers();
    }
    public void poleHit(GameObject obs)
    {
        StartCoroutine(setTarget(obs.transform, 0.75f));
    }
}
