using Unity.VisualScripting;
using UnityEngine;

public class MagnetEffect : MonoBehaviour
{
    [SerializeField]private int planetId;
    [SerializeField]private int mergeCount;
    private Rigidbody2D rb;

    private bool isMerge = true;
    private void Start(){
        rb = GetComponent<Rigidbody2D>();

        planetId = PlanetManager.Instance.planetCount;
        PlanetManager.Instance.planetCount += 1;
    }
    private void FixedUpdate()
    {
        if (rb.simulated == true){
            Vector2 gravityDir = (Vector2.zero - rb.position).normalized;
            Vector2 gravity = 9.8f * gravityDir;

            Vector2 nowVelocity = rb.velocity;
            //Vector2 perpendicularVel = Vector3.Project(nowVelocity, gravityDir);
            Vector2 newVelocity = nowVelocity + gravity * Time.fixedDeltaTime;


            float fixedSpeed = 5f;
            if (newVelocity.magnitude > fixedSpeed)
            {
                newVelocity = newVelocity.normalized * fixedSpeed;
            }
            rb.velocity = newVelocity;
        }
        if (!isMerge) {
            isMerge = true; // 다음 프레임에서 다시 충돌 가능
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Planet") && isMerge){
            isMerge = false; // 충돌 처리 시작

            MagnetEffect otherPlanet = other.gameObject.GetComponent<MagnetEffect>(); 
            if(this.planetId > otherPlanet.planetId && this.mergeCount == otherPlanet.mergeCount){
                Debug.Log("같은 행성끼리 충돌했다!!!!!!");
                Destroy(this.gameObject);
                Destroy(other.gameObject);

                Vector2 middlePosition = (this.transform.position + other.transform.position) / 2;
                GameObject nextPlanet = Instantiate(PlanetManager.Instance.planetPrefabList[this.mergeCount+1], middlePosition, Quaternion.identity);
            }
        }  
    }
}
