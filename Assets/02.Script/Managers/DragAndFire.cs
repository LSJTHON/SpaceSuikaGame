using UnityEngine;

public class DragAndFire : MonoBehaviour
{
    private Vector2 firstMousePoint = new Vector2(-10, 0);
    private float shootingForce;
    private Vector2 initialMousePosition;
    private Vector2 deltaPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 클릭 시작 시 초기 마우스 위치 저장
        {
            initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }else if (Input.GetMouseButton(0) && PlanetManager.Instance.firePlanet != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 이동한 거리 계산
            deltaPosition = mousePosition - initialMousePosition;
            //Debug.Log(Vector2.Distance(initialMousePosition,mousePosition));
            if(Vector2.Distance(initialMousePosition,mousePosition) <= 1f){
                PlanetManager.Instance.firePlanet.transform.position = firstMousePoint + deltaPosition;
                shootingForce = Vector2.Distance(initialMousePosition,mousePosition);
            }else{
                Debug.Log("어어 길다길어");
            }
        }else if(Input.GetMouseButtonUp(0) && PlanetManager.Instance.firePlanet != null)
        {
            Rigidbody2D waitingPlanetRigidbody = PlanetManager.Instance.firePlanet.GetComponent<Rigidbody2D>();
            waitingPlanetRigidbody.simulated = true;
            Debug.Log(shootingForce+" 발싸!!!!!!");
            waitingPlanetRigidbody.AddForce(-deltaPosition.normalized * shootingForce* 500f);

            PlanetManager.Instance.firePlanet = null;

            StartCoroutine(PlanetManager.Instance.NextPlanet(1f));
        }
        else
        {
            return;
        }
    }
}
