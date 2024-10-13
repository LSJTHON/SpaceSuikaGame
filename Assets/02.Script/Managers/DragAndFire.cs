using UnityEngine;

public class DragAndFire : MonoBehaviour
{
    [SerializeField] private GameObject OrbitLine;
    private LineRenderer planetOrbitLine;
    private Vector2 firstPlanetPoint = new Vector2(-10, 0);
    private Vector2 initialMousePosition;
    private Vector2 deltaPosition;
    private float shootingForce;
    private float maxMouseDistance = 1f;
    private float addShottingForce = 500f;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Init mouse position setting
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
            // Max dragRange is 1f
            if (distance > maxMouseDistance)
            {
                deltaPosition = deltaPosition.normalized * maxMouseDistance;
                distance = maxMouseDistance;
            }
            //Planet position
            PlanetManager.Instance.GetFirePlanet().transform.position = firstPlanetPoint + deltaPosition;
            shootingForce = distance;
            //Orbit range
            planetOrbitLine.SetPosition(1, -(deltaPosition * 3));
        }
        else if (Input.GetMouseButtonUp(0) && PlanetManager.Instance.GetFirePlanet() != null && initialMousePosition.x <= 5f)
        {
            Rigidbody2D firePlanetRigidBody = PlanetManager.Instance.GetFirePlanet().GetComponent<Rigidbody2D>();
            firePlanetRigidBody.simulated = true;
            firePlanetRigidBody.AddForce(-deltaPosition.normalized * shootingForce * addShottingForce);
            PlanetManager.Instance.SetFirePlanet(null);
            OrbitLine.SetActive(false);
            StartCoroutine(PlanetManager.Instance.NextPlanet(1f));
        }
    }
}
