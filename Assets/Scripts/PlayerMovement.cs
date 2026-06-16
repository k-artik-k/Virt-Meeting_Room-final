using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviourPun
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float rotateSpeed = 90f;

    [Header("References")]
    public Animator animator;

    private CharacterController cc;

    void Start()
    {
        if (!photonView.IsMine) return;
        cc = GetComponent<CharacterController>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (cc != null && cc.enabled && transform.position.y < -0.1f)
            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);

        HandleMovement();
    }

    void HandleMovement()
    {
        if (cc == null || !cc.enabled) return;

        float h = Input.GetKey(KeyCode.D) ? 1f : Input.GetKey(KeyCode.A) ? -1f : 0f;
        float v = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;

        Vector3 move = transform.forward * v + transform.right * h;
        move.y = cc.isGrounded ? 0f : -9.8f * Time.deltaTime;
        cc.Move(move * moveSpeed * Time.deltaTime);

        float rotate = 0f;
        if (Input.GetKey(KeyCode.Q)) rotate = -1f;
        if (Input.GetKey(KeyCode.E)) rotate = 1f;
        transform.Rotate(0, rotate * rotateSpeed * Time.deltaTime, 0);

        animator.SetFloat("Speed", new Vector3(h, 0, v).magnitude);
    }
}