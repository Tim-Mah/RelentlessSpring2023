using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    private bool isFacingRight = true;
    private float doubleClickTime = 0.2f;

    // Player Physics
    [SerializeField] private Rigidbody2D rb2;
    [SerializeField] private BoxCollider2D bc2;
    [SerializeField] private BoxCollider2D slideCollider;

    // Detection Objects
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    // Terrain Layers
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    // Movement Attributes
    private float horizontal;
    private bool run = false;
    private float walkSpeed = 375f;
    private float runSpeed = 600f;
    private float maxSpeed = 12f;

    // Acceleration Boosts
    private float accelBoost = 3f;
    private float turnBoost = 2.5f;
    private float airAccelBoost = 5f;
    private float coldStartRunBoost = 3f;
    private float runStopForceFactor = 3;

    // Jump Attributes
    private bool jumpRelease = false;
    private bool jump = false;
    private float jumpPower = 14f;
    private float playerGravity = 3f;
    private float jumpGravity = 1f;
    private float fallGravity = 20f;
    private float maxFallSpeed = 750f;
    // Coyote Time makes jumps forgiving
    private float coyoteTime = 0.2f;
    private float coyoteTimer;

    // Wall Movement
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 2f;
    private float wallJumpTimer;
    private float walljumpDuration = 0.4f;
    private Vector2 wallJumpPower = new Vector2(8f, 16f);

    // Slide Movement
    private float drag = 1f;
    private float slideSpeed = 1500f;
    private bool slide = false;
    private bool isSliding = false;
    private float slideTime = 0.8f;
    private float slideCooldown = 0.15f;
    private float slideTimer;

    private bool crouch = false;

    // Double Clicking
    private float lastDownClick;
    private float lastMoveClick;
    private bool wasRight = false;
    private bool wasRunnging = false;

    // Ladder Movement
    private float vertical;
    private readonly float ladderSpeed = 8f;
    private bool isLadder;
    private bool isClimbing;

    // Dash Movement
    private bool dash = false;
    private bool canDash = true;
    private bool isDashing;
    private float dashPower = 35f;
    private float dashTime = 0.2f;
    private float dashCooldown = 0.2f;
    [SerializeField] private TrailRenderer trail;

    private bool onMovable = false;
    private Rigidbody2D movable;

    private bool onIce = false;
    private float iceVelocity;

    //Sound effects
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource wallJumpSound;
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private float stepFrames = 30;

    [Range(0.1f, 0.5f)]
    public float volumeChangeMult = 0.2f;

    [Range(0.1f, 0.5f)]
    public float pitchChangeMult = 0.35f;
    private float step;


    public AudioClip[] sounds;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        bc2.enabled = true;
        slideCollider.enabled = false;
        source = GetComponent<AudioSource>();
        step = stepFrames;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
        if (collision.CompareTag("Moving Ridable"))
        {
            print("Collision detected");
            onMovable = true;
        }
        if (collision.CompareTag("Ice"))
        {
            onIce = true;
            iceVelocity = rb2.velocity.x;
        }
    }

        private void OnTriggerStay2D(Collider2D collision)
        {
            movable = collision.gameObject.GetComponent<Rigidbody2D>();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Ladder"))
            {
                isLadder = false;
                isClimbing = false;
            }
            if (collision.CompareTag("Moving Ridable"))
            {
                onMovable = false;
            }
            if (collision.CompareTag("Ice"))
            {
                onIce = false;

            }
        }

        /**
         * Sets jump to true if pressed
         */
        public void Jump(InputAction.CallbackContext context)
        {
            jump = context.performed;
            jumpRelease = context.canceled;
        }

        /**
         * Makes the player jump
         */
        private void JumpCode()
        {
            if (isLadder)
            {
                return;
            }
            /*
             * Jumping action
             */
            if (jump)
            {
                if (IsGrounded())
            {
                jumpSound.Play();
            }
                if (IsWalled())
            {
                wallJumpSound.Play();
            }
            if (coyoteTimer > 0f)
                {
                    coyoteTimer = 0f;
                    rb2.velocity = new Vector2(rb2.velocity.x, 0);
                    rb2.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                }
            }
        }
        /**
         * Movement button is hit, changes horizontal, a
         * variable tracking if left (-1), right (1), or
         * no input (0) was hit.
         */
        public void Move(InputAction.CallbackContext context)
        {
            bool doubleClickSameDirection;
            if (wasRight == true && context.ReadValue<Vector2>().x >= 0f)
            {
                doubleClickSameDirection = true;
            } else if (wasRight == false && context.ReadValue<Vector2>().x < 0f)
            {
                doubleClickSameDirection = true;
            }
            else
            {
                doubleClickSameDirection = false;
            }

            if ((Time.time - lastMoveClick) <= doubleClickTime && (doubleClickSameDirection || wasRunnging))
            {
                run = context.performed;
            }
            horizontal = context.ReadValue<Vector2>().x;

            if (context.canceled)
            {
                if (isFacingRight)
                {
                    wasRight = true;
                }
                else
                {
                    wasRight = false;
                }
                run = false;
                lastMoveClick = Time.time;
            }

            vertical = context.ReadValue<Vector2>().y;

    
    }

        /** 
         * Force Movement code (Adds force instead of setting velocity)
         */
        private void MoveCode()
        {
            // If it is sliding, don't do anything
            if (isSliding || (isClimbing && !IsGrounded()) || isDashing)
            {
                return;
            }

            /*
             * Stopping the player
             */
            if (horizontal == 0f && IsGrounded())
            {
                // Add large force in other direction
                rb2.AddForce(new Vector2(rb2.velocity.x * -1 * runStopForceFactor, rb2.velocity.y));

                // Activate boost when they start running
                accelBoost = coldStartRunBoost;
            }
            else if (!IsGrounded())
            {
                // Movement vector (-1, 0, or 1)
                Vector2 movementDirection = new Vector2(horizontal, 0);
                rb2.AddForce(movementDirection * walkSpeed * airAccelBoost * Time.fixedDeltaTime);
            }
            else
            // Player ground movement
            {
                Vector2 movementDirection = new Vector2(horizontal, 0);
                if (run)
                {
                    rb2.AddForce(movementDirection * runSpeed * accelBoost * Time.fixedDeltaTime);
                    wasRunnging = true;
                } else
                {
                    rb2.AddForce(movementDirection * walkSpeed * accelBoost * Time.fixedDeltaTime);
                    wasRunnging = false;
                }
            }

            if (isLadder && Mathf.Abs(vertical) > 0f)
            {
                isClimbing = true;
            }
        }

        /**
         * Jump Feel (modify jump after it's been pressed)
         */

        private void JumpFeel()
        {
            if (isDashing)
            {
                return;
            }
            /*
             * Shorten jump
             */
            if (jumpRelease && rb2.velocity.y > 0f)
            {
                rb2.gravityScale = fallGravity * 20f;
            }
            else
            {
                rb2.gravityScale = playerGravity;
            }
            /*
            * Apex control (top of jump gets lower gravity)
            */
            if (rb2.velocity.y >= 0f && rb2.velocity.y < 0.1f && !IsGrounded())
            {
                rb2.gravityScale = jumpGravity;
            }
            // Fast fall
            if (rb2.velocity.y < -0.1f)
            {
                rb2.gravityScale = fallGravity;
            }
        }

        /**
         * Makes the player slide down a wall
         */
        private void WallSlide()
        {
            // prevent wall sliding and sliding
            if (isSliding || isDashing)
            {
                return;
            }

            if (IsWalled() && !IsGrounded() && horizontal != 0f)
            {
                isWallSliding = true;

                // Stops all upward movement
                rb2.velocity = new Vector2(rb2.velocity.x, -5);

                // Makes sliding slower
                rb2.gravityScale = wallSlidingSpeed;
            }
            else
            {
                rb2.gravityScale = playerGravity;
                isWallSliding = false;
            }

        }

        /**
         * Makes the player wall jump
         */
        private void WallJump()
        {
            // prevent wall jumping and sliding
            if (isSliding)
            {
                return;
            }

            if (isWallSliding)
            {
                isWallJumping = false;
                wallJumpDirection = -transform.localScale.x;
                wallJumpTimer = wallJumpTime;
                CancelInvoke(nameof(StopWallJumping));
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }

            // Jump
            if (jump && wallJumpTimer > 0f && isWallSliding)
            {
                isWallJumping = true;
                rb2.velocity = new Vector2(0, 0);
                rb2.AddForce(new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y), ForceMode2D.Impulse);
                wallJumpTimer = 0f;

                // Flip player to face the same way as jump
                if (transform.localScale.x != wallJumpDirection)
                {
                    Flip();
                }

                Invoke(nameof(StopWallJumping), walljumpDuration);
            }
        }

        /**
         * Stops a wall jump
         */
        private void StopWallJumping()
        {
            isWallJumping = false;
        }

        /**
         * If slide button is hit
         */
        public void Crouch(InputAction.CallbackContext context)
        {
            if ((Time.time - lastDownClick) <= doubleClickTime && Equals(slideTimer, 0f))
            {
                slide = context.performed;
            }
            else
            {
                crouch = context.performed;
            }

            if (context.canceled)
            {
                lastDownClick = Time.time;
            }
        }


        /**
         * Rotates sprite player 90 degrees
         */
        private void slideRot()
        {

            // Needs to actually be nice. The slide collider also needs to change, so that
            // when switched, it doesn't instantly clip (it should be a rotation of
            // the reg one, pivit on the ground
            if (isFacingRight)
            {
                sprite.transform.Rotate(new Vector3(0, 0, 90));
            }
            else
            {
                sprite.transform.Rotate(new Vector3(0, 0, -90));
            }
        }


        /**
         * Makes the player ground slide
         */
        private void GroundSlide()
        {
            if (slide && IsGrounded() && !isSliding)
            {
                slide = false;
                rb2.drag = -1f;
                slideRot();
                isSliding = true;
                bc2.enabled = false;
                slideCollider.enabled = true;
                rb2.velocity = new Vector2(0, rb2.velocity.y);
                if (isFacingRight)
                {
                    rb2.AddForce(Vector2.right * slideSpeed);
                }
                else
                {
                    rb2.AddForce(Vector2.left * slideSpeed);
                }
                StartCoroutine("stopSlide");
            }
        }

        IEnumerator stopSlide()
        {
            float counter = 0;

            while (counter < slideTime || onIce)
            {
                //Increment Timer until counter >= waitTime
                counter += Time.deltaTime;
                // Action to break slide
                if (jump)
                {
                    //Quit wait time
                    break;
                    //Following stops stopSlide from running
                    //yield break;
                }
                //Wait for a frame so that Unity doesn't freeze
                yield return null;
            }
            slideTimer = slideCooldown;
            bc2.enabled = true;
            slideCollider.enabled = false;
            isSliding = false;
            sprite.transform.rotation = new Quaternion(0, 0, 0, 0);
        }


        /**
         * Returns true if player is touching ground
         */
        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        }

        /**
         * Returns true if player is touching a wall
         */
        private bool IsWalled()
        {
            return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        }

        /**
         * Dash key
         */
        public void Dash(InputAction.CallbackContext context)
        {
            dash = context.performed;
        }

        /**
         * Performs a dash, if requirements met
         */
        private void DashCall()
        {
            if (dash && canDash)
            {
                //dash sound effect
                dashSound.Play();
                dashSound.pitch = Random.Range(1 - pitchChangeMult, 1 + pitchChangeMult);
            StartCoroutine("DashCode");
            }
        }
        /**
         * Dash Code
         */
        IEnumerator DashCode()
        {
            canDash = false;
            isDashing = true;
            rb2.drag = 3;
            if (!IsGrounded())
            {
                rb2.drag = 0;
                rb2.gravityScale = 0;
                rb2.angularDrag = 0;
            }
            rb2.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
            trail.emitting = true;
            yield return new WaitForSeconds(dashTime);
            rb2.gravityScale = playerGravity;
            trail.emitting = false;
            rb2.drag = drag;
            if (IsGrounded())
            {
                yield return new WaitForSeconds(dashCooldown);
            }
            isDashing = false;
            //canDash = true;
        }

        /**
         * Makes the character face left/right
         */
        private void Flip()
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        // Called every few frames, used for Physics
        private void FixedUpdate()
        {
            if (movable != null && onMovable && horizontal == 0 && IsGrounded())
            {
                rb2.velocity = movable.velocity;
            }
            MoveCode();
            JumpCode();
            WallSlide();
            WallJump();
            GroundSlide();
            DashCall();

            if (!isDashing)
            {
                rb2.AddForce(new Vector2(0f, -1 * rb2.gravityScale) * rb2.mass);
            }

            if (isClimbing)
            {
                rb2.gravityScale = 0;
                rb2.velocity = new Vector2(rb2.velocity.x, vertical * ladderSpeed);
            }
            else if (!isDashing)
            {
                rb2.gravityScale = playerGravity;
            }

        //footstep sound effect
            if (sounds.Length > 0)
            {
                if (rb2.velocity.x != 0 && IsGrounded() && step < 0)
                {

                    source.clip = sounds[Random.Range(1, sounds.Length)];
                    source.volume = Random.Range(0.8f - volumeChangeMult, 0.8f);
                    source.pitch = Random.Range(1 - pitchChangeMult, 1 + pitchChangeMult);
                    source.PlayOneShot(source.clip);
                    step = stepFrames;
                }
                else if (rb2.velocity.x != 0 && IsGrounded() && step >= 0)
                {
                    step = step - 1;
                }
            }
    }

        // Update is called once per frame
        void Update()
        {
            if (isDashing)
            {
                return;
            }
            if (onIce && IsGrounded())
            {
                rb2.velocity = new Vector2(iceVelocity, rb2.velocity.y);
            }
            // Max speed clamping
            if (rb2.velocity.x > maxSpeed)
            {
                rb2.velocity = new Vector2(maxSpeed, rb2.velocity.y);
            }
            else if (rb2.velocity.x < maxSpeed * -1f)
            {
                rb2.velocity = new Vector2(maxSpeed * -1, rb2.velocity.y);
            }

            if (rb2.velocity.y < maxFallSpeed * -1 && !IsGrounded())
            {
                rb2.velocity = new Vector2(rb2.velocity.x, maxFallSpeed * -1);
            }

            
            
 

        slideTimer -= Time.deltaTime;
            if (slideTimer < 0f)
            {
                slideTimer = 0f;
            }

            if (!isSliding)
            {
                rb2.drag = drag;
            }
            /*
             * Flip player, give them a turn boost, otherwise bleed boost
             */
            if (!isWallJumping && !isSliding)
            {
                if (!isFacingRight && horizontal > 0f)
                {
                    Flip();
                    //Experiment, replace turnboost with a variable turnBoost
                    accelBoost = turnBoost;
                }
                else if (isFacingRight && horizontal < 0f)
                {
                    Flip();
                    accelBoost = turnBoost;
                }
            }
            else
            {
                if (accelBoost > 1)
                {
                    accelBoost -= 0.75f * Time.deltaTime;
                }
                else if (accelBoost < 1)
                {
                    accelBoost = 1;
                }
            }

            JumpFeel();

            /*
             * Attributes to reset when on the ground
             */
            if (IsGrounded())
            {
                coyoteTimer = coyoteTime;
                accelBoost = coldStartRunBoost;
                if (!isDashing)
                {
                    canDash = true;
                }
            }
            else
            {
                coyoteTimer -= Time.deltaTime;
                if (coyoteTimer < 0f)
                {
                    coyoteTimer = 0f;
                }
            }
        }
    }
