using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    public Button button;

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
