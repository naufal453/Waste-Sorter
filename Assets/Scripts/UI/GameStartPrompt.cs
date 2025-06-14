using UnityEngine;
using UnityEngine.UI;

public class GameStartPrompt : MonoBehaviour
{
    [SerializeField] public GameObject promptPanel;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    public System.Action OnGameStart;

    private void Awake()
    {
        if (promptPanel != null)
            promptPanel.SetActive(false); // Pastikan panel prompt tidak aktif di awal
    }

    public void ShowPrompt()
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(true);
            Time.timeScale = 0f; // Pause game saat prompt muncul
        }
    }

    private void Start()
    {
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    private void OnYesClicked()
    {
        promptPanel.SetActive(false);
        Time.timeScale = 1f; // Unpause game saat Yes ditekan
        OnGameStart?.Invoke();
        // Lanjutkan ke gameplay, misal: aktifkan kontrol player, timer, dsb
    }

    private void OnNoClicked()
    {
        // Bisa keluar ke menu atau hanya menutup prompt
        Application.Quit();
    }
}