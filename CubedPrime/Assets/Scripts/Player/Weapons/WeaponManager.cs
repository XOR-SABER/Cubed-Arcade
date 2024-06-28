using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class WeaponManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    
    private Weapon _mainWeapon;
    public GameObject mainWeaponSlot;
    public GameObject secondaryWeaponSlot;
    public GameObject primary;
    public GameObject secondary;

    private Collider2D[] _weaponsOnGround = new Collider2D[1024];
    private PlayersControls _playersControls;
    private InputAction _shoot;
    private InputAction _pickUp;
    public int pickupDistance;
    private InputAction _switch;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (primary != null)
        {
            primary = Instantiate(primary, mainWeaponSlot.transform);
            primary.transform.position = mainWeaponSlot.transform.position;
            primary.transform.rotation = mainWeaponSlot.transform.rotation;
            primary.tag = "weaponEquipped";
        }
        else
        {
            Debug.Log("the player start with no weapon. intended?");
        }
        

        if (secondary != null)
        {
            secondary = Instantiate(secondary, secondaryWeaponSlot.transform);
            secondary.transform.position = secondaryWeaponSlot.transform.position;
            secondary.transform.rotation = secondaryWeaponSlot.transform.rotation;
            secondary.tag = "weaponEquipped";
            
            if (secondary.GetComponent<Weapon>().isDualSprite)
            {
                secondary.GetComponent<Weapon>().gameObject.GetComponent<SpriteRenderer>().enabled = true;
                secondary.GetComponent<Weapon>().gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
            
        }
        
        _mainWeapon = primary.GetComponent<Weapon>();
        
        
        _playersControls = new PlayersControls();
        
        _shoot = _playersControls.Player.Fire;
        _shoot.Enable();
        
        _pickUp = _playersControls.Player.PickUp;
        _pickUp.Enable();
        _pickUp.performed += _ => TryPickUp();

        _switch = _playersControls.Player.Switch;
        _switch.Enable();
        _switch.performed += _ => SwitchWeapon();
    }
    
    private void OnEnable()
    {
        _shoot.Enable();
        _pickUp.Enable();
        _switch.Enable();
    }

    private void OnDisable()
    {
        _shoot.Disable();
        _pickUp.Disable();
        _switch.Disable();
    }

    // Pick up code
    public void TryPickUp()
    {
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, pickupDistance, _weaponsOnGround);
        for (int i = 0; i < size; i++)
        {
            if (_weaponsOnGround[i].CompareTag("weapon"))
            {
                if (secondary != null)
                {
                    var offset = Vector3.zero;
                    offset.x += 4;
                    offset.y += 4;
                    Instantiate(secondary, transform.position + offset, transform.rotation).tag = "weapon";
                    Destroy(secondary);
                }
                secondary = Instantiate(_weaponsOnGround[i].gameObject, mainWeaponSlot.transform);
                secondary.transform.position = secondaryWeaponSlot.transform.position;
                secondary.transform.rotation = secondaryWeaponSlot.transform.rotation;
                if (secondary.GetComponent<Weapon>().isDualSprite)
                {
                    secondary.GetComponent<Weapon>().gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    secondary.GetComponent<Weapon>().gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
                }
                Destroy(_weaponsOnGround[i].gameObject);
                secondary.tag = "weaponEquipped";
                return;
            }
        }
        Debug.Log("No weapon here");
    }

    public void SwitchWeapon()
    {
        if (secondary != null)
        {
            (primary, secondary) = (secondary, primary);
            primary.transform.position = mainWeaponSlot.transform.position;
            primary.transform.rotation = mainWeaponSlot.transform.rotation;
            primary.gameObject.GetComponent<Weapon>().isEquiped = true;
            
            secondary.transform.position = secondaryWeaponSlot.transform.position;
            secondary.transform.rotation = secondaryWeaponSlot.transform.rotation;
            secondary.gameObject.GetComponent<Weapon>().isEquiped = false;
            if (_mainWeapon.isDualSprite)
            {
                _mainWeapon.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                _mainWeapon.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }
            _mainWeapon = primary.GetComponent<Weapon>();
            if (_mainWeapon.isDualSprite)
            {
                _mainWeapon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                _mainWeapon.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
            }
        }
    }
    
    private void Update()
    {
        if (playerMovement.inputType is PlayerMovement.InputType.Mobile or PlayerMovement.InputType.Controller)
        {
            return;
        }
        if (_shoot.IsPressed())
        {
            _mainWeapon.ShootOrder();
        }
        else
        {
            _mainWeapon.ResetTimeHeld();
        }
    }


    public void ResetTimeHeld()
    {
        _mainWeapon.ResetTimeHeld();
    }
    public void Shoot()
    {
        _mainWeapon.ShootOrder();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }
}
