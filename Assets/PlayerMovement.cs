using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
    public Animator animator;
    public Rigidbody2D playerRigidbody;

	public float runSpeed = 40f;
	float horizontalMove = 0f;

	bool jump = false;
    bool jumpLeft = true;
    float jumpCooldown = 3f;
	bool crouch = false;
    bool respawn;
    Vector2 spawnPosition = new Vector2(-1.92f, 1f);

    private void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Respawn(Rigidbody2D player) {
        player.position = spawnPosition;
        animator.SetBool("isJumping", false);
        animator.SetBool("isCrouching", false);
    }

    void Update () {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Respawn")) {
            respawn = true;
        }

        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }
        if (controller.onGround() && jump == true)
        {
            animator.SetBool("isJumping", true);
        }
        
        if (controller.onGround())
        {
            jumpLeft = true;
            jumpCooldown = 3f;
        } else {
            jumpCooldown--;
        }

		if (Input.GetButtonDown("Crouch")) {
			crouch = true;
            animator.SetBool("isCrouching", true);
		} else if (Input.GetButtonUp("Crouch")) {
			crouch = false;
            animator.SetBool("isCrouching", false);
		}
	}

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

	void FixedUpdate () {
        // Move the character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        if (jump && jumpLeft && jumpCooldown < 0)
        {
            controller.Jump(700f);
            jumpLeft = false;
            jump = false;
        }

        if (respawn) {
            Respawn(playerRigidbody);
        }

        if (controller.onGround())
        {
            OnLanding();
        }

        jump = false;
        respawn = false;
	}
}
