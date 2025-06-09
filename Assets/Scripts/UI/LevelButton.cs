using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private Image lockedIcon;
    
    public void SetupButton(string levelName, bool isLocked)
    {
        if (levelNameText != null)
        {
            levelNameText.text = levelName;
        }
        
        if (lockedIcon != null)
        {
            lockedIcon.gameObject.SetActive(isLocked);
        }
        
        // Disable button if level is locked
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.interactable = !isLocked;
        }
    }
}