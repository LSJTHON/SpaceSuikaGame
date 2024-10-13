using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanetManager : Singleton<PlanetManager>
{
    [Header("[PlanetSetting]")]
    [SerializeField] private int planetCount = 1;
    [SerializeField] private List<GameObject> planetPrefabList = new List<GameObject>();
    [SerializeField] private Button restartButton;
    [SerializeField] private Transform nextPlanetDisplayPoint;
    [SerializeField] private Transform planetLaunchPoint;
    [SerializeField] private GameObject waitingPlanet;
    [SerializeField] private GameObject firePlanet;

    [Header("[ScoreSetting]")]
    [SerializeField] private int totalScore = 0;
    public TextMeshProUGUI scoreText;

    [Header("[PlaySetting]")]
    [SerializeField] private GameObject gameOverTargetHole;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Transform deadPlanetTransform;
    [SerializeField] private DragAndFire dragAndFire;
    [SerializeField] private Transform deadLine;
    private Animator deadAnimation;
    public bool isDead = false;

    #region Getter and setter
    public void SetScore(int score)
    {
        if (totalScore >= 0)
        {
            totalScore += score;
        }
    }
    public int GetScore()
    {
        return totalScore;
    }
    public void SetPlanetCount()
    {
        planetCount++;
    }
    public int GetPlanetCount()
    {
        return planetCount;
    }
    public List<GameObject> GetPlanetPrefabLists()
    {
        return planetPrefabList;
    }
    public GameObject GetPlanetPrefabList(int planetIndex)
    {
        return planetPrefabList[planetIndex];
    }
    public Transform GetFirePlanetSpawnPoint()
    {
        return planetLaunchPoint;
    }
    public GameObject GetFirePlanet()
    {
        return firePlanet;
    }
    public void SetFirePlanet(GameObject fireSpawnPoint = null)
    {
        firePlanet = fireSpawnPoint;
    }
    public Transform GetDeadLine()
    {
        return deadLine;
    }
    #endregion

    private void Start()
    {
        StartGame();
        restartButton.onClick.AddListener(() =>
        {
            //씬 전환으로 재시작
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }
    private void Update()
    {
        if (deadPlanetTransform == null)
        {
            return;
        }
        if (gameOverPanel.activeSelf && gameOverTargetHole.transform.localScale.x <= deadPlanetTransform.localScale.x + 1f)
        {
            deadAnimation = gameOverTargetHole.GetComponent<Animator>();
            deadAnimation.enabled = false;
            deadPlanetTransform = null;
            //Debug.Log("한번만 호출하제이?");
        }
    }
    public IEnumerator NextPlanet(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (firePlanet == null)
        {
            waitingPlanet.transform.SetParent(planetLaunchPoint);

            // waitingPlanet -> firePlanet
            firePlanet = waitingPlanet;
            // generate new waitingPlanet
            int randomPlanetIndex = Random.Range(0, 4);
            waitingPlanet = Instantiate(planetPrefabList[randomPlanetIndex], nextPlanetDisplayPoint.transform);
            firePlanet.transform.position = planetLaunchPoint.position;
            waitingPlanet.GetComponent<Rigidbody2D>().simulated = false;
            waitingPlanet.GetComponent<CustomPlanetMovement>().enabled = false;
            waitingPlanet.GetComponent<ParticleSystem>().Stop();
        }
    }
    private void StartGame()
    {
        dragAndFire.enabled = true;
        scoreText.text = $"SCORE : {totalScore}";
        firePlanet = Instantiate(planetPrefabList[Random.Range(0, 4)], planetLaunchPoint);
        firePlanet.GetComponent<Rigidbody2D>().simulated = false;
        waitingPlanet = Instantiate(planetPrefabList[Random.Range(0, 4)], nextPlanetDisplayPoint.transform);
        waitingPlanet.GetComponent<Rigidbody2D>().simulated = false;
    }
    public void GameOver(Transform deadPlanet = null)
    {
        if (isDead && deadPlanet != null)
        {
            isDead = false;
            dragAndFire.enabled = false;
            gameOverPanel.SetActive(true);
            deadPlanetTransform = deadPlanet;
            gameOverTargetHole.transform.position = new Vector2(deadPlanetTransform.position.x, deadPlanetTransform.position.y);
            for (int fireChildIndex = 0; fireChildIndex < planetLaunchPoint.childCount; fireChildIndex++)
            {
                planetLaunchPoint.GetChild(fireChildIndex).gameObject.GetComponent<Rigidbody2D>().simulated = false;
            }
            //Debug.Log(" 너 지금 몇번 호출중?");
        }
    }
}