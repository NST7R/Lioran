using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LioranHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth;
    public int currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("UI")]
    public Transform HealthBar;
    private List<HeartUIScript> hearts;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();

        hearts = new List<HeartUIScript>();
        foreach (Transform heart in HealthBar)
        {
            var heartScript = heart.GetComponent<HeartUIScript>();
            if (heartScript != null)
                hearts.Add(heartScript);
            else
                Debug.LogWarning($"Missing HeartUIScript on {heart.name}");
        }
    }

    public void TakeDamage(int amount)
    {
        if (dead || amount <= 0) return;

        int previousHealth = currentHealth;
        currentHealth = Mathf.Max(currentHealth - amount, 0);

        for (int i = previousHealth - 1; i >= currentHealth; i--)
        {
            if (i < hearts.Count)
                hearts[i].PlayLoseAnimation();
        }

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            StartCoroutine(Invulnerability());
        }
        else
        {
            Die();
        }
    }

    public void AddHealth(int amount)
    {
        if (dead || amount <= 0) return;

        int previousHealth = currentHealth;
        currentHealth = Mathf.Min(currentHealth + amount, startingHealth);

        for (int i = previousHealth; i < currentHealth; i++)
        {
            if (i < hearts.Count)
                hearts[i].PlayGainAnimation();
        }
    }

    private void Die()
    {
        if (dead) return;

        anim.SetTrigger("Die");
        GetComponent<LioranMovement>().enabled = false;
        dead = true;

        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);

        LioranRespawn respawner = GetComponent<LioranRespawn>();
        if (respawner != null)
        {
            respawner.Respawn();
        }
        else
        {
            Debug.LogWarning("LioranRespawn component not found on this GameObject.");
        }
    }

    public void Respawn()
    {
        dead = false;

        // Directly set health and update UI
        currentHealth = startingHealth;
        UpdateHeartsUI();

        anim.ResetTrigger("Die");
        anim.Play("LioranIdle");

        foreach (Behaviour component in components)
            component.enabled = true;

        var movement = GetComponent<LioranMovement>();
        if (movement != null && !movement.enabled)
        {
            movement.enabled = true;
        }

        StartCoroutine(Invulnerability());
    }

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
                hearts[i].PlayGainAnimation();
            else
                hearts[i].PlayLoseAnimation();
        }
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / numberOfFlashes);
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / numberOfFlashes);
        }

        Physics2D.IgnoreLayerCollision(8, 9, false);
    }
}
