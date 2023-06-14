using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Platformer
{
    public enum State
    {
        Idle,
        Running,
        Jumping,
        Falling,
        Hurt,
    }

    public class Player : PhotonPlayer
    {
        public static Player Me;

        private static readonly int AnimState = Animator.StringToHash("state");

        private Rigidbody2D rb;
        private Animator animator;
        private new Collider2D collider;

        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float hurtForce;
        [SerializeField] private LayerMask ground;

        [SerializeField] private int cherries = 0;
        [SerializeField] public TextMeshProUGUI cherryText { private get; set; }

        private State state = State.Idle;

        // Start is called before the first frame update
        private void Start()
        {
            speed = 5f;
            jumpForce = speed * 5f;
            hurtForce = 3f * speed;
            rb = GetComponent<Rigidbody2D>();
            rb.drag = speed;
            rb.gravityScale = speed;
            animator = GetComponent<Animator>();
            collider = GetComponent<Collider2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            this.OwnerController();
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Collectable"))
            {
                Destroy(col.gameObject);
                cherries++;
                cherryText.text = cherries.ToString();
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                if (state == State.Falling)
                {
                    Destroy(col.gameObject);
                    Jump();
                }
                else
                {
                    state = State.Hurt;
                    Hurt(transform.position.x, col.gameObject.transform.position.x);
                    StartCoroutine(Stun());
                    gameObject.SetActive(false);
                }
            }
        }

        private void Hurt(float x, float otherX)
        {
            rb.velocity = x < otherX ? new Vector2(-hurtForce, rb.velocity.y) : new Vector2(hurtForce, rb.velocity.y);
        }


        private void HandleInput()
        {
            var hDirection = Input.GetAxis("Horizontal");
            Move(hDirection);

            if (Input.GetButtonDown("Jump") && collider.IsTouchingLayers(ground))
            {
                Jump();
            }
        }

        private void Move(float hDirection)
        {
            switch (hDirection)
            {
                case < 0:
                    transform.localScale = new Vector2(-1f, 1f);
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                    break;
                case > 0:
                    transform.localScale = new Vector2(1f, 1f);
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                    break;
            }
        }

        private void Jump()
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        private IEnumerator Stun()
        {
            // collider.enabled = false;
            yield return new WaitForSeconds(0.5f);
            // collider.enabled = true;
            state = State.Idle;
        }

        private void UpdateState()
        {
            if (collider.IsTouchingLayers(ground))
            {
                state = Mathf.Abs(rb.velocity.x) > speed * 0.5 ? State.Running : State.Idle;
            }
            else
            {
                if (rb.velocity.y > Mathf.Epsilon)
                {
                    state = State.Jumping;
                }
                else if (rb.velocity.y < Mathf.Epsilon)
                {
                    state = State.Falling;
                }
            }
        }
        
        protected virtual void OwnerController()
        {
            if (this.photonView.ViewID != 0 && !this.photonView.IsMine) return;
            if (state != State.Hurt)
            {
                HandleInput();
                UpdateState();
            }

            animator.SetInteger(AnimState, (int)state);
        }

    }
}