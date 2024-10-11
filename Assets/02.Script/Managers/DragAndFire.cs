using UnityEngine;

public class DragAndFire : MonoBehaviour
{
    [SerializeField] private GameObject OrbitLine;
    private LineRenderer planetSlingEffect;
    private Vector2 firstMousePoint = new Vector2(-10, 0);
    private Vector2 initialMousePosition;
    private Vector2 deltaPosition;
    private float shootingForce;
    private float maxDistance = 1f;
    private float addShottingForce = 500f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 클릭 시작 시 초기 마우스 위치 저장
        {
            initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OrbitLine.SetActive(true);
            planetSlingEffect = OrbitLine.GetComponent<LineRenderer>();
            planetSlingEffect.SetPosition(0, Vector2.zero);
            planetSlingEffect.SetPosition(1, Vector2.zero);
        }
        else if (Input.GetMouseButton(0) && PlanetManager.Instance.GetFirePlanet() != null && initialMousePosition.x <= 5f)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            deltaPosition = mousePosition - initialMousePosition;
            float distance = Vector2.Distance(initialMousePosition, mousePosition);

            // 거리가 최대 사거리를 넘으면 최대 거리로 제한
            if (distance > maxDistance)
            {
                deltaPosition = deltaPosition.normalized * maxDistance;
                distance = maxDistance;
            }

            // 행성의 위치 업데이트 (최대 사거리 내에서만 움직이도록 제한)
            PlanetManager.Instance.GetFirePlanet().transform.position = firstMousePoint + deltaPosition;
            shootingForce = distance;

            // 슬링 이펙트 업데이트
            planetSlingEffect.SetPosition(1, -(deltaPosition * 3));
        }
        else if (Input.GetMouseButtonUp(0) && PlanetManager.Instance.GetFirePlanet() != null && initialMousePosition.x <= 5f)
        {
            Rigidbody2D firePlanetRigidbody = PlanetManager.Instance.GetFirePlanet().GetComponent<Rigidbody2D>();
            firePlanetRigidbody.gameObject.GetComponent<PlanetEffect>().enabled = true;
            firePlanetRigidbody.simulated = true;
            firePlanetRigidbody.AddForce(-deltaPosition.normalized * shootingForce * addShottingForce);
            PlanetManager.Instance.SetFirePlanet(null);
            OrbitLine.SetActive(false);
            StartCoroutine(PlanetManager.Instance.NextPlanet(1f));
        }
    }
}
