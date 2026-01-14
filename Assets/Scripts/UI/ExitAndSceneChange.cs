using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitAndSceneChange : MonoBehaviour
{
    public void ExitGameFunction()
    {
        Debug.Log("game exit");
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("mainMenu");
        GameManager.Instance.ResetGame();
    }

}
