using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameStarting : MonoBehaviour, IWinObserver
{
    public void StartGame()
    {
        GameManager.Instance.Add_WinObserver(this);
    }
    public void WinScenario()
    {

    }
}