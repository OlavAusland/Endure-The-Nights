using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Tools/Axe")]
public class Axe : Weapon
{
    public AnimationCurve primary;
    public LayerMask canHit;

    public AudioClip sound;

    public override void PrimaryAttack(PlayerCombat caller)
    {
        caller.StartCoroutine(Swing(caller));
    }

    public IEnumerator Swing(PlayerCombat caller)
    {
        List<Collider2D> hits = new List<Collider2D>();
        SpriteRenderer sr = caller.weaponTransform.GetComponent<SpriteRenderer>();
        Vector3 ir = caller.weaponTransform.eulerAngles;
        followMouse = false;
        caller.pmove.canFlip = false;
        float time = primary[primary.length - 1].time;
        while (time > 0)
        {
            caller.weaponTransform.eulerAngles = new Vector3(caller.weaponTransform.rotation.x,
                caller.weaponTransform.rotation.y, 
                ir.z + (sr.flipY ? -1 : 1) * Mathf.Rad2Deg * (primary.Evaluate(time) * Mathf.PI));

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(caller.transform.position, 0.35f, layerMask: canHit);

            foreach (Collider2D hit in hitColliders)
            {
                if (!hits.Contains(hit))
                {
                    Debug.Log("Hitted!");
                    if(sound){caller.AudioSource.PlayOneShot(sound);}
                    // TRAINING DUMMY TEST EXAMPLE
                    hit.GetComponent<EntityManager>().Health -= 10;
                }
                hits.Add(hit);
            }

            time -= Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
        }
        
        followMouse = true;
        caller.pmove.canFlip = true;
        yield return null;
    }
}