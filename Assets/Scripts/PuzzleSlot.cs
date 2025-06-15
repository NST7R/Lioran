using UnityEngine;

/// <summary>
/// Représente un emplacement de pièce sur la plateforme.
/// Gère son état, l’effet visuel et l’apparition de la pièce.
/// </summary>
public class PuzzleSlot : MonoBehaviour
{
    [SerializeField] private string expectedPieceID; // ID de la pièce attendue
    [SerializeField] private SpriteRenderer motifRenderer; // Sprite affiché à la pose de la pièce
    [SerializeField] private GameObject glowEffect; // Effet lumineux pour l’emplacement

    public string ExpectedPieceID => expectedPieceID;
    public bool IsFilled { get; private set; } = false;

    private void Awake()
    {
        motifRenderer.enabled = false; // Masque l'emplacement au début
        if (glowEffect != null) glowEffect.SetActive(false);
    }

    /// <summary>
    /// Active l’emplacement, affiche la pièce et joue une animation.
    /// </summary>
    public void Place(Sprite sprite)
    {
        motifRenderer.sprite = sprite;
        motifRenderer.enabled = true;

        if (glowEffect != null) glowEffect.SetActive(true);

        // Lance l'effet visuel de mise en valeur
        StartCoroutine(ScaleEffect(motifRenderer.transform));

        IsFilled = true;
    }

    /// <summary>
    /// Effet d'animation simple (zoom in/out rapide)
    /// </summary>
    private System.Collections.IEnumerator ScaleEffect(Transform target)
    {
        Vector3 originalScale = target.localScale;
        Vector3 enlargedScale = originalScale * 1.2f;
        float duration = 0.15f;
        float t = 0f;

        // Zoom in
        while (t < duration)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(originalScale, enlargedScale, t / duration);
            yield return null;
        }

        // Zoom out
        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            target.localScale = Vector3.Lerp(enlargedScale, originalScale, t / duration);
            yield return null;
        }

        target.localScale = originalScale;
    }
}
