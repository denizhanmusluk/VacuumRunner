using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{
    [SerializeField] GameObject[] level1Prefab;
    [SerializeField] int level;
    void Start()
    {
        //PlayerPrefs.SetInt("level", Globals.currentLevel);
        level = PlayerPrefs.GetInt("level");
        Globals.currentLevel = PlayerPrefs.GetInt("level");

        if(level1Prefab.Length <= level)
        {
            Globals.currentLevel = 0;
            level = 0;
            PlayerPrefs.SetInt("level", Globals.currentLevel);
        }
        level1Prefab[level].SetActive(true);
    }
}            

