using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public bool haveShotgun = false;
    public GameObject pistol;
    public GameObject shotgun;
    private int currentWeapon = 0;  
     private AudioSource audioSource;
    public AudioClip weaponPickup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        pistol.SetActive(true);
        shotgun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!haveShotgun) return;
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if(scroll < 0f || scroll > 0f)
        {
            SwitcWeapon();
        }


    }

    void SwitcWeapon()
    {
        currentWeapon = 1 - currentWeapon;
        
        pistol.SetActive(currentWeapon == 0);
        shotgun.SetActive(currentWeapon == 1);

    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Shotgun")) return;

        if (weaponPickup && audioSource)
    {
        audioSource.PlayOneShot(weaponPickup);
    }

        haveShotgun = true;
        Destroy(other.gameObject);
    }

}
