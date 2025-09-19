using UnityEngine.UI;
using UnityEngine;
using System;

public class MonsterController : MonoBehaviour
{
    [Header("Monster references")]
    public Action OnMonsterDeath;
    public Slider slider;
    public float maxHealth = 10f;
    private float currentHealth;
    public SpawnerMonster spawnerMonster;
    public Animator monsterAnimator;
    public bool isMoving = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (slider != null)
        {
            slider.maxValue = maxHealth;
            slider.value = currentHealth;
        }
    }

    void Update() // Make the auto move of the monster toward the player
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            float step = 2f * Time.deltaTime; // Adjust speed as necessary
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
            isMoving = true;
        }

        if (isMoving) // Update animation parameters
        {
            monsterAnimator.SetBool("IsMoving", isMoving);
        }
        else
        {
            monsterAnimator.SetBool("IsMoving", false);
        }
    }

    public void TakeDamage(float damage) // Call this function to apply damage to the monster
    {
        currentHealth -= damage;
        if (slider != null)
        {
            slider.value = currentHealth;
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die() // Function to be called when the monster dies
    {
        // Trigger the death event
        if (spawnerMonster != null)
        {
            spawnerMonster.DropRandomRecyclingObject();
        }
        // OnMonsterDeath?.Invoke(spawnerMonster.OnMonsterDeathDrop());
        // Additional logic for monster death can be added here
    }
}
