using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip mudZoneClip; // Sound for mud zone
    [SerializeField] private AudioClip floodZoneClip; // New sound for flood zone
    private AudioSource audioSource;

    [Header("Slow Zone Settings")]
    [SerializeField] private float slowSpeed = 2f; // For mud zone
    [SerializeField] private float floodSpeed = 1.5f; // For flood zone (even slower)
    private float originalSpeed;
    private bool isInMudZone = false; // Track if player is in mud zone
    private bool isInFloodZone = false; // Track if player is in flood zone

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    [Header("Player Stats")]
    [SerializeField] private int maxHealth = 100;
    private int health;
    private int score;

    [Header("Pickup System")]
    public static UnityEvent<int> OnItemPickedUp = new UnityEvent<int>();
    private int itemCount = 0;

    [Header("Item Counting")]
    private int organikCount = 0;
    private int anorganikCount = 0;
    private int b3Count = 0;

    [Header("Inventory Limits")]
    public int maxItemCapacity = 4; // Changed to public

    [Header("Item Handling")]
    [SerializeField] private KeyCode placeItemKey = KeyCode.Space;
    [SerializeField] private GameObject organikItemPrefab;
    [SerializeField] private GameObject anorganikItemPrefab;
    [SerializeField] private GameObject b3ItemPrefab;
    [SerializeField] private Transform heldItemPosition;

    [Header("UI Elements")]
    [SerializeField] private GameObject fullCapacityText; // Assign a Text or TextMeshPro object
    [SerializeField] private Vector3 textOffset = new Vector3(0, 1.5f, 0); // Position above player's head

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = maxHealth;
        originalSpeed = moveSpeed; // Save original speed
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;

        // Cek untuk setiap tipe sampah yang dimiliki, jika tombol letak ditekan
        if (Input.GetKeyDown(placeItemKey))
        {
            TryPlaceItem();
        }

        // Update UI position and visibility
        UpdateCapacityUI();

        // Visual indicator of capacity
        if (GetTotalItemCount() >= maxItemCapacity)
        {
            // Make player flash or change color to indicate they're at capacity
            // This example uses a simple visual cue - you can improve it
            SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                // Flash the player sprite with a subtle red tint to indicate full capacity
                float pulseValue = Mathf.PingPong(Time.time * 2, 0.3f);
                playerSprite.color = new Color(1, 1 - pulseValue, 1 - pulseValue);
            }

            // Show the full text
            if (fullCapacityText != null)
            {
                fullCapacityText.SetActive(true);
            }
        }
        else
        {
            // Reset to normal color when not at capacity
            SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                playerSprite.color = Color.white;
            }

            // Hide the full text
            if (fullCapacityText != null)
            {
                fullCapacityText.SetActive(false);
            }
        }
    }

    private void UpdateCapacityUI()
    {
        if (fullCapacityText != null)
        {
            // Position the text above the player's head
            fullCapacityText.transform.position = transform.position + textOffset;

            // Optional: Add a floating/bobbing effect
            Vector3 position = fullCapacityText.transform.position;
            position.y += Mathf.Sin(Time.time * 4) * 0.05f; // Small bobbing effect
            fullCapacityText.transform.position = position;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            moveInput = Vector2.zero;
            animator.SetBool("isWalking", false);
            animator.SetFloat("lastInputX", moveInput.x);
            animator.SetFloat("lastInputY", moveInput.y);

            // Stop any audio when player stops moving
            if (audioSource.isPlaying)
                audioSource.Stop();

            return;
        }

        moveInput = context.ReadValue<Vector2>();
        animator.SetBool("isWalking", true);
        animator.SetFloat("inputX", moveInput.x);
        animator.SetFloat("inputY", moveInput.y);

        // Play appropriate sound based on zone
        if (!audioSource.isPlaying && moveInput.magnitude > 0.1f)
        {
            if (isInFloodZone)
            {
                // Play flood sound if in flood zone
                audioSource.clip = floodZoneClip;
                audioSource.loop = true;
                audioSource.Play();
            }
            else if (isInMudZone)
            {
                // Play mud sound if in mud zone
                audioSource.clip = mudZoneClip;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                // Play normal footstep sound
                audioSource.clip = footstepClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check for item pickup
        PickupItem pickup = other.GetComponent<PickupItem>();
        if (pickup != null)
        {
            // Check if player has reached maximum capacity
            int totalItems = organikCount + anorganikCount + b3Count;
            if (totalItems >= maxItemCapacity)
            {
                // Player is at max capacity - maybe show a message
                Debug.Log("Cannot pick up more items! Maximum capacity reached.");
                return; // Skip pickup
            }

            switch (other.tag)
            {
                case "Anorganik":
                    anorganikCount++; // Make sure this is incrementing, not assigning
                    OnItemPickedUp.Invoke(0);
                    break;
                case "Organik":
                    organikCount++; // Make sure this is incrementing, not assigning
                    OnItemPickedUp.Invoke(0);
                    break;
                case "B3":
                    b3Count++; // Make sure this is incrementing, not assigning
                    OnItemPickedUp.Invoke(0);
                    break;
            }

            HandlePickup(pickup);
            return;
        }

        if (other.CompareTag("MudZone"))
        {
            moveSpeed = slowSpeed;
            isInMudZone = true; // Set flag to true

            // Stop regular footstep sound if it's playing
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Only play mud zone sound if player is moving
            if (moveInput.magnitude > 0.1f && mudZoneClip != null)
            {
                audioSource.clip = mudZoneClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        if (other.CompareTag("FloodZone"))
        {
            moveSpeed = floodSpeed; // Even slower than mud
            isInFloodZone = true;

            // Stop any other sounds if playing
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Only play flood zone sound if player is moving
            if (moveInput.magnitude > 0.1f && floodZoneClip != null)
            {
                audioSource.clip = floodZoneClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        if (other.CompareTag("Anorganik") || other.CompareTag("Organik") ||
            other.CompareTag("B3"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MudZone"))
        {
            moveSpeed = originalSpeed;
            isInMudZone = false;

            // Stop mud zone sound
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Resume footstep sound if player is moving
            if (moveInput.magnitude > 0.1f)
            {
                audioSource.clip = footstepClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        if (other.CompareTag("FloodZone"))
        {
            moveSpeed = originalSpeed;
            isInFloodZone = false;

            // Stop flood zone sound
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // Resume footstep sound if player is moving
            if (moveInput.magnitude > 0.1f)
            {
                audioSource.clip = footstepClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    public void HandlePickup(PickupItem pickup)
    {
        if (pickup == null) return;

        // Set the current held item type based on the tag
        // currentHeldItemType = pickup.gameObject.tag;
        // isHoldingItem = true;

        // Visual feedback for holding an item
        GameObject itemPrefab = null;
        switch (pickup.gameObject.tag)
        {
            case "Organik":
                // Don't increment here since it's already incremented in OnTriggerEnter2D
                itemPrefab = organikItemPrefab;
                break;
            case "Anorganik":
                // Don't increment here since it's already incremented in OnTriggerEnter2D
                itemPrefab = anorganikItemPrefab;
                break;
            case "B3":
                // Don't increment here since it's already incremented in OnTriggerEnter2D
                itemPrefab = b3ItemPrefab;
                break;
        }

        // Create visual representation of held item
        if (itemPrefab != null && heldItemPosition != null)
        {
            Instantiate(itemPrefab, heldItemPosition.position, Quaternion.identity, heldItemPosition);
        }

        pickup.PlayPickupEffects();
        Destroy(pickup.gameObject);

        UpdateHeldItemsVisual(); // Tambahkan ini
    }

    private void TryPlaceItem()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);

        foreach (Collider2D collider in colliders)
        {
            TrashBin bin = collider.GetComponent<TrashBin>();
            if (bin != null)
            {
                // Cek tipe bin dan apakah player punya sampah tipe itu
                bool placed = false;
                switch (bin.BinType) // tambahkan public getter BinType di TrashBin
                {
                    case "Organik":
                        if (organikCount > 0)
                        {
                            placed = bin.TryPlaceItem("Organik");
                            if (placed) organikCount--;
                        }
                        break;
                    case "Anorganik":
                        if (anorganikCount > 0)
                        {
                            placed = bin.TryPlaceItem("Anorganik");
                            if (placed) anorganikCount--;
                        }
                        break;
                    case "B3":
                        if (b3Count > 0)
                        {
                            placed = bin.TryPlaceItem("B3");
                            if (placed) b3Count--;
                        }
                        break;
                }

                if (placed)
                {
                    score += 10;
                    UpdateHeldItemsVisual();
                    OnItemPickedUp.Invoke(0);
                    Debug.Log("Item placed successfully.");
                    return;
                }
                else
                {
                    score -= 5;
                    Debug.Log("Incorrect placement or no matching item.");
                }
            }
        }
    }

    // Tambahkan fungsi ini untuk update visual item yang dipegang
    private void UpdateHeldItemsVisual()
    {
        if (heldItemPosition != null)
        {
            foreach (Transform child in heldItemPosition)
            {
                Destroy(child.gameObject);
            }
            // Tampilkan visual sesuai jumlah tiap tipe
            for (int i = 0; i < organikCount; i++)
                Instantiate(organikItemPrefab, heldItemPosition.position, Quaternion.identity, heldItemPosition);
            for (int i = 0; i < anorganikCount; i++)
                Instantiate(anorganikItemPrefab, heldItemPosition.position, Quaternion.identity, heldItemPosition);
            for (int i = 0; i < b3Count; i++)
                Instantiate(b3ItemPrefab, heldItemPosition.position, Quaternion.identity, heldItemPosition);
        }
    }

    private void PlayMudZoneSound()
    {
        if (audioSource != null && mudZoneClip != null)
        {
            audioSource.clip = mudZoneClip;
            audioSource.Play();
        }
    }

    // Getter methods for UI
    public int GetAnorganikCount() => anorganikCount;
    public int GetOrganikCount() => organikCount;
    public int GetB3Count() => b3Count;

    public int GetScore() => score;
    public float GetHealthPercentage() => (float)health / maxHealth;
    public int GetItemCount() => itemCount;

    public int GetTotalItemCount()
    {
        return organikCount + anorganikCount + b3Count;
    }
}