using UnityEngine;
using TMPro;

public class ItemCounterUI : MonoBehaviour
{
    public TextMeshProUGUI itemCountText;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (itemCountText == null)
        {
            itemCountText = GetComponent<TextMeshProUGUI>();
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
        if (playerMovement == null || itemCountText == null) return;

        // Show all item counts and capacity information
        string displayText =
            $"Capacity: {playerMovement.GetTotalItemCount()}/{playerMovement.maxItemCapacity}\n" +
            $"Organik: {playerMovement.GetOrganikCount()}\n" +
            $"Anorganik: {playerMovement.GetAnorganikCount()}\n" +
            $"B3: {playerMovement.GetB3Count()}\n" +
            $"Score: {playerMovement.GetScore()}";

        itemCountText.text = displayText;
    }
}
