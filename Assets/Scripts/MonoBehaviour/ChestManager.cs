using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    private bool _canOpen;
    private bool _isOpen;
    private Animator _anim {get{return GetComponent<Animator>();}}


    public void Update()
    {
        if(!_canOpen){return;}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _anim.Play(_isOpen ? "ChestClose" : "ChestOpen");
            _isOpen = !_isOpen;
        }
    }

    public void OnTriggerExit2D(Collider2D other){ if(other.CompareTag("Player")){ _canOpen = false;} }

    public void OnTriggerEnter2D(Collider2D other) { if(other.CompareTag("Player")){_canOpen = true;} }
}
