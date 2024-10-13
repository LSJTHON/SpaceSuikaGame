using System.Collections;
using UnityEngine;

public class CustomPlanetMovement : MonoBehaviour
{
    [SerializeField] private int planetId;
    [SerializeField] private int mergeCount;
    private Rigidbody2D rb;
    private float radius;
    private float deadRadius;
    private float maxSpeed = 12f;
    private float addMagnet = 5f;
    private int maxMergeCount = 9;
    private bool canDie = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        planetId = PlanetManager.Instance.GetPlanetCount();
        PlanetManager.Instance.SetPlanetCount();
        maxMergeCount = PlanetManager.Instance.GetPlanetPrefabLists().Count;
        deadRadius = PlanetManager.Instance.GetDeadLine().localScale.x / 2;
        radius = transform.localScale.x / 2;
    }
    private void FixedUpdate()
    {
        if (rb.simulated)
        {
            Vector2 gravityDir = (Vector2.zero - rb.position).normalized;
            Vector2 gravity = 9.8f * gravityDir;
            Vector2 newGravity = rb.velocity + gravity * Time.fixedDeltaTime;

            Vector2 planetToZeroVector = (Vector2.zero - newGravity).normalized;
            newGravity += planetToZeroVector * addMagnet * Time.fixedDeltaTime;

            if (newGravity.magnitude > maxSpeed)
            {
                newGravity = newGravity.normalized * maxSpeed;
            }
            //Debug.Log(newVelocity + " 어어 끌어당긴다");
            rb.velocity = newGravity;
        }
        float isDeadRadius = deadRadius - radius;
        float planetDistance = transform.position.magnitude;
        if (canDie && isDeadRadius < planetDistance)
        {
            PlanetManager.Instance.isDead = true;
            PlanetManager.Instance.GameOver(this.transform);
            //GetChild(1) : ExplosionEffect object
            transform.GetChild(1).gameObject.SetActive(true);
            canDie = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            CustomPlanetMovement otherPlanet = other.gameObject.GetComponent<CustomPlanetMovement>();
            //Debug.Log(otherPlanet.planetId +" 니 어케 접근함?");
            if (this.planetId > otherPlanet.planetId
                && this.mergeCount == otherPlanet.mergeCount
                && this.mergeCount < maxMergeCount)
            {
                Debug.Log("Merge!!!!!!");
                Destroy(this.gameObject);
                Destroy(other.gameObject);
                Vector2 middlePosition = (this.transform.position + other.transform.position) / 2;
                GameObject mergePlanet =
                    Instantiate(PlanetManager.Instance.GetPlanetPrefabList(this.mergeCount + 1), middlePosition, Quaternion.identity);
                mergePlanet.GetComponent<ParticleSystem>().Play();
                mergePlanet.transform.SetParent(PlanetManager.Instance.GetFirePlanetSpawnPoint());
                PlanetManager.Instance.SetScore((mergeCount + 1) * 30);
                PlanetManager.Instance.scoreText.text = $"SCORE : {PlanetManager.Instance.GetScore()}";
            }
            else if (this.mergeCount >= maxMergeCount)
            {
                //Debug.Log("은하계 멸망");
                Destroy(this.gameObject);
                Destroy(other.gameObject);
                PlanetManager.Instance.SetScore((mergeCount + 1) * 30);
                PlanetManager.Instance.scoreText.text = $"SCORE : {PlanetManager.Instance.GetScore()}";
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