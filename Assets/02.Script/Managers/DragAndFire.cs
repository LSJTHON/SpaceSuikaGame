using UnityEngine;

public class DragAndFire : MonoBehaviour
{
    [SerializeField] private GameObject OrbitLine;
    private LineRenderer planetOrbitLine;
    private Vector2 firstMousePoint = new Vector2(-10, 0);
    private Vector2 initialMousePosition;
    private Vector2 deltaPosition;
    private float shootingForce;
    private float maxMouseDistance = 1f;
    private float addShottingForce = 500f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 클릭 시작 시 초기 마우스 위치 저장
        {
            initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OrbitLine.SetActive(true);
            planetOrbitLine = OrbitLine.GetComponent<LineRenderer>();
            planetOrbitLine.SetPosition(0, Vector2.zero);
            planetOrbitLine.SetPosition(1, Vector2.zero);
        }
        else if (Input.GetMouseButton(0) && PlanetManager.Instance.GetFirePlanet() != null && initialMousePosition.x <= 5f)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            deltaPosition = mousePosition - initialMousePosition;
            float distance = Vector2.Distance(initialMousePosition, mousePosition);
            // 거리가 최대 사거리를 넘으면 최대 거리로 제한
            if (distance > maxMouseDistance)
            {
                deltaPosition = deltaPosition.normalized * maxMouseDistance;
                distance = maxMouseDistance;
            }

            Debug.Log($"{firstMousePoint} 와 {deltaPosition}을 더함");
            // 행성의 위치
            PlanetManager.Instance.GetFirePlanet().transform.position = firstMousePoint + deltaPosition;
            shootingForce = distance;
            // 궤도 설정
            planetOrbitLine.SetPosition(1, -(deltaPosition * 3));
        }
        else if (Input.GetMouseButtonUp(0) && PlanetManager.Instance.GetFirePlanet() != null && initialMousePosition.x <= 5f)
        {
            Rigidbody2D firePlanetRigidBody = PlanetManager.Instance.GetFirePlanet().GetComponent<Rigidbody2D>();
            firePlanetRigidBody.gameObject.GetComponent<CustomPlanetMovement>().enabled = true;
            firePlanetRigidBody.simulated = true;
            firePlanetRigidBody.AddForce(-deltaPosition.normalized * shootingForce * addShottingForce);
            PlanetManager.Instance.SetFirePlanet(null);
            OrbitLine.SetActive(false);
            StartCoroutine(PlanetManager.Instance.NextPlanet(1f));
        }
    }
}
