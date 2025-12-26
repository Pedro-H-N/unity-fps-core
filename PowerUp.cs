using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public float duration = 3f;

    protected abstract void Activate();

    private AudioSource audioSource;
    public AudioClip powerupPickup;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        AudioSource playerAudio = other.GetComponent<AudioSource>();
        if (playerAudio && powerupPickup)
        {
            playerAudio.PlayOneShot(powerupPickup);
        }

        Activate();
        Destroy(gameObject);
    }
}