using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public ItemCounterUImenu itemCounterUI; // Counter inside menu
    public ItemCounterUI itemCounterUI2;    // Counter outside menu

    void Start()
    {
        // Find both counters
        itemCounterUI = FindObjectOfType<ItemCounterUImenu>();
        itemCounterUI2 = FindObjectOfType<ItemCounterUI>();

        if (itemCounterUI == null)
        {
            Debug.LogError("Menu ItemCounterUI not found in the scene.");
        }

        if (itemCounterUI2 == null)
        {
            Debug.LogError("Outside ItemCounterUI not found in the scene.");
        }

        menuCanvas.SetActive(false);

        // Make sure we're not trying to set active state on a null reference
        if (itemCounterUI2 != null)
        {
            itemCounterUI2.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool newState = !menuCanvas.activeSelf;
            menuCanvas.SetActive(newState);

            // Toggle visibility of outside counter
            if (itemCounterUI2 != null)
            {
                itemCounterUI2.gameObject.SetActive(!newState);
            }

            // Update counts when menu is opened
            if (newState && itemCounterUI != null)
            {
                itemCounterUI.UpdateAllCounts();
            }
        }
    }
}