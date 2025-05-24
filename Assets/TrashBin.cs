using UnityEngine;
using TMPro;

public class TrashBin : MonoBehaviour
{
    [SerializeField] private string binType; // "Organik", "Anorganik", or "B3"
    [SerializeField] private TextMeshProUGUI countText; // To display count of items
    [SerializeField] private AudioClip correctPlacementSound;
    [SerializeField] private AudioClip wrongPlacementSound;
    [SerializeField] private AudioClip placeItemSound; // Tambahkan SFX baru
    [SerializeField] private ParticleSystem correctEffect;

    private int itemCount = 0;
    private AudioSource audioSource;
    private Vector3 originalScale;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Store the original scale when the game starts
        originalScale = transform.localScale;

        UpdateCountDisplay();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player entered the trigger
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            // Visual feedback that player is near bin - scale up by 10%
            transform.localScale = originalScale * 1.1f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if player left the trigger
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            // Reset to original scale
            transform.localScale = originalScale;
        }
    }

    public bool TryPlaceItem(string itemType)
    {
        bool isCorrectBin = (itemType == binType);

        // Mainkan sound effect umum setiap kali meletakkan sampah
        if (placeItemSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(placeItemSound, 0.5f); // Use volume scale
        }

        if (isCorrectBin)
        {
            // Item placed in correct bin
            itemCount++;
            UpdateCountDisplay();

            // Play correct placement effect
            if (correctPlacementSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(correctPlacementSound, 0.5f); // Use volume scale
            }

            if (correctEffect != null)
            {
                correctEffect.Play();
            }
        }
        else
        {
            // Item placed in wrong bin
            if (wrongPlacementSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(wrongPlacementSound, 0.5f); // Use volume scale
            }
        }

        return isCorrectBin;
    }

    private void UpdateCountDisplay()
    {
        if (countText != null)
        {
            countText.text = $"{binType}: {itemCount}";
        }
    }

    public string BinType => binType; // Tambahkan getter publik
}
