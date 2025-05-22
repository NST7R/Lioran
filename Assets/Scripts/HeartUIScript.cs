using UnityEngine;

public class HeartUIScript : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning($"Animator not found on {gameObject.name}. HeartScript requires an Animator.");
        }
    }

    public void PlayLoseAnimation()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Gain"); // Optional: avoid conflicting triggers
            animator.SetTrigger("Lose");
        }
    }

    public void PlayGainAnimation()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Lose"); // Optional: avoid conflicting triggers
            animator.SetTrigger("Gain");
        }
    }
}
