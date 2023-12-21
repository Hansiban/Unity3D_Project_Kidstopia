using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerControll : NetworkBehaviour
{
    [SerializeField] private FixedJoystick joystick;

    // Network Component
    public NetworkRigidbodyReliable rigid_net;
    public NetworkTransformReliable trans_net;
    private NetworkAnimator ani_net;

    public Rigidbody playerRigid;
    private Transform playerTrans;

    [SerializeField] private float moveSpeed = 3f;
    public float jumpForce = 3f;
    public bool isGround;

    private void OnEnable()
    {
        joystick = FindObjectOfType<FixedJoystick>();

        TryGetComponent(out rigid_net);
        TryGetComponent(out trans_net);
        TryGetComponent(out ani_net);

        playerRigid = rigid_net.target.GetComponent<Rigidbody>();
        playerTrans = trans_net.target.transform;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        playerRigid.velocity = new Vector3(joystick.Horizontal * moveSpeed, playerRigid.velocity.y, joystick.Vertical * moveSpeed);

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            playerTrans.rotation = Quaternion.LookRotation(new Vector3(playerRigid.velocity.x, 0f, playerRigid.velocity.z)); // jump ���� �� ������ �Ѿ����� �ʰ� ����
            ani_net.animator.SetBool("isWalk", true);
        }
        else
        {
            ani_net.animator.SetBool("isWalk", false);
        }
    }

    public void PlayerJump()
    {
        if (!isLocalPlayer) return;
        if (isGround)
        {
            playerRigid.AddForce(new Vector3(0f, 2f, 0f) * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGround = false;
    }
}
