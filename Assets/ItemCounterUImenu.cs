using UnityEngine;
using TMPro;

public class ItemCounterUImenu : MonoBehaviour
{
    public TextMeshProUGUI itemCountTextMenu;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (itemCountTextMenu == null)
        {
            itemCountTextMenu = GetComponent<TextMeshProUGUI>();
        }

        UpdateAllCounts();
    }

    private void OnEnable()
    {
        PlayerMovement.OnItemPickedUp.AddListener(UpdateAllCounts);
    }

    private void OnDisable()
    {
        PlayerMovement.OnItemPickedUp.RemoveListener(UpdateAllCounts);
    }

    public void UpdateAllCounts(int dummy = 0)
    {
        if (playerMovement == null || itemCountTextMenu == null) return;

        // Show all item counts
        string displayText =
                            $"Total:{playerMovement.GetOrganikCount() + playerMovement.GetAnorganikCount() + playerMovement.GetB3Count()}\n" + $"Anorganik: {playerMovement.GetAnorganikCount()}\n" +
                           $"Organik: {playerMovement.GetOrganikCount()}\n" +
                           $"B3: {playerMovement.GetB3Count()}";

        itemCountTextMenu.text = displayText;
    }
}
