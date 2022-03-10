using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;


public class Finish : MonoBehaviour
{
    [SerializeField] GameObject clothesParent, sprayTarget;
    int clothesCount;
    [SerializeField] float attackSpeed = 1;
    [SerializeField] GameObject[] clothesPrefab;
    int clothesSelectionCount;
    float sprayRange = 1.3f;
    [SerializeField] GameObject machina;
    [SerializeField] SkinnedMeshRenderer macCase, macCap;

    [SerializeField] float blinkSpeed = 100;
    [SerializeField] float upScaleFactor = 6;
    [SerializeField] float downScaleFactor = 6;
    float vacumScale = 0;
    public bool scaleActive = true;
    float machineFirstScale;
    [SerializeField] CinemachineVirtualCamera cam;
    int thrownClothesCount = 0;
    Animator anim;
    protected Rigidbody[] childrenClothes;
    GameObject player;
    public int scoreMagnitude = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Transform playerRotPart;
    public Image progressBar;
    public GameObject finScoreBar;
    public GameObject star1,star2,star3;
    public GameObject star1Particle, star2Particle, star3Particle;
    public GameObject nextButton;
    [SerializeField] GameObject amazingImagePrefab;
    private void Start()
    {
        finScoreBar.SetActive(false);
        anim = machina.GetComponent<Animator>();
        machineFirstScale = machina.transform.localScale.x;
        clothesSelectionCount = clothesPrefab.Length;
        //cam = GameObject.Find("CameraMain").GetComponent<CinemachineVirtualCamera>();
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "followobject")
    //    {
    //        Globals.isGameActive = false;
    //        other.GetComponentInParent<PathFollow>().speed = 120;
    //        //cam.Follow = other.gameObject.transform;
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            clothesCount = clothesParent.transform.childCount;
            Globals.isGameActive = false;
            FindObjectOfType<PathFollow>().speed = 0;
            //other.transform.GetChild(0).GetChild(0).gameObject.GetComponent<IHit>().finish(gameObject);
            other.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("idle");
            transform.GetComponent<Collider>().enabled = false;
            StartCoroutine(clothesAttack());
            //playerRotPart.LookAt(sprayTarget.transform);
            StartCoroutine(playerRotation());
            other.transform.GetChild(0).GetChild(0).GetComponent<ManRagdoll>().player = true;
            GameManager.Instance.Notify_GameFinishObservers();
            
        }
        if (other.gameObject.tag == "human")
        {
            other.transform.GetComponent<Pursuit>().currentBehaviour = Pursuit.States.caught;
            other.transform.GetComponent<ManRagdoll>().humansRagdoll(true);
        }
    }
    IEnumerator playerRotation()
    {
        float counter = 0f;
        while (counter < 3)
        {
            counter += Time.deltaTime;
            Vector3 relativeVector = playerRotPart.transform.InverseTransformPoint(sprayTarget.transform.position);
            relativeVector /= relativeVector.magnitude;
            float newSteer = (relativeVector.z / relativeVector.magnitude);
            playerRotPart.transform.Rotate(newSteer * Time.deltaTime * 50 * 5, 0, 0);
            yield return null;
        }
    }
    IEnumerator clothesAttack()
    {
        anim.SetTrigger("open");
        for (int i = 0; i < clothesCount; i++)
        {
            StartCoroutine(sprayingClothes());
            yield return new WaitForSeconds(attackSpeed);
        }
        Debug.Log("deneme");
    }

    IEnumerator sprayingClothes()
    {
        cam.Priority = 20;

        int select = Random.Range(0, clothesSelectionCount);

        float counter = 0;

        GameObject currentClothes = Instantiate(clothesPrefab[select], clothesParent.transform.position, Quaternion.identity);
        //currentClothes.SetActive(true);
        currentClothes.transform.localScale = new Vector3(0.6f, 0.6f, 4);
        Vector3 targetAdd = new Vector3(Random.Range(-sprayRange, sprayRange), Random.Range(-sprayRange, sprayRange),0);
        while (Vector3.Distance(currentClothes.transform.position, sprayTarget.transform.position + targetAdd) > 0.5f)
        {
            counter += Time.deltaTime;
            currentClothes.transform.localScale = Vector3.Lerp(currentClothes.transform.localScale, new Vector3(1, 1, 1), 2 * Time.deltaTime);

            currentClothes.transform.LookAt(sprayTarget.transform);
            currentClothes.transform.position = Vector3.MoveTowards(currentClothes.transform.position, sprayTarget.transform.position + targetAdd, (1 + counter * 5) * 20 * Time.deltaTime);
            yield return null;
        }
        currentClothes.transform.parent = sprayTarget.transform;
        currentClothes.gameObject.AddComponent<Rigidbody>();
        currentClothes.gameObject.GetComponent<Rigidbody>().mass = 0f;
        currentClothes.gameObject.GetComponent<Rigidbody>().drag = 0f;
        currentClothes.gameObject.AddComponent<BoxCollider>();
        setScale();
        //currentClothes.transform.parent = null;
        //currentClothes.SetActive(false);
        //Destroy(currentClothes);
        //sprayTarget.transform.parent.GetComponent<VacuumMachine>().setScale();
        thrownClothesCount++;
        if (thrownClothesCount >= clothesCount)
        {
            StartCoroutine(machRun());
            Debug.Log("deneme2222");
            FindObjectOfType<VacuumMachine>().vacuumFinishClosed();
        }
    }
    public void setScale()
    {
        if (scaleActive)
        {
            scaleActive = false;
            StartCoroutine(_blink());
        }
    }

    IEnumerator _blink()
    {
        float blink = 0f;
        while (blink < upScaleFactor)
        {
            blink += Time.deltaTime * blinkSpeed;
            if (blink > upScaleFactor) { blink = upScaleFactor; }
            macCase.SetBlendShapeWeight(0, vacumScale + blink);
            macCap.SetBlendShapeWeight(0, vacumScale + blink);
            machina.transform.localScale = new Vector3(machineFirstScale + macCase.GetBlendShapeWeight(0) / 200, machineFirstScale + macCase.GetBlendShapeWeight(0) / 200, machineFirstScale + macCase.GetBlendShapeWeight(0) / 200);
            yield return null;
        }

        while (blink > downScaleFactor)
        {
            blink -= Time.deltaTime * blinkSpeed;
            if (blink < downScaleFactor) { blink = downScaleFactor; }
            macCase.SetBlendShapeWeight(0, vacumScale + blink);
            macCap.SetBlendShapeWeight(0, vacumScale + blink);
            yield return null;
        }
        vacumScale += blink;
        scaleActive = true;
    }
    void machineForce()
    {
     

        machina.transform.GetComponent<Rigidbody>().useGravity = true;
        machina.transform.GetComponent<Rigidbody>().AddForce(-machina.transform.forward * 1000 * clothesCount);
        machina.transform.GetComponent<Rigidbody>().AddForce(machina.transform.up * 300);

        //macCap.gameObject.AddComponent<Rigidbody>();
        //macCap.gameObject.AddComponent<BoxCollider>();
        //macCap.transform.GetComponent<Rigidbody>().AddForce(-machina.transform.forward * 5000);
        StartCoroutine(checkFinish());
    
    }
    IEnumerator machRun()
    {
        if ((float)Globals.score / (float)Globals.maxScore >= 0.9f)
        {
            Instantiate(amazingImagePrefab, machina.transform.position + new Vector3(0, 0, -8), Quaternion.identity);
        }
        anim.SetTrigger("close");
        anim.SetTrigger("run");
        yield return new WaitForSeconds(3f);
        anim.SetTrigger("open");

        StartCoroutine(extinguishing());
    }
    IEnumerator extinguishing()
    {
        Debug.Log("finishh");
     
        anim.enabled = false;

        //sprayTarget.transform.parent = null;
        childrenClothes = sprayTarget.GetComponentsInChildren<Rigidbody>();
        machineForce();
        //sprayTarget.GetComponent<Collider>().enabled = false;

        foreach (var clothes in childrenClothes)
        {
            clothes.GetComponent<Collider>().isTrigger = true;
            clothes.AddForce(machina.transform.forward * 200);
        }

        float blink = 100;
        while (blink > -50)
        {
            blink -= 5 * Time.deltaTime * blinkSpeed;
            if (blink < -50) { blink = -50; }
            macCase.SetBlendShapeWeight(0, blink);
            macCap.SetBlendShapeWeight(0, blink);
            machina.transform.localScale = Vector3.Lerp(machina.transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), Time.deltaTime * 20);

            yield return null;
        }
        while (blink < 0)
        {
            blink += 5 * Time.deltaTime * blinkSpeed;
            if (blink > 0) { blink = 0; }
            macCase.SetBlendShapeWeight(0, blink);
            macCap.SetBlendShapeWeight(0, blink);
            //machina.transform.localScale = Vector3.Lerp(machina.transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), Time.deltaTime * 20);

            yield return null;
        }
        //childrenClothes = sprayTarget.GetComponentsInChildren<Transform>();

        //foreach (var clothes in childrenClothes)
        //{
        //    clothes.gameObject.AddComponent<Rigidbody>();
        //    clothes.gameObject.AddComponent<BoxCollider>();
        //}
    }
    IEnumerator scattering()
    {
        for (int i = 0; i < sprayTarget.transform.childCount; i++)
        {
            StartCoroutine(scatteringClothes(sprayTarget.transform.GetChild(i).gameObject));
            yield return new WaitForSeconds(attackSpeed);
        }
    }

    IEnumerator scatteringClothes(GameObject currentClothes)
    {
        float counter = 0;
        Vector3 targetAdd = new Vector3(Random.Range(-sprayRange, sprayRange), Random.Range(-sprayRange, sprayRange), 0);
        while (Vector3.Distance(currentClothes.transform.position, player.transform.position + targetAdd) > 0.5f)
        {
            counter += Time.deltaTime;
            currentClothes.transform.localScale = Vector3.Lerp(currentClothes.transform.localScale, new Vector3(0.6f, 0.6f, 4), 50 * Time.deltaTime);

            currentClothes.transform.LookAt(sprayTarget.transform);
            currentClothes.transform.position = Vector3.MoveTowards(currentClothes.transform.position, player.transform.position + targetAdd, (1 + counter * 5) * 20 * Time.deltaTime);
            yield return null;
        }
    }
    
    IEnumerator checkFinish()
    {
        float counter = 0f;
        while (counter < 0.5f)
        {
            counter += Time.deltaTime;
            macCap.transform.localRotation = Quaternion.RotateTowards(macCap.transform.localRotation, Quaternion.Euler(macCap.transform.localEulerAngles.x, macCap.transform.localEulerAngles.y + 5, macCap.transform.localEulerAngles.z), Time.deltaTime * 3000);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        counter = 0f;
        while (counter < 0.5f)
        {
            counter += Time.deltaTime;
            macCap.transform.localRotation = Quaternion.RotateTowards(macCap.transform.localRotation, Quaternion.Euler(macCap.transform.localEulerAngles.x, 0, macCap.transform.localEulerAngles.z), Time.deltaTime * 3000);
            yield return null;
        }
        yield return new WaitForSeconds(3);
        GameManager.Instance.Notify_WinObservers();
        finScoreBar.SetActive(true);

        StartCoroutine(scoreBar());
        StartCoroutine(scoreMult());
    }
    IEnumerator scoreBar()
    {
        float currentBar = 0;
        int score = Globals.score;
        //int newScore = scoreMagnitude * score;
        while (currentBar <= score)
        {
            currentBar += 40 * Time.deltaTime;
            float scoreRatio = (float)currentBar / (float)Globals.maxScore;
           
            progressBar.fillAmount = scoreRatio;

            if (scoreRatio >= 0.95f)
            {
                star1.SetActive(false);
                star2.SetActive(false);
                star3.SetActive(false);
                star3Particle.SetActive(true);

            }
            else if (scoreRatio >= 0.75f)
            {
                star1.SetActive(false);
                star2.SetActive(false);
                star2Particle.SetActive(true);

            }
            else if (scoreRatio >= 0.5f)
            {
                star1.SetActive(false);
                star1Particle.SetActive(true);
            }
            yield return null;
        }
        nextButton.SetActive(true);
    }
    IEnumerator scoreMult()
    {
        int score = Globals.score;
        int newScore = scoreMagnitude * score;
        int sign = -1;
        int magn = -1;

        while (score <= newScore)
        {
            magn = sign * magn;
            score += scoreMagnitude;
            Globals.score = score;

            scoreText.text = Globals.score.ToString();
            scoreText.rectTransform.localScale = new Vector3(1 + magn / 2, 1 + magn / 2, 1);
            yield return null;
        }
    }
}
