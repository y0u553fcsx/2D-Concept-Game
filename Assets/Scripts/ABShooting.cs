using UnityEngine;

public class ABShooting : MonoBehaviour
{
    public KeyCode Return;           // the shoot key, chosen in the Inspector
    public Transform shootingPoint;  // where bullets appear
    public GameObject bullet;        // the bullet prefab
    public SpriteRenderer sr;        // player's sprite (bullet reads its facing direction)

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(Return))
        {
            Shooting();
        }
    }

    public void Shooting()
    {
        Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
    }
}