using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
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
    private Animator deadAnimation;
    private bool isStopAnimation = false;
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
    #endregion

    private void Start()
    {
        totalScore = 0;
        StartGame();

        restartButton.onClick.AddListener(() => {

            for (int waitingChildIndex = 0; waitingChildIndex < nextPlanetDisplayPoint.childCount; waitingChildIndex++)
            {
                Destroy(nextPlanetDisplayPoint.GetChild(waitingChildIndex).gameObject);
            }
            for (int fireChildIndex = 0; fireChildIndex < planetLaunchPoint.childCount; fireChildIndex++)
            {
                Destroy(planetLaunchPoint.GetChild(fireChildIndex).gameObject);
            }

            if (isStopAnimation)
            {
                gameOverTargetHole.transform.localScale = new Vector2(33,33);
                deadAnimation.enabled = true;
            }

            gameOverPanel.SetActive(false);
            totalScore = 0;
            StartGame();
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
            isStopAnimation = true;
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
            waitingPlanet.GetComponent<PlanetEffect>().enabled = false;
            waitingPlanet.GetComponent<ParticleSystem>().Stop();
        }
    }
    private void StartGame()
    {
        dragAndFire.enabled = true;
        scoreText.text = $"Score : {totalScore}";
        firePlanet = Instantiate(planetPrefabList[Random.Range(0, 4)], planetLaunchPoint);
        firePlanet.GetComponent<Rigidbody2D>().simulated = false;
        firePlanet.GetComponent<ParticleSystem>().Stop();
        waitingPlanet = Instantiate(planetPrefabList[Random.Range(0, 4)], nextPlanetDisplayPoint.transform);
        waitingPlanet.GetComponent<Rigidbody2D>().simulated = false;
        waitingPlanet.GetComponent<ParticleSystem>().Stop();
    }
    public void GameOver(float delay = 0.1f, Transform deadPlanet = null)
    {
        if (deadPlanet != null)
        {
            deadPlanetTransform = deadPlanet;
        }
        if (isDead)
        {
            isDead = false;
            for (int waitingChildIndex = 0; waitingChildIndex < nextPlanetDisplayPoint.childCount; waitingChildIndex++)
            {
                planetLaunchPoint.GetChild(waitingChildIndex).gameObject.GetComponent<Rigidbody2D>().simulated = false;
            }
            for (int fireChildIndex = 0; fireChildIndex < planetLaunchPoint.childCount; fireChildIndex++)
            {
                planetLaunchPoint.GetChild(fireChildIndex).gameObject.GetComponent<Rigidbody2D>().simulated = false;
            }
            gameOverPanel.SetActive(true);
            dragAndFire.enabled = false;
            gameOverTargetHole.transform.position = new Vector2(deadPlanetTransform.position.x, deadPlanetTransform.position.y);
            isStopAnimation = false;
            //Debug.Log(" 너 지금 몇번 호출중?");
        }
    }
}