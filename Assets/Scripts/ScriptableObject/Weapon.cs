using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAttack
{
    public virtual void PrimaryAttack(PlayerCombat caller) { }
    public virtual void SecondaryAttack(PlayerCombat caller) { }
}


[CreateAssetMenu(menuName = "Weapon")]
public class Weapon : Item, IAttack
{
    Camera camera { get{return Camera.main;}}

    public int rotationOffset;
    public bool followMouse = true;
    public float fireRate;

    public virtual void PrimaryAttack(PlayerCombat caller) {MonoBehaviour.print("Primary Attack");}
    public virtual void SecondaryAttack(PlayerCombat caller) { MonoBehaviour.print("Secondary Attack"); }

    public Vector2 Direction(Transform caller, bool normalized = true)
    {
        Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)caller.position).normalized;
        return normalized ? direction.normalized : direction;
    }
}