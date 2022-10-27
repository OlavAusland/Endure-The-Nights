using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Utilities;

public class PlayerCombat : MonoBehaviour
{
    
    
    public PlayerManager pm { get { return GetComponent<PlayerManager>(); } }
    public PlayerMovement pmove { get {return GetComponent<PlayerMovement>(); } }
    public SpriteRenderer sr { get { return transform.GetChild(0).GetComponent<SpriteRenderer>(); } }

    public static PlayerCombat instance;
    public AudioSource AudioSource { get { return GetComponent<AudioSource>(); } }
    
    private Weapon _weapon;
    
    public Weapon primary;
    public Weapon secondary;
    public Weapon Weapon
    {
        get
        {
            return _weapon;
        }
        set
        {
            _weapon = value;
            if (value != null)
            {
                SetWeapon(value.sprite);
                _cooldown = 0;
            }
            else
                sr.sprite = null;
            Debug.Log("Changed Weapon!");
        }
    }
    public Transform weaponTransform;
    private float _cooldown;
    
    [Header("Player Statistics")]
    private int _health;
    public int Health
    {
        get { return _health; }
        set
        {
            if(value > 0){Utilities.Health.Heal(transform, value - _health);}
            else if(value < 0){Utilities.Health.TakeDamage(transform, value - _health);}
            _health = value;
        }
    }

    public void Update()
    {
        SwapWeapon();
        if (_weapon)
        {
            if(_weapon.followMouse){Rotate();FlipWeapon();}Attack();
        }

        if (Input.GetKeyDown(KeyCode.H))
            Health -= 10;
        else if (Input.GetKeyDown(KeyCode.P))
            Health += 10;

    }

    private void SwapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Weapon = Weapon == primary ? null : primary;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Weapon = Weapon == secondary ? null : secondary;
    }

    private void Attack()
    {
        if (_cooldown > 0){_cooldown -= Time.deltaTime;return;}

        if (Input.GetMouseButton(0)) {
            _weapon.PrimaryAttack(this);
            _cooldown = Weapon.fireRate;
        }
        else if (Input.GetMouseButton(1)) {
            _weapon.SecondaryAttack(this);
            _cooldown = Weapon.fireRate;
        }

    }

    public void SetWeapon(Sprite sprite){sr.sprite = sprite;}

    private Vector2 Direction()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        return direction;
    }

    //OPTIMIZE?
    void FlipWeapon()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouse.x > transform.position.x && sr.flipY)
            sr.flipY = false;
        else if (mouse.x < transform.position.x && !sr.flipY)
            sr.flipY = true;
    }

    private void Rotate(){
        Vector2 dir = Direction();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        weaponTransform.rotation = Quaternion.AngleAxis(angle + _weapon.rotationOffset * (int)(sr.flipY ? -1 : 1), Vector3.forward);
    }
}
