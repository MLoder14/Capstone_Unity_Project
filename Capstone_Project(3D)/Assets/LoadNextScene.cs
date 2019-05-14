using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    /// <summary>
    /// this function loads the next scene
    /// </summary>
    public void LoadNextLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
