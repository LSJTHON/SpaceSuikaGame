using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetManager : Singleton<PlanetManager>
{
    [Header("[PlanetSetting]")]
    [SerializeField] private int planetCount = 1;
    [SerializeField] private List<GameObject> planetPrefabList = new List<GameObject>();
    [SerializeField] private Button restartButton;
    [SerializeField] private Transform waitingPlanetSpawnPint;
    [SerializeField] private Transform firePlanetSpawnPoint;
    [SerializeField] private GameObject waitingPlanet;
    [SerializeField] private GameObject firePlanet;

    [Header("[ScoreSetting]")]
    [SerializeField] private int totalScore = 0;
    public TextMeshProUGUI scoreText;

    [Header("[PlaySetting]")]
    [SerializeField] private GameObject gameOverTargetHole;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Transform deadPlanetTransform;
    private Animator deadAnimation;


    public bool isDead = false;
    private bool isStopAnimation = false;

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
        return firePlanetSpawnPoint;
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

            for (int waitingChildIndex = 0; waitingChildIndex < waitingPlanetSpawnPint.childCount; waitingChildIndex++)
            {
                Destroy(waitingPlanetSpawnPint.GetChild(waitingChildIndex).gameObject);
            }
            for (int fireChildIndex = 0; fireChildIndex < firePlanetSpawnPoint.childCount; fireChildIndex++)
            {
                Destroy(firePlanetSpawnPoint.GetChild(fireChildIndex).gameObject);
            }

            if (isStopAnimation)
            {
                gameOverTargetHole.transform.localScale = new Vector2(33,33);
                deadAnimation.enabled = true;
            }

            gameOverPanel.SetActive(false);
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
            //gameOverTargetHole.transform.position = new Vector2(deadPlanetTransform.position.x, deadPlanetTransform.position.y);
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
            waitingPlanet.transform.SetParent(firePlanetSpawnPoint);

            // waitingPlanet -> firePlanet
            firePlanet = waitingPlanet;
            // generate new waitingPlanet
            int randomPlanetIndex = Random.Range(0, 4);
            waitingPlanet = Instantiate(planetPrefabList[randomPlanetIndex], waitingPlanetSpawnPint.transform);
            firePlanet.transform.position = firePlanetSpawnPoint.position;
            waitingPlanet.GetComponent<Rigidbody2D>().simulated = false;
            waitingPlanet.GetComponent<PlanetEffect>().enabled = false;
        }
    }
    private void StartGame()
    {
        scoreText.text = $"Score : {totalScore}";
        firePlanet = Instantiate(planetPrefabList[Random.Range(0, 4)], firePlanetSpawnPoint);
        firePlanet.GetComponent<Rigidbody2D>().simulated = false;
        waitingPlanet = Instantiate(planetPrefabList[Random.Range(0, 4)], waitingPlanetSpawnPint.transform);
        waitingPlanet.GetComponent<Rigidbody2D>().simulated = false;
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
            for (int waitingChildIndex = 0; waitingChildIndex < waitingPlanetSpawnPint.childCount; waitingChildIndex++)
            {
                firePlanetSpawnPoint.GetChild(waitingChildIndex).gameObject.GetComponent<Rigidbody2D>().simulated = false;
            }
            for (int fireChildIndex = 0; fireChildIndex < firePlanetSpawnPoint.childCount; fireChildIndex++)
            {
                firePlanetSpawnPoint.GetChild(fireChildIndex).gameObject.GetComponent<Rigidbody2D>().simulated = false;
            }
            gameOverPanel.SetActive(true);
            gameOverTargetHole.transform.position = new Vector2(deadPlanetTransform.position.x, deadPlanetTransform.position.y);
            isStopAnimation = false;
            //Debug.Log(" 너 지금 몇번 호출중?");
        }
    }
}