using FishNet.Object;
using System;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public Transform playerCam;
    private CharacterController controller;
    private float verticalVelocity;
    public bool groundedPlayer;
    private float playerSpeed = 5.0f;
    private float jumpHeight = 2.0f;
    private float gravityValue = -15.81f;
    private float groundedTimer;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        playerCam = transform.Find("Camera").transform;
    }


    private void Update()
    {
        if (!IsOwner) return;

        bool groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            // cooldown interval to allow reliable jumping even whem coming down ramps
            groundedTimer = 0.2f;
        }
        if (groundedTimer > 0)
        {
            groundedTimer -= Time.deltaTime;
        }

        // slam into the ground
        if (groundedPlayer && verticalVelocity < 0)
        {
            // hit ground
            verticalVelocity = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        var camDir = -playerCam.position + transform.position;
        var lookRotation = Quaternion.LookRotation(camDir, Vector3.up);

        move = lookRotation * move;
        move.y = 0;
        move = move.normalized;

        move *= playerSpeed;

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        verticalVelocity += gravityValue * Time.deltaTime;

        // allow jump as long as the player is on the ground
        if (Input.GetButtonDown("Jump"))
        {
            // must have been grounded recently to allow jump
            if (groundedTimer > 0)
            {
                // no more until we recontact ground
                groundedTimer = 0;

                // Physics dynamics formula for calculating jump up velocity based on height and gravity
                verticalVelocity += Mathf.Sqrt(jumpHeight * 2 * -gravityValue);
            }
        }

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
        //groundedPlayer = controller.isGrounded;
    }
}
