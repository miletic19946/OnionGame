using UnityEngine;
using UnityEngine.EventSystems;

// Button component used in the gun reload mini-game
public class UI_ReloadButton : MonoBehaviour, IPointerDownHandler
{

    // Triggered when player clicks this button during reload sequence
    // name="eventData">Data associated with the pointer event
    public void OnPointerDown(PointerEventData eventData)
    {
        // Notify the UI system that a reload step has been completed
        UI.instance.AttemptToReload();
        // Hide this button after it has been clicked
        gameObject.SetActive(false);
    }
}
