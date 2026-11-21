using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed;
    public ABShooting player;
    public int damage;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float timer = 0;
    private float lifeTime = 2;

    void Start()
    {
        player = FindObjectOfType<ABShooting>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // face the same way as the player
        // (works whether your player turns using Flip X or a negative Scale X)
        sr.flipX = player.sr.flipX || player.transform.localScale.x < 0;
    }

    void Update()
    {
        if (sr.flipX)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);   // fly left
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);    // fly right
        }

        // destroy the bullet after lifeTime seconds so it doesn't fly forever
        if ((timer += Time.deltaTime) >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().EnemyTakeDamage(damage);
            Destroy(gameObject);
        }

        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}