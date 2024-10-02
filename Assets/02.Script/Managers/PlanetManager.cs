using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class PlanetManager : Singleton<PlanetManager>
{
    public List<GameObject> planetPrefabList = new List<GameObject>();
    [SerializeField] public int planetCount = 1;
    Vector2 firstPosition = new Vector2(-10,0);
    [SerializeField] public GameObject waitingPlanet;
    private void Start() {
        Debug.Log("Game Start!!!");

        StartCoroutine(InstantiatePlanet(1f));
    }

    public IEnumerator InstantiatePlanet(float delay){

        yield return new WaitForSeconds(delay);

        int randomPlanetIndex = Random.Range(0, 4);

        Debug.Log($"{randomPlanetIndex}번의 행성은 {planetPrefabList[randomPlanetIndex].gameObject.name}!!!!");

        waitingPlanet = Instantiate(planetPrefabList[randomPlanetIndex], firstPosition , Quaternion.identity);
        waitingPlanet.GetComponent<Rigidbody2D>().simulated = false;
    }
}