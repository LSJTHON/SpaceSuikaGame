using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetManager : Singleton<PlanetManager>
{
    [Header("PlanetSetting")]
    [SerializeField] private int planetCount = 1;
    [SerializeField] private List<GameObject> planetPrefabList = new List<GameObject>();
    [SerializeField] private Button restartButton;
    [SerializeField] private Transform waitingPlanetSpawnPint;
    [SerializeField] private Transform firePlanetSpawnPoint;
    [SerializeField] private GameObject waitingPlanet;
    [SerializeField] private GameObject firePlanet;

    [Header("ScoreSetting")]
    [SerializeField] private int totalScore = 0;
    public TextMeshProUGUI scoreText;

    [Header("PlaySetting")]
    [SerializeField] private GameObject gameOverPanel;
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
    //public Transform GetWaitingPlanetSpawnPint()
    //{
    //    return waitingPlanetSpawnPint;
    //}
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
            gameOverPanel.SetActive(false);

            StartGame();
        });
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
    public IEnumerator ReStartGame(float delay = 0.1f)
    {
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
            yield return new WaitForSeconds(delay);
            gameOverPanel.SetActive(true);
        }
    }
}