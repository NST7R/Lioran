using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverArrows : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject leftArrow;
    public GameObject rightArrow;

    public void OnPointerEnter(PointerEventData eventData)
    {
        leftArrow.SetActive(true);
        rightArrow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
    }
}