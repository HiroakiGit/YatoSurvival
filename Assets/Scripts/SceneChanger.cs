using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public string sceneName;
    public Button button;

    public void SceneChange()
    {
        SceneManager.LoadScene(sceneName);

        if(button != null)
        {
            button.interactable = false;
        }
    }
}
