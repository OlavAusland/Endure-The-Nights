using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerCombat pc { get { return GetComponent<PlayerCombat>(); } }


    SpriteRenderer sr { get { return GetComponent<SpriteRenderer>(); } }
    public Rigidbody2D rb2D { get { return GetComponent<Rigidbody2D>(); } }

    public float speed = 0.5f;
    public float maxSpeed;
    private Vector2 velocity;

    [Header("Animation")]
    public AnimationCurve flip;
    public AnimationCurve move;
    private float _moveTime;

    public bool canFlip = true;



    // Update is called once per frame
    void Update()
    {
        Movement();
        if(canFlip){Flip();}
    }

    public void Movement()
    {
        velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;

        if (velocity.magnitude > 0)
        {
            AnimateMove();
            if (velocity.magnitude > maxSpeed)
                velocity = (maxSpeed / velocity.magnitude) * velocity;
        }
        else{transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);}
        rb2D.MovePosition(transform.position + (Vector3)velocity);
    }

    private void Flip()
    {
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        if (sr.flipX && mouse.x > transform.position.x){
            StartCoroutine(AnimateFlip());
        }
        else if (!sr.flipX && mouse.x < transform.position.x){
            StartCoroutine(AnimateFlip());
        }
    }

    public IEnumerator AnimateFlip()
    {
        float time = flip[flip.length - 1].time;
        sr.flipX = !sr.flipX;
        while (time > 0)
        {
            transform.localScale = new Vector3(flip.Evaluate(time), 1, 1);
            time -= Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = new Vector3(1, 1, 1);
        yield return null;
    }

    public void AnimateMove()
    {
        if(_moveTime <= 0){_moveTime = move[move.length - 1].time;}
        
        transform.eulerAngles = new Vector3(transform.rotation.x,
            transform.rotation.y, Mathf.Rad2Deg * (move.Evaluate(_moveTime) * Mathf.PI));

        _moveTime -= Time.deltaTime;
    }
}
