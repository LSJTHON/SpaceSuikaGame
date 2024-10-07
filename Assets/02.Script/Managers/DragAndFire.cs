using Unity.VisualScripting;
using UnityEngine;

public class DragAndFire : MonoBehaviour
{
    private Vector2 firstMousePoint = new Vector2(-10, 0);
    private float shootingForce;
    private Vector2 initialMousePosition;
    private Vector2 deltaPosition;
    [SerializeField] private GameObject Line;
    [SerializeField] private LineRenderer planetSlingEffect;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 클릭 시작 시 초기 마우스 위치 저장
        {
            initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Line.SetActive(true);
            planetSlingEffect = Line.GetComponent<LineRenderer>();

            planetSlingEffect.SetPosition(0, Vector2.zero);
            planetSlingEffect.SetPosition(1, Vector2.zero);
        }
        else if (Input.GetMouseButton(0) && PlanetManager.Instance.GetFirePlanet() != null && initialMousePosition.x <= 5f)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 이동한 거리 계산
            deltaPosition = mousePosition - initialMousePosition;
            //Debug.Log(Vector2.Distance(initialMousePosition,mousePosition));
            if(Vector2.Distance(initialMousePosition,mousePosition) <= 1f){
                PlanetManager.Instance.GetFirePlanet().transform.position = firstMousePoint + deltaPosition;
                shootingForce = Vector2.Distance(initialMousePosition,mousePosition);
                planetSlingEffect.SetPosition(1, -(deltaPosition*2));
            }else{
                //Debug.Log("어어 길다길어");
            }
        }else if(Input.GetMouseButtonUp(0) && PlanetManager.Instance.GetFirePlanet() != null && initialMousePosition.x <= 5f)
        {
            Rigidbody2D waitingPlanetRigidbody = PlanetManager.Instance.GetFirePlanet().GetComponent<Rigidbody2D>();
            waitingPlanetRigidbody.gameObject.GetComponent<PlanetEffect>().enabled = true;
            waitingPlanetRigidbody.simulated = true;
            waitingPlanetRigidbody.AddForce(-deltaPosition.normalized * shootingForce* 500f);
            PlanetManager.Instance.SetFirePlanet(null);
            Line.SetActive(false);
            StartCoroutine(PlanetManager.Instance.NextPlanet(1f));
        }
        else
        {
            return;
        }
    }
}
