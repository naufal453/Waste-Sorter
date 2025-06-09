using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LevelChooser : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string challengeAreaSceneName = "ChallengeArea";
    
    [System.Serializable]
    public class LevelData
    {
        public string levelName;
        public string sceneName;
    }
    
    [SerializeField] private List<LevelData> levels = new List<LevelData>();
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform levelButtonContainer;
    
    private void Start()
    {
        GenerateLevelButtons();
    }
    
    private void GenerateLevelButtons()
    {
        if (levelButtonPrefab == null || levelButtonContainer == null) return;
        
        // Clear existing buttons
        foreach (Transform child in levelButtonContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Create buttons for each level
        foreach (LevelData level in levels)
        {
            GameObject buttonObj = Instantiate(levelButtonPrefab, levelButtonContainer);
            
            // Ensure proper position and scale
            RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Reset position 
                rectTransform.anchoredPosition = new Vector2(0, -100 * levels.IndexOf(level)); // Stack vertically
                
                // Ensure reasonable size
                rectTransform.sizeDelta = new Vector2(200, 50); // Set width/height
                
                // Ensure normal scale
                rectTransform.localScale = Vector3.one;
            }
            
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                // Enable the Button component
                button.enabled = true;
                
                // Make sure the button is interactable
                button.interactable = true;
                
                // Add click handler
                button.onClick.AddListener(() => LoadLevel(level.sceneName));
            }
            
            // Fix the image transparency
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                // Make sure image is enabled and visible
                buttonImage.enabled = true;
                Color color = buttonImage.color;
                color.a = 1f; // Full opacity
                buttonImage.color = color;
            }
            
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>(true); // true to find inactive components
            
            // Set button text if it has a Text component as a child
            if (buttonText != null)
            {
                // Enable the TextMeshPro component
                buttonText.gameObject.SetActive(true);
                buttonText.enabled = true;
                
                // Set the text
                buttonText.text = level.levelName;
            }
        }
    }
    
    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}