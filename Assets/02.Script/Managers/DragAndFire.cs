using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndFire : MonoBehaviour
{

    private Vector2 defaultPosition = new Vector2(-10, 0);
    private Rigidbody2D waitingPlanetRigidbody;
    private GameObject waitingPlanet;
    private Vector2 initialMousePosition;
    private void Start()
    {
        waitingPlanet = PlanetManager.Instance.waitingPlanet;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 클릭 시작 시 초기 마우스 위치 저장
        {
            initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }else if (Input.GetMouseButton(0) && waitingPlanet != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePosition);

            // 이동한 거리 계산
            Vector2 deltaPosition = mousePosition - initialMousePosition;

            // waitingPlanet의 위치를 기본 위치에서 deltaPosition만큼 이동
            waitingPlanet.transform.position = defaultPosition + deltaPosition;
        }else if(Input.GetMouseButtonUp(0))
        {
            waitingPlanet.GetComponent<Rigidbody2D>().simulated = true;


            StartCoroutine(PlanetManager.Instance.InstantiatePlanet(1f));
        }
        else
        {
            waitingPlanet = PlanetManager.Instance.waitingPlanet;
        }
    }
}
