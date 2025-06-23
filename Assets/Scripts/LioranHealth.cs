using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LioranHealth : MonoBehaviour
{
    public int startingHealth = 5;  // made public as requested
    public int currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("UI")]
    public Transform HealthBar;
    private List<HeartUIScript> hearts;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration = 1f;
    [SerializeField] private int numberOfFlashes = 5;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    private bool isTemporarilyInvulnerable = false;

    private const string HealthSaveKey = "LioranHealth";

    private void Awake()
    {
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

        LoadHealth(); // Load saved health immediately
    }

    // 🟢 Move UI update to Start, where it's guaranteed that heart scripts are ready
    private void Start()
    {
        UpdateHeartsUI();
    }

    public void TakeDamage(int amount)
    {
        if (dead || amount <= 0 || isTemporarilyInvulnerable) return;

        AudioManager.Instance?.PlaySFX(AudioManager.Instance.hurtClip);

        int previousHealth = currentHealth;
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        SaveHealth();

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

    private void Die()
    {
        if (dead) return;

        AudioManager.Instance?.PlaySFX(AudioManager.Instance.hurtClip); // You can change to a dieClip if you want

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

        LoadHealth();      // Reload saved health on respawn
        
        if (currentHealth <= 0)
        {
            currentHealth = startingHealth;  // Restore at least full or minimal health
            SaveHealth();
        }

        UpdateHeartsUI();  // Update UI immediately

        anim.ResetTrigger("Die");
        anim.Play("LioranIdle");

        foreach (Behaviour component in components)
            component.enabled = true;

        var movement = GetComponent<LioranMovement>();
        if (movement != null && !movement.enabled)
            movement.enabled = true;

        StartCoroutine(Invulnerability());
    }

    public void LoadHealth()
    {
        currentHealth = PlayerPrefs.GetInt(HealthSaveKey, startingHealth);
        Debug.Log($"[LioranHealth] Loaded health: {currentHealth}");
    }

    public void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentHealth)
                hearts[i].PlayGainAnimation();
            else
                hearts[i].PlayLoseAnimation();
        }
        Debug.Log("[LioranHealth] Hearts UI updated.");
    }

    public void AddHealth(int amount)
    {
        if (dead || amount <= 0) return;

        int previousHealth = currentHealth;
        currentHealth = Mathf.Min(currentHealth + amount, startingHealth);
        SaveHealth();

        for (int i = previousHealth; i < currentHealth; i++)
        {
            if (i < hearts.Count)
                hearts[i].PlayGainAnimation();
        }
    }

    private void SaveHealth()
    {
        PlayerPrefs.SetInt(HealthSaveKey, currentHealth);
        PlayerPrefs.Save();
        Debug.Log($"[LioranHealth] Saved health: {currentHealth}");
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

    public void EnableSilentInvulnerability()
    {
        isTemporarilyInvulnerable = true;
    }

    public void DisableSilentInvulnerability()
    {
        isTemporarilyInvulnerable = false;
    }

    public void ResetSavedHealth()
    {
        PlayerPrefs.DeleteKey(HealthSaveKey);
    }
}
