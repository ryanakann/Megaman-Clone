using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


// public delegate void 

public class MenuManager : MonoBehaviour {

    public UnityEvent OnHealthChange;

    public void StartGame () {
        SceneManager.LoadScene(1);
    }
}