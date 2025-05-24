using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum WasteType
    {
        Organik,
        Anorganik,
        B3
    }

    [SerializeField] private WasteType wasteType;
    [TextArea(2, 5)]
    [SerializeField] private string itemDescription;
    public int value = 1;
    public AudioClip pickupSound;
    [Range(0.1f, 10f)] public float volumeMultiplier = 2f;

    private bool playerNearby = false;
    private Vector3 originalScale;
    private float hoverAmount = 0.2f;

    public System.Action OnPickedUp; // Tambahkan ini

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (playerNearby)
        {
            // Create slight bobbing effect
            float hoverOffset = Mathf.Sin(Time.time * 4f) * hoverAmount;
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + hoverOffset * Time.deltaTime,
                transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            transform.localScale = originalScale * 1.1f; // Slight scale up
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            transform.localScale = originalScale; // Return to original scale
        }
    }

    // Helper to get the string tag based on waste type
    public string GetWasteTypeTag()
    {
        switch (wasteType)
        {
            case WasteType.Organik: return "Organik";
            case WasteType.Anorganik: return "Anorganik";
            case WasteType.B3: return "B3";
            default: return "Untagged";
        }
    }

    // You could automatically set the tag based on the waste type
    private void OnValidate()
    {
        gameObject.tag = GetWasteTypeTag();
    }

    public void PlayPickupEffects(bool playSound = true)
    {
        if (playSound && pickupSound != null)
        {
            PlayLouderSound(pickupSound, transform.position);
        }
        OnPickedUp?.Invoke(); // Tambahkan ini
    }

    private void PlayLouderSound(AudioClip clip, Vector3 position)
    {
        // Create temporary GameObject for audio playback
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;

        // Add and configure AudioSource
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;

        // Apply volume boost (clamped to avoid distortion)
        audioSource.volume = Mathf.Clamp(volumeMultiplier, 0.1f, 10f);

        // Configure for clean playback
        audioSource.spatialBlend = 1.0f; // 3D sound
        audioSource.maxDistance = 20f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        audioSource.Play();

        // Destroy after playback completes
        Destroy(tempGO, clip.length);
    }

    public string GetItemDescription()
    {
        return itemDescription;
    }
}