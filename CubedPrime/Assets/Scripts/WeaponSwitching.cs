using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public Weapon primary;
    public Weapon secondary;
    public Transform weaponPickupPoint;
    public GameObject secondaryHolster;
    public LayerMask weaponLayer;
    public float pickupDistance = 2f;
    
    private int currentWeaponIndex;
    
    //TODO: Make logic to pickup other guns to replace 2nd index.
    

    void Start()
    {
        primary.isEquipped = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUpWeapon();
        }
    }

    void SwitchWeapon()
    {
        if (secondary == null)
        {
            return;
        }
        if (primary.isEquipped)
        {
            EquipSecondary();
        }
        else
        {
            EquipPrimary();
        }
    }

    void PickUpWeapon()
    {
        Collider2D[] nearbyWeaponColliders = Physics2D.OverlapCircleAll(weaponPickupPoint.position, pickupDistance, weaponLayer);
        foreach (Collider2D nearbyWeaponCollider in nearbyWeaponColliders)
        {
            Weapon newWeapon = nearbyWeaponCollider.GetComponent<Weapon>();
            if (newWeapon != null && newWeapon != primary && newWeapon != secondary)
            {
                if (secondary != null)
                {
                    Destroy(secondary.gameObject);
                }

                secondary = newWeapon;
                var transform1 = secondary.transform;
                transform1.parent = secondaryHolster.transform;
                transform1.localPosition = Vector3.zero;
                transform1.localRotation = Quaternion.identity;
                
                EquipSecondary(); 
                
                Debug.Log("Picked up " + secondary.name);
                return; 
            }
        }
    }

    void EquipPrimary()
    {
        secondary.isEquipped = false;
        secondary.gameObject.SetActive(false);
        primary.isEquipped = true;
        primary.gameObject.SetActive(true);
    }

    void EquipSecondary()
    {
        primary.isEquipped = false;
        primary.gameObject.SetActive(false);
        secondary.isEquipped = true;
        secondary.gameObject.SetActive(true);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(weaponPickupPoint.position, pickupDistance);
    }
    
}
