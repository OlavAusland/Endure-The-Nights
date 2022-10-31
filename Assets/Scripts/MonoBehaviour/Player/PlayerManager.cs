using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    PlayerCombat pc
    {
        get { return GetComponent<PlayerCombat>(); }
    }

    PlayerMovement pm
    {
        get { return GetComponent<PlayerMovement>(); }
    }

    public LayerMask layerMask;
    public InventoryManager im;
    public float pickupRange;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){Pickup();}
    }

    public void Pickup()
    {
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mouse, Vector2.zero, 4f, layerMask);

        if (hit.collider != null)
        {
            if (Vector2.Distance(hit.transform.position, mouse) < pickupRange)
            {
                im.AddItem(hit.transform.GetComponent<ItemInformation>().item);
                Destroy(hit.transform.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
