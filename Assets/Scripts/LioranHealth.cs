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
    public Transform HealthBar; // Drag "HealthBar" here in Inspector
    private List<HeartUIScript> hearts;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;


    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();

        // Collect all heart scripts from children of HealthBar
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

        // Play lose animations
        for (int i = previousHealth - 1; i >= currentHealth; i--)
        {
            if (i < hearts.Count)
                hearts[i].PlayLoseAnimation();
        }

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            StartCoroutine(Invunerability());
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

        // Play gain animations
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
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(8,9, true);

        //invunerability duration
        for(int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes));
        }
        Physics2D.IgnoreLayerCollision(8,9, false);

    }
}
