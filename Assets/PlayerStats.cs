using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int health = 3;
    public static int lives = 3;
    public static int score = 0;
    private float flickerTime = 0f;
    public float flickerDuration = 0.1f;

    private SpriteRenderer sr;

    public bool isImmune = false;
    private float immunityTime = 0f;
    public float immunityDuration = 1.5f;
    public TextMeshProUGUI scoreUI;
    public Image healthBar;
    public Image[] hearts;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isImmune == true)
        {
            SpriteFlicker();
            immunityTime = immunityTime + Time.deltaTime;
            if (immunityTime >= immunityDuration)
            {
                isImmune = false;
                sr.enabled = true;
            }
        }
        scoreUI.text = " " + score;
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < lives;   // show one heart per life left
        }
    }

    void SpriteFlicker()
    {
        if (flickerTime < flickerDuration)
        {
            flickerTime = flickerTime + Time.deltaTime;
        }
        else if (flickerTime >= flickerDuration)
        {
            sr.enabled = !(sr.enabled);
            flickerTime = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isImmune == false)
        {
            health = health - damage;
            healthBar.fillAmount = this.health / 3f;
            if (health < 0)
                health = 0;
            if (lives > 0 && health == 0)
            {
                FindObjectOfType<LevelManager>().RespawnPlayer();
                health = 3;
                healthBar.fillAmount = this.health / 3f;
                lives--;
            }
            else if (lives == 0 && health == 0)
            {
                Debug.Log("Game Over");
                SceneManager.LoadScene(2);
            }
            Debug.Log("Player Health: " + health.ToString());
            Debug.Log("Player Lives: " + lives.ToString());
        }
        isImmune = true;
        immunityTime = 0f;
    }
    public bool Heal(int amount)
    {
        if (health >= 3)
            return false;

        health = Mathf.Min(health + amount, 3);
        healthBar.fillAmount = this.health / 3f;
        return true;
    }
}
