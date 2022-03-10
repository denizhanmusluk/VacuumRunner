using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static UnityEvent characterEvent;
    // Start is called before the first frame update
    private void Awake()
    {
        if (characterEvent == null)
        {
            characterEvent = new UnityEvent();
        }
    }
    private void LateUpdate()
    {
        characterEvent.Invoke();
    }
}        
    
