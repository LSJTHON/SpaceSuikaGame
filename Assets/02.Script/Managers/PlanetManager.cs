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
    private void Start()
    {
        StartGame();

        restartButton.onClick.AddListener(() => {
            //Debug.Log("Å¬¸¯¾²!");
            spawnPosition = GameObject.Find("NextPlanetSpawnPoint").transform;
            firePlanetPosition = GameObject.Find("FirePlanetPosition").transform;


            for (int waitingChildIndex = 0; waitingChildIndex < spawnPosition.childCount; waitingChildIndex++)
            {
                Destroy(spawnPosition.GetChild(waitingChildIndex).gameObject);
            }
            for (int fireChildIndex = 0; fireChildIndex < firePlanetPosition.childCount; fireChildIndex++)
            {
                Destroy(firePlanetPosition.GetChild(fireChildIndex).gameObject);
            }

            totalScore = 0;
            StartGame();

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

    public void StartGame()
    {
        scoreText.text = $"Score : {totalScore}";
        firePlanet = Instantiate(planetPrefabList[Random.Range(0, 4)], firePlanetPosition);
        firePlanet.GetComponent<Rigidbody2D>().simulated = false;
        waitingPlanet = Instantiate(planetPrefabList[Random.Range(0, 4)], spawnPosition.transform);
        waitingPlanet.GetComponent<Rigidbody2D>().simulated = false;
    }
}