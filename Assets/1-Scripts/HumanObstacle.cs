using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HumanObstacle : MonoBehaviour
{
    [SerializeField] GameObject pointPrefab;
    TextMeshProUGUI scoreText;
    private void Start()
    {
        scoreText = GameObject.Find("ScoreText").transform.GetComponent<TextMeshProUGUI>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "human")
        {
            //GameManager.Instance.Remove_WinObserver(collision.transform.GetComponent<Pursuit>());
            Destroy(collision.transform.GetComponent<Pursuit>());

            collision.transform.GetComponent<ManRagdoll>().humansRagdollObstacle(true);
            //Instantiate(pointPrefab, collision.transform.position + new Vector3(0, 0, 2), Quaternion.identity);
            //Globals.score++;
            //scoreText.text = Globals.score.ToString();
        }
    }
}
