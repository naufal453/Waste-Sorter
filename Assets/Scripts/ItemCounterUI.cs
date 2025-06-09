using UnityEngine;
using TMPro;


public class ItemCounterUI : MonoBehaviour
{
    public TextMeshProUGUI itemCountText;
    private PlayerMovement playerMovement;

    // Add sprite references for the icons
    [Header("UI Icons")]
    public Sprite anorganikIcon;
    public Sprite organikIcon;
    public Sprite b3Icon;

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

        // Create the styled text with sprite tags for TMPro
        string displayText = string.Empty;

        // Anorganik with gear icon (gray/silver color)
        displayText += "<color=#a8aebb><sprite=\"WasteIcons\" index=0></color> ";
        displayText += $"<color=#a8aebb>Anorganik:{playerMovement.GetAnorganikCount()}</color>\n";

        // Organik with tree icon (green color)
        displayText += "<color=#3cb043><sprite=\"WasteIcons\" index=1></color> ";
        displayText += $"<color=#3cb043>Organik:{playerMovement.GetOrganikCount()}</color>\n";

        // B3 with biohazard icon (red color)
        displayText += "<color=#ff0000><sprite=\"WasteIcons\" index=2></color> ";
        displayText += $"<color=#ff0000>B3:{playerMovement.GetB3Count()}</color>";

        itemCountText.text = displayText;
    }
}
