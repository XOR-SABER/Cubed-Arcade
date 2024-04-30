using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public Weapon primary;
    public Weapon secondary;
    public Transform weaponPickupPoint;
    public Vector3 EquipPos;
    public GameObject offhandSlot;
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
        // Todo: make Angel Look at this 
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
        Debug.Log("Weapons Detected: " + nearbyWeaponColliders.Length.ToString());
        foreach (Collider2D nearbyWeaponCollider in nearbyWeaponColliders)
        {
            Weapon newWeapon = nearbyWeaponCollider.GetComponent<Weapon>();
            // If no weapon is not avaliable
            if(newWeapon == null) continue;
            // If we already have it as primary
            if(newWeapon == primary) continue;
            // If we already have it as secondary
            if(newWeapon == secondary) continue;
            // If already have a different 
            if (secondary != null) {
                // Drop the bitch.. 
                Instantiate(secondary.gameObject, transform);
                Destroy(secondary.gameObject);
            }

            // Okay Finally assign the weapon.    
            secondary = newWeapon;
            // secondary.transform.parent = weaponPickupPoint.transform;
            // secondary.transform.localPosition = weaponPickupPoint.transform.position;
            // secondary.transform.localRotation = weaponPickupPoint.transform.rotation;
            dequipWeapon(primary);
            var transform1 = secondary.transform;
            transform1.parent = transform;
            transform.position = weaponPickupPoint.transform.position;
            transform.localRotation = weaponPickupPoint.transform.rotation;
            //  transform1.parent = weaponPickupPoint.transform;
            // transform1.localPosition = Vector3.zero;
            transform1.parent = weaponPickupPoint.transform;
            // transform1.localPosition = Vector3.zero;
            // transform1.localRotation = Quaternion.identity;
            


            
            EquipSecondary(); 
            
            Debug.Log("Picked up " + secondary.name);
            return; 
        }
    }

    void EquipPrimary()
    {
        dequipWeapon(secondary);
        equipWeapon(primary);
    }

    void EquipSecondary()
    {
        dequipWeapon(primary);
        equipWeapon(secondary);
    }

    void dequipWeapon(Weapon toDequip) {
        toDequip.isEquipped = false;
        toDequip.transform.localPosition = offhandSlot.transform.position;
    }

    void equipWeapon(Weapon toEquip) {
        toEquip.isEquipped = true;
        // toEquip.transform.parent = weaponPickupPoint.transform;
        // toEquip.transform.rotation = Quaternion.identity;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(weaponPickupPoint.position, pickupDistance);
    }
    
}
