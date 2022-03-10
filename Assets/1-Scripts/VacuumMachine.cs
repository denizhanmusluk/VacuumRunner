using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VacuumMachine : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer vacuum1, vacuum2, vacuum3;
    public bool eyeBlink = true;
    [SerializeField] float blinkSpeed = 100;
    [SerializeField] float upScaleFactor = 6;
    [SerializeField] float downScaleFactor = 6;
    float vacumScale = 0;
    public bool scaleActive = true;
    [SerializeField] ParticleSystem effect1, effect2;
    [SerializeField] CapsuleCollider playerVacuumCollider;
    bool buttonDown;
    bool buttonUp;
    [SerializeField] GameObject effects;
    [SerializeField] Transform target1, target2;
    private void Start()
    {

    }
    private void Update()
    {
        if (Globals.isGameActive)
        {
            gameUpdate();
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
            vacuum1.SetBlendShapeWeight(0, vacumScale + blink);
            vacuum2.SetBlendShapeWeight(0, vacumScale + blink);
            vacuum3.SetBlendShapeWeight(0, vacumScale + blink);
            yield return null;
        }

        while (blink > downScaleFactor)
        {
            blink -= Time.deltaTime * blinkSpeed;
            if (blink < downScaleFactor) { blink = downScaleFactor; }
            vacuum1.SetBlendShapeWeight(0, vacumScale + blink);
            vacuum2.SetBlendShapeWeight(0, vacumScale + blink);
            vacuum3.SetBlendShapeWeight(0, vacumScale + blink);
            yield return null;
        }
        vacumScale += blink;
        scaleActive = true;
    }
    IEnumerator vacuumClosed()
    {
       
        float firstSize1 = 6f;
        float firstSize2 = 0.12f;
        //effect1.startSize = firstSize1;
        //effect2.startSize = firstSize2;
        effects.transform.localScale = new Vector3(1, 1, 1);
        effects.transform.position = target2.position;

        float counter = 0;
        yield return null;
        while (counter < 1 && buttonUp)
        {
            counter +=  Time.deltaTime;
            //effect1.startSize = (firstSize1 - (counter * firstSize1 * 5 / 6));
            //effect2.startSize = (firstSize2 - (counter * firstSize2 * 5 / 6));

            effects.transform.localScale = Vector3.MoveTowards(effects.transform.localScale, new Vector3(0.1f, 0.1f, 0.01f), 2 * Time.deltaTime);
            effects.transform.position = Vector3.MoveTowards(effects.transform.position, target1.transform.position, Time.deltaTime * 10);
            yield return null;
        }
        if (buttonUp)
        {
            effect1.Stop();
            effect2.Stop();
        }
    }

    IEnumerator vacuumOpened()
    {
        effect1.Play();
        effect2.Play();
        float firstSize1 = 1f;
        float firstSize2 = 0.02f;
        //effect1.startSize = firstSize1;
        //effect2.startSize = firstSize2;
        effects.transform.localScale = new Vector3(0.1f, 0.1f, 0.01f);
        float counter = 0;
        yield return null;
        effects.transform.position = target1.position;
        while (counter < 1 && buttonDown)
        {
            counter +=  Time.deltaTime;
            //effect1.startSize = (firstSize1 * counter * 6);
            //effect2.startSize = (firstSize2 * counter * 6);

            effects.transform.localScale = Vector3.MoveTowards(effects.transform.localScale, new Vector3(1, 1, 1), 2 * Time.deltaTime);
            effects.transform.position = Vector3.MoveTowards(effects.transform.position, target2.transform.position, Time.deltaTime * 10);

            yield return null;
        }
    
    }
    public void vacuumFinishClosed()
    {
        buttonDown = false;
        buttonUp = true;
        playerVacuumCollider.enabled = false;
        StartCoroutine(vacuumClosed());
    }
    private void gameUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            buttonDown = true;
            buttonUp = false;
            playerVacuumCollider.enabled = true;
            StartCoroutine(vacuumOpened());
        }
        if (Input.GetMouseButton(0))
        {

        }
        if (Input.GetMouseButtonUp(0))
        {
            buttonDown = false;
            buttonUp = true;
            playerVacuumCollider.enabled = false;
            StartCoroutine(vacuumClosed());
        }
    }
}
