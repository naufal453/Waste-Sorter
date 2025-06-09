using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupItemSpawner : MonoBehaviour
{
    public GameObject pickupItemPrefab;
    public int spawnCount = 5;
    public Vector2 spawnAreaMin = new Vector2(-5, -5);
    public Vector2 spawnAreaMax = new Vector2(5, 5);
    public float spawnInterval = 3f;

    public TMP_Text timerText; // Assign this in the Inspector or create at runtime

    private int spawnedCount = 0;
    private float timer = 0f;

    void Start()
    {
        // If timerText is not assigned, create it at runtime
        if (timerText == null)
        {
            GameObject canvasObj = GameObject.Find("Canvas");
            if (canvasObj == null)
            {
                canvasObj = new GameObject("Canvas");
                var canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
            }

            GameObject textObj = new GameObject("TimerText");
            textObj.transform.SetParent(canvasObj.transform);
            timerText = textObj.AddComponent<TMP_Text>();
            timerText.font = TMP_Settings.defaultFontAsset;
            timerText.fontSize = 32;
            timerText.alignment = TextAlignmentOptions.Center;
            timerText.color = Color.black;
            RectTransform rect = timerText.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(0, -20);
            rect.sizeDelta = new Vector2(400, 60);
        }

        timer = spawnInterval;
        InvokeRepeating(nameof(SpawnItem), 0f, spawnInterval);
    }

    void Update()
    {
        if (spawnedCount < spawnCount)
        {
            timer -= Time.deltaTime;
            if (timer < 0f) timer = 0f;
            if (timerText != null)
                timerText.text = $"{timer:F1}s";
            if (timer == 0f)
                timer = spawnInterval;
        }
        else
        {
            if (timerText != null)
                timerText.text = "All items spawned!";
        }
    }

    void SpawnItem()
    {
        if (spawnedCount >= spawnCount)
        {
            CancelInvoke(nameof(SpawnItem));
            return;
        }

        Vector2 randomPos = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );
        Instantiate(pickupItemPrefab, randomPos, Quaternion.identity);
        spawnedCount++;
        timer = spawnInterval; // Reset timer after spawn
    }
}