using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private int planetId;

    private Rigidbody2D rb;

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
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
    }
}
