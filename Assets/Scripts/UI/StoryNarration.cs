using UnityEngine;
using TMPro;
using System.Collections;

public class StoryNarration : MonoBehaviour
{
    [TextArea(3, 10)]
    public string narrationText = "Selamat datang di Area Tantangan!\nDi sini kamu akan belajar memilah sampah dengan benar. Selesaikan setiap tantangan untuk menjadi pahlawan lingkungan!";

    public GameObject narrationPanel;
    public TextMeshProUGUI narrationTextUI;

    [Tooltip("Waktu (detik) sebelum narasi tertutup otomatis")]
    public float autoCloseTime = 6f;

    private bool isClosed = false;

    [Header("Game Start Prompt")]
    public GameStartPrompt gameStartPrompt; // Tambahkan field ini

    private void Start()
    {
        if (narrationPanel != null && narrationTextUI != null)
        {
            narrationPanel.SetActive(true);
            narrationTextUI.text = narrationText;
            Time.timeScale = 0f; // Pause game
            StartCoroutine(AutoCloseNarration());
        }
    }

    private IEnumerator AutoCloseNarration()
    {
        yield return new WaitForSecondsRealtime(autoCloseTime);
        CloseNarration();
    }

    public void CloseNarration()
    {
        if (isClosed) return;
        isClosed = true;

        if (narrationPanel != null)
            narrationPanel.SetActive(false);
        Time.timeScale = 1f; // Resume game

        // Tampilkan popup game start jika ada
        if (gameStartPrompt != null)
        {
            gameStartPrompt.ShowPrompt(); // Gunakan ShowPrompt agar otomatis pause
        }
    }
}