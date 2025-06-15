using UnityEngine;

/// <summary>
/// Gère la collecte des pièces du puzzle et leur placement automatique sur la plateforme.
/// </summary>
public class PuzzlePieceManager : MonoBehaviour
{
    [SerializeField] private PuzzlePlatform platform; // Référence à la plateforme de puzzle dans la scène
    [SerializeField] private CameraTransition cameraTransition; // Transition de caméra vers la plateforme

    /// <summary>
    /// Appelée lorsqu'une pièce est collectée. Déclenche une transition caméra et place la pièce.
    /// </summary>
    public void CollectPiece(string pieceID, Sprite sprite)
    {
        // Transition caméra vers la plateforme, puis placement automatique
        cameraTransition.FocusOn(platform.transform, () =>
        {
            platform.PlacePiece(pieceID, sprite);
        });
    }
}
