using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class reloader : MonoBehaviour
{
    public void Replay()
    {
        Debug.Log("wow");
        SceneManager.LoadScene("game");

    }
}
