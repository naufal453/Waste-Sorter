using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChallengeAreaManager : MonoBehaviour
{
    public float challengeTime = 30f;
    public string nextSceneName = "NextScene";
    public GameObject tryAgainPopup; // Assign di Inspector
    public GameObject successPopup;  // Assign di Inspector (UI Panel dengan tombol lanjut)
    public TMP_Text timerText;       // Assign di Inspector

    private float timer;
    private bool challengeActive = true;

    void Start()
    {
        timer = challengeTime;
        if (tryAgainPopup != null) tryAgainPopup.SetActive(false);
        if (successPopup != null) successPopup.SetActive(false);
    }

    void Update()
    {
        if (!challengeActive) return;

        timer -= Time.deltaTime;
        if (timerText != null)
            timerText.text = $"Time: {Mathf.Ceil(timer)}";

        bool itemMasihAda = FindObjectsOfType<PickupItem>().Length > 0;

        // Ambil total item dari PlayerMovement
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        int totalItem = 0;
        if (player != null)
        {
            totalItem = player.GetOrganikCount() + player.GetAnorganikCount() + player.GetB3Count();
        }

        if (timer <= 0f)
        {
            // Jika waktu habis, tetap gagal jika item masih ada
            challengeActive = false;
            if (tryAgainPopup != null) tryAgainPopup.SetActive(true);
        }
        else
        {
            // Success hanya jika item habis DAN total item == 0
            if (!itemMasihAda && totalItem == 0)
            {
                challengeActive = false;
                if (successPopup != null) successPopup.SetActive(true);
            }
        }
    }

    // Fungsi untuk tombol Try Again
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Fungsi untuk tombol Lanjut pada popup berhasil
    public void NextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}