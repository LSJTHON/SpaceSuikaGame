using System.Collections;
using UnityEngine;

public class PlanetEffect : MonoBehaviour
{
    [SerializeField] private int planetId;
    [SerializeField] private int mergeCount;
    private Rigidbody2D rb;
    private float radius;
    private float deadRadius = 4f;
    private bool canDie = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        planetId = PlanetManager.Instance.GetPlanetCount();
        PlanetManager.Instance.SetPlanetCount();
        radius = transform.localScale.x / 2;
    }
    private void FixedUpdate()
    {
        if (rb.simulated)
        {
            Vector2 gravityDir = (Vector2.zero - rb.position).normalized;
            Vector2 gravity = 9.8f * gravityDir;
            Vector2 newVelocity = rb.velocity + gravity * Time.fixedDeltaTime;

            // 추가된 코드: Vector2.zero 방향으로 더 강하게 주기
            float addMagnet = 3f; // 추가 힘의 크기

            Vector2 zeroPosition = (Vector2.zero - newVelocity).normalized;

            newVelocity += zeroPosition * addMagnet * Time.fixedDeltaTime;

            float fixedSpeed = 7f;

            if (newVelocity.magnitude > fixedSpeed)
            {
                newVelocity = newVelocity.normalized * fixedSpeed;
            }
            rb.velocity = newVelocity;
        }
        float isDeadRadius = deadRadius - radius;
        float planetDistance = transform.position.magnitude;
        if (canDie && isDeadRadius < planetDistance)
        {
            PlanetManager.Instance.isDead = true;
            StartCoroutine(PlanetManager.Instance.GameOver(2f, this.transform));
            canDie = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            PlanetEffect otherPlanet = other.gameObject.GetComponent<PlanetEffect>();
            if (this.planetId > otherPlanet.planetId && this.mergeCount == otherPlanet.mergeCount)
            {
                Debug.Log("Merge!!!!!!");
                Destroy(this.gameObject);
                Destroy(other.gameObject);
                Vector2 middlePosition = (this.transform.position + other.transform.position) / 2;
                GameObject mergePlanet = Instantiate(PlanetManager.Instance.GetPlanetPrefabList(this.mergeCount + 1), middlePosition, Quaternion.identity);
                mergePlanet.transform.SetParent(PlanetManager.Instance.GetFirePlanetSpawnPoint());
                PlanetManager.Instance.SetScore((mergeCount + 1) * 10);
                PlanetManager.Instance.scoreText.text = $"Score : {PlanetManager.Instance.GetScore()}";
            }
        }
        if (!canDie)
        {
            StartCoroutine(OnDeath());
        }
    }
    private IEnumerator OnDeath()
    {
        yield return new WaitForSeconds(2f);
        canDie = true;
        //Debug.Log(" 이제 죽는데이");
    }
}
