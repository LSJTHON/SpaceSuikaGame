using System.Collections;
using UnityEngine;

public class PlanetEffect : MonoBehaviour
{
    [SerializeField] private int planetId;
    [SerializeField] private int mergeCount;
    private Rigidbody2D rb;
    private float radius;
    private float deadRadius = 5f;
    private int maxMergeCount = 9;
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
            Debug.Log(gravityDir);
            Vector2 gravity = 9.8f * gravityDir;
            Vector2 newVelocity = rb.velocity + gravity * Time.fixedDeltaTime;

            // 추가된 코드: Vector2.zero 방향으로 더 강하게 주기
            float addMagnet = 4f; // 추가 힘의 크기
            Vector2 zeroPosition = (Vector2.zero - newVelocity).normalized;
            newVelocity += zeroPosition * addMagnet * Time.fixedDeltaTime;
            float fixedSpeed = 9f;
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
            PlanetManager.Instance.GameOver(2f, this.transform);
            //GetChild(1) : ExplosionEffect object
            transform.GetChild(1).gameObject.SetActive(true);
            canDie = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            PlanetEffect otherPlanet = other.gameObject.GetComponent<PlanetEffect>();
            if (this.planetId > otherPlanet.planetId 
                && this.mergeCount == otherPlanet.mergeCount 
                && this.mergeCount < maxMergeCount
                )
            {
                Debug.Log("Merge!!!!!!");
                Destroy(this.gameObject);
                Destroy(other.gameObject);
                Vector2 middlePosition = 
                    (this.transform.position + other.transform.position) / 2;
                GameObject mergePlanet = 
                    Instantiate(PlanetManager.Instance.GetPlanetPrefabList(this.mergeCount + 1), 
                    middlePosition, 
                    Quaternion.identity);
                mergePlanet.transform.SetParent(PlanetManager.Instance.GetFirePlanetSpawnPoint());
                PlanetManager.Instance.SetScore((mergeCount + 1) * 30);
                PlanetManager.Instance.scoreText.text = 
                    $"Score : {PlanetManager.Instance.GetScore()}";
            }else if(this.planetId > otherPlanet.planetId 
                && this.mergeCount == otherPlanet.mergeCount
                && this.mergeCount >= maxMergeCount
                )
            {
                Debug.Log("하늘 아래 태양이 둘 일 수 없다 이말이야");
                Destroy(this.gameObject);
                Destroy(other.gameObject);
                PlanetManager.Instance.SetScore((mergeCount + 1) * 30);
                PlanetManager.Instance.scoreText.text = 
                    $"Score : {PlanetManager.Instance.GetScore()}";
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
