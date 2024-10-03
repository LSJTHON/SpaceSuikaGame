using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]private Button restartButton;

    private string sceneName = "MainScene";
    private void Start() {
        restartButton.onClick.AddListener(()=>{
            Debug.Log("클릭쓰!");
            SceneManager.LoadScene(sceneName);
        });
    }
}
