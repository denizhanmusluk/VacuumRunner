using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Score : MonoBehaviour
{
    public static Score Instance;
    protected TextMeshProUGUI[] childText;
    protected Image[] childImage;
    Image scoreImage;
    TextMeshProUGUI scoreText;
    float firstScale;
    void Start()
    {
        Globals.score = 0;

        childText = GetComponentsInChildren<TextMeshProUGUI>();
        childImage = GetComponentsInChildren<Image>();
        if (Instance == null)
        {
            Instance = this;
        }
        foreach (var chImg in childImage)
        {
            scoreImage = chImg;
        }
        foreach (var chText in childText)
        {
            scoreText = chText;
        }
        firstScale = scoreImage.rectTransform.localScale.x;
    }

    void Update()
    {

    }
    public void scoreUp()
    {
        Debug.Log("acore");
        Globals.score++;
        scoreText.text = Globals.score.ToString();
        StartCoroutine(scaleImage());
    }
    
    IEnumerator scaleImage()
    {
        float counter = 0;
        float scale = 0;
        while (counter < Mathf.PI)
        {
            counter += 20 * Time.deltaTime;
            scale = Mathf.Sin(counter);
            scale *= 0.25f;
            scoreImage.rectTransform.localScale = new Vector3(firstScale + scale, firstScale + scale, firstScale + scale);
            //transform.rotation = Quaternion.Euler(angle * vect.x, angle * vect.y, angle * vect.z);

            yield return null;
        }
        scoreImage.rectTransform.localScale = new Vector3(firstScale, firstScale, firstScale);
    }
    public void scoreMultple(int multiple)
    {

        StartCoroutine(scoreSet(multiple));
    }
    IEnumerator scoreSet(int multiple)
    {
        int counter = Globals.score;

        while (counter < Globals.score * multiple)
        {
            counter++;

            StartCoroutine(scaleImage());
            scoreText.text = counter.ToString();

            yield return new WaitForSeconds(0.04f);
        }
    }

}