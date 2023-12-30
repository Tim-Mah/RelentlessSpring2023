using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    public float maxHealth = 3f;
    public float currentHealth;
    private const float damageCoef = 0.2f;
    private const float healCoef = 0.5f;

    public Rigidbody2D playerRigidbody;
    public float bounceFactor = 10;

    public HealthBar healthBar;

    public int targetSceneIndex;
    bool isWater;
    bool isFire;

    [SerializeField] private GameObject respawnBox;
    private Respawn respawn;

    
    public void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        playerRigidbody = GetComponent<Rigidbody2D>();

        respawnBox = GameObject.FindWithTag("Respawn");
        respawn = respawnBox.GetComponent<Respawn>();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Damage")) {
            TakeDamage(1);
        }
        if(collision.CompareTag("HealthDrain") || collision.CompareTag("Water")) {
            isFire = false;
            isWater = true;
        }
        if(collision.CompareTag("Fire")) {
            isWater = false;
            isFire = true;
        }
        if(collision.CompareTag("Heal")) {
            TakeHealing(1);
        }
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            respawn.respawn();
        }
        else {
            healthBar.SetHealth(currentHealth);
            playerRigidbody.AddForce(Vector2.left * bounceFactor, ForceMode2D.Impulse);
        }
    }

    public void TakeHealing(float heal) {
        currentHealth += heal;
        if (currentHealth <= 0) {
            respawn.respawn();
        }
        else {
            healthBar.SetHealth(currentHealth);
        }
    }

    void Update() {
        if(isWater) {
            currentHealth -= damageCoef * Time.deltaTime;
            if (currentHealth <= 0) {
                respawn.respawn();
            }
            else {
                healthBar.SetHealth(currentHealth);
            }
        }
        if(isFire) {
            currentHealth += healCoef * Time.deltaTime;
            if (currentHealth <= 0) {
                respawn.respawn();
            }
            else {
                if(currentHealth >= 3) {
                    isFire = false;
                }
                else {
                    healthBar.SetHealth(currentHealth);
                }
            }
        }
    }
}
