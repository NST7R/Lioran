using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSounds : MonoBehaviour, IPointerEnterHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            // Add click listener to play click sound
            button.onClick.AddListener(() =>
            {
                AudioManager.Instance?.PlayUIClick();
            });
        }
        else
        {
            Debug.LogWarning("UIButtonSounds requires a Button component.");
        }
    }

    // This is called when the mouse pointer enters the button area (hover)
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance?.PlayUIHover();
    }
}
