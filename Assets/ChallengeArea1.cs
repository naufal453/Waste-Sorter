using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Tambahkan di atas

public class ChallengeArea1 : MonoBehaviour
{
    public GameObject organikPrefab;
    public GameObject anorganikPrefab;
    public GameObject b3Prefab;
    public int itemCount = 5;
    public Transform[] spawnPoints;
    public float countdownTime = 30f;

    public GameObject confirmPanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public TMP_Text countdownText; // Diubah menjadi TMP_Text

    private bool challengeActive = false;
    private float timer;
    private int itemsLeft;
    private List<GameObject> spawnedItems = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !challengeActive)
        {
            confirmPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void StartChallenge()
    {
        confirmPanel.SetActive(false);
        Time.timeScale = 1f;
        challengeActive = true;
        timer = countdownTime;
        itemsLeft = itemCount;

        if (countdownText != null)
            countdownText.gameObject.SetActive(true); // Tampilkan countdown

        // Spawn items
        for (int i = 0; i < itemCount; i++)
        {
            if (i >= spawnPoints.Length) break;
            GameObject prefabToSpawn = null;
            int type = Random.Range(0, 3); // 0: organik, 1: anorganik, 2: b3

            switch (type)
            {
                case 0: prefabToSpawn = organikPrefab; break;
                case 1: prefabToSpawn = anorganikPrefab; break;
                case 2: prefabToSpawn = b3Prefab; break;
            }

            GameObject item = Instantiate(prefabToSpawn, spawnPoints[i].position, Quaternion.identity);
            spawnedItems.Add(item);
            PickupItem pickup = item.GetComponent<PickupItem>();
            if (pickup != null)
            {
                pickup.OnPickedUp += OnItemCollected;
            }
        }

        StartCoroutine(CountdownRoutine());
    }

    public void CancelChallenge()
    {
        confirmPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private IEnumerator CountdownRoutine()
    {
        while (timer > 0 && itemsLeft > 0)
        {
            timer -= Time.deltaTime;
            if (countdownText != null)
                countdownText.text = "Time: " + Mathf.CeilToInt(timer).ToString();
            yield return null;
        }

        if (countdownText != null)
            countdownText.gameObject.SetActive(false); // Sembunyikan countdown saat selesai

        if (itemsLeft <= 0)
        {
            WinChallenge();
        }
        else
        {
            LoseChallenge();
        }
    }

    private void OnItemCollected()
    {
        itemsLeft--;
        if (itemsLeft <= 0 && challengeActive)
        {
            WinChallenge();
        }
    }

    private void WinChallenge()
    {
        challengeActive = false;
        winPanel.SetActive(true);
        if (countdownText != null)
            countdownText.gameObject.SetActive(false); // Sembunyikan countdown
        CleanupItems();
    }

    private void LoseChallenge()
    {
        challengeActive = false;
        losePanel.SetActive(true);
        if (countdownText != null)
            countdownText.gameObject.SetActive(false); // Sembunyikan countdown
        CleanupItems();
    }

    private void CleanupItems()
    {
        foreach (var item in spawnedItems)
        {
            if (item != null) Destroy(item);
        }
        spawnedItems.Clear();
    }

    public void CloseWinPanel()
    {
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void CloseLosePanel()
    {
        if (losePanel != null)
            losePanel.SetActive(false);
    }
}