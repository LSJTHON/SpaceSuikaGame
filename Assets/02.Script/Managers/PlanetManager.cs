using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetManager : Singleton<PlanetManager>
{
    public List<GameObject> planetPrefabList = new List<GameObject>();
    [SerializeField] public int planetCount = 1;
    Vector2 firstPosition = new Vector2(-10,0);

    private void Start() {
        Debug.Log("Game Start!!!");

        int randomPlanetIndex = Random.Range(0, 4);

        Debug.Log($"{randomPlanetIndex}번의 행성은 {planetPrefabList[randomPlanetIndex].gameObject.name}!!!!");

        GameObject firstPlanet = Instantiate(planetPrefabList[randomPlanetIndex], firstPosition , Quaternion.identity);
    }
}