using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Gère une transition de la caméra vers un point donné (ex: la plateforme du puzzle), sans LeanTween.
/// </summary>
public class CameraTransition : MonoBehaviour
{
    [Header("Références")]
    public Camera cam;

    [Header("Réglages")]
    public float duration = 2f;

    private Coroutine transitionRoutine;

    /// <summary>
    /// Déplace la caméra vers la cible, puis exécute une action à la fin.
    /// </summary>
    public void FocusOn(Transform target, Action onComplete)
    {
        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(MoveCameraCoroutine(target.position, onComplete));
    }

    /// <summary>
    /// Coroutine qui interpole la position de la caméra.
    /// </summary>
    private IEnumerator MoveCameraCoroutine(Vector3 targetPosition, Action onComplete)
    {
        Vector3 startPos = cam.transform.position;
        Vector3 endPos = new Vector3(targetPosition.x, targetPosition.y, startPos.z);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cam.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        cam.transform.position = endPos;

        onComplete?.Invoke();
    }
}
