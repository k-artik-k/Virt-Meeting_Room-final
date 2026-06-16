using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;
using System.Collections;

public class ChairSit : MonoBehaviour
{
    [Header("Chair Settings")]
    public Transform sitPoint;
    public float sitRange = 1.5f;

    [Header("UI")]
    public GameObject promptUI;

    private Transform player;
    private CharacterController controller;
    private PlayerMovement movement;
    private Animator avatarAnimator;

    private bool isSitting = false;
    private bool aButtonPrev = false;

    void Start()
    {
        if (promptUI != null) promptUI.SetActive(false);
    }

    void Update()
    {
        if (player == null)
        {
            foreach (var pm in FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None))
            {
                if (pm.photonView.IsMine)
                {
                    player = pm.transform;
                    controller = player.GetComponent<CharacterController>();
                    movement = pm;
                    avatarAnimator = player.GetComponentInChildren<Animator>();
                    break;
                }
            }
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);
        bool inRange = dist < sitRange;

        bool aPressed = false;
        InputDevice right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        right.TryGetFeatureValue(CommonUsages.primaryButton, out aPressed);
        bool aDown = aPressed && !aButtonPrev;
        aButtonPrev = aPressed;

        bool triggered = aDown;

        if (!isSitting)
        {
            if (promptUI != null) promptUI.SetActive(inRange);
            if (inRange && triggered) Sit();
        }
        else
        {
            if (promptUI != null) promptUI.SetActive(false);

            Vector2 joystick = Vector2.zero;
            InputDevice left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            left.TryGetFeatureValue(CommonUsages.primary2DAxis, out joystick);

            bool shouldStand = triggered || joystick.magnitude > 0.3f ||
                               Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                               Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

            if (shouldStand) StandUp();
        }

        if (promptUI != null && promptUI.activeSelf)
        {
            Vector3 dir = promptUI.transform.position - player.position;
            dir.y = 0;
            if (dir != Vector3.zero)
                promptUI.transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    void Sit()
    {
        isSitting = true;
        if (promptUI != null) promptUI.SetActive(false);
        if (controller != null) controller.enabled = false;
        if (movement != null) movement.enabled = false;

        player.position = sitPoint.position;
        player.rotation = sitPoint.rotation;

        if (avatarAnimator != null)
        {
            avatarAnimator.SetBool("IsSitting", true);
            avatarAnimator.SetFloat("Speed", 0f);
        }
    }

    void StandUp()
    {
        isSitting = false;
        if (avatarAnimator != null)
            avatarAnimator.SetBool("IsSitting", false);
        if (movement != null) movement.enabled = true;
        StartCoroutine(ReenableController());
    }
IEnumerator ReenableController()
{
    yield return new WaitForSeconds(0.3f);
    Debug.Log("[ChairSit] Player Y before snap: " + player.position.y);
    player.position = new Vector3(player.position.x, 0.1f, player.position.z);
    Debug.Log("[ChairSit] Player Y after snap: " + player.position.y);
    if (controller != null) controller.enabled = true;
    Debug.Log("[ChairSit] Controller re-enabled: " + controller.enabled);
}
}