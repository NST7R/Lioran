using UnityEngine;

/// <summary>
/// HiddenLeaf
/// Feuilles cachées que le joueur peut découvrir en s'approchant.
/// </summary>
public class HiddenLeaf : MonoBehaviour
{
    [SerializeField] private SpriteRenderer leafRenderer;
    [SerializeField] private float revealDistance = 2f;
    [SerializeField] private Transform player;

    private void Update()
    {
        if (player == null) return;
        float dist = Vector2.Distance(player.position, transform.position);
        leafRenderer.enabled = dist < revealDistance;
    }
}
