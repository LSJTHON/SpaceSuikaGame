using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : Singleton<PlanetManager>
{
    [SerializeField] public int planetCount = 1;
    public List<GameObject> planetPrefabList = new List<GameObject>();
    private Vector2 firstPosition = new Vector2(-10, 0);
    public GameObject waitingPlanet;
    public GameObject firePlanet;
    public Transform spawnPosition;
    private void Start()
    {
        Debug.Log("Game Start!!!");
        firePlanet = Instantiate(planetPrefabList[Random.Range(0, 4)], firstPosition, Quaternion.identity);
        firePlanet.GetComponent<Rigidbody2D>().simulated = false;
        waitingPlanet = Instantiate(planetPrefabList[Random.Range(0, 4)], spawnPosition.transform.position, Quaternion.identity);
        waitingPlanet.GetComponent<Rigidbody2D>().simulated = false;
    }
    public IEnumerator NextPlanet(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (firePlanet == null)
        {
            // waitingPlanet -> firePlanet
            firePlanet = waitingPlanet;
            // generate new waitingPlanet
            int randomPlanetIndex = Random.Range(0, 4);
            waitingPlanet = Instantiate(planetPrefabList[randomPlanetIndex], spawnPosition.transform.position, Quaternion.identity);
            firePlanet.transform.position = firstPosition;
            waitingPlanet.GetComponent<Rigidbody2D>().simulated = false;
            waitingPlanet.GetComponent<MagnetEffect>().enabled = false;
        }
    }
}