using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetManager : Singleton<PlanetManager>
{
    [SerializeField] private Button restartButton;
    public int planetCount = 1;
    public int totalScore = 0;
    public TextMeshProUGUI scoreText;
    public List<GameObject> planetPrefabList = new List<GameObject>();
    [SerializeField] private Transform spawnPosition;
    public Transform firePlanetPosition;
    public GameObject waitingPlanet;
    public GameObject firePlanet;

    [SerializeField] private GameObject gameOverPanel;
    public bool isDead = false;
    private void Start()
    {
        StartGame();

        restartButton.onClick.AddListener(() => {
            //StartCoroutine(ReStartGame());

            for (int waitingChildIndex = 0; waitingChildIndex < spawnPosition.childCount; waitingChildIndex++)
            {
                Destroy(spawnPosition.GetChild(waitingChildIndex).gameObject);
            }
            for (int fireChildIndex = 0; fireChildIndex < firePlanetPosition.childCount; fireChildIndex++)
            {
                Destroy(firePlanetPosition.GetChild(fireChildIndex).gameObject);
            }
            gameOverPanel.SetActive(false);

            StartGame();

            totalScore = 0;
        });
    }
    public IEnumerator NextPlanet(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (firePlanet == null)
        {
            waitingPlanet.transform.SetParent(firePlanetPosition);

            // waitingPlanet -> firePlanet
            firePlanet = waitingPlanet;
            // generate new waitingPlanet
            int randomPlanetIndex = Random.Range(0, 4);
            waitingPlanet = Instantiate(planetPrefabList[randomPlanetIndex], spawnPosition.transform);
            firePlanet.transform.position = firePlanetPosition.position;
            waitingPlanet.GetComponent<Rigidbody2D>().simulated = false;
            waitingPlanet.GetComponent<MagnetEffect>().enabled = false;
        }
    }

    private void StartGame()
    {
        scoreText.text = $"Score : {totalScore}";
        firePlanet = Instantiate(planetPrefabList[Random.Range(0, 4)], firePlanetPosition);
        firePlanet.GetComponent<Rigidbody2D>().simulated = false;
        waitingPlanet = Instantiate(planetPrefabList[Random.Range(0, 4)], spawnPosition.transform);
        waitingPlanet.GetComponent<Rigidbody2D>().simulated = false;
    }

    public IEnumerator ReStartGame(float delay = 0.1f)
    {

        if (isDead)
        {
            isDead = false;
            for (int waitingChildIndex = 0; waitingChildIndex < spawnPosition.childCount; waitingChildIndex++)
            {
                //firePlanetPosition.GetChild(waitingChildIndex).gameObject.GetComponent<MagnetEffect>().enabled = false;
                firePlanetPosition.GetChild(waitingChildIndex).gameObject.GetComponent<Rigidbody2D>().simulated = false;
            }
            for (int fireChildIndex = 0; fireChildIndex < firePlanetPosition.childCount; fireChildIndex++)
            {
                firePlanetPosition.GetChild(fireChildIndex).gameObject.GetComponent<Rigidbody2D>().simulated = false;
                //firePlanetPosition.GetChild(fireChildIndex).gameObject.GetComponent<MagnetEffect>().enabled = false;
            }
            yield return new WaitForSeconds(delay);
            gameOverPanel.SetActive(true);
        }
    }
}