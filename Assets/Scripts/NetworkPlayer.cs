using UnityEngine;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviourPun, IPunObservable
{
    [Header("Local XR Rig References (leave empty, auto-found)")]
    public Transform headTarget;
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    [Header("Avatar Body Parts")]
    public Transform headBone;
    public Transform leftHandBone;
    public Transform rightHandBone;

    [Header("Floor")]
    public float floorY = 0.1f;

    private Animator anim;

    private Vector3 netHeadPos, netLeftPos, netRightPos;
    private Quaternion netHeadRot, netLeftRot, netRightRot;
    private Vector3 netRootPos;
    private Quaternion netRootRot;
    private float netSpeed;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (photonView.IsMine)
        {
            Camera xrCam = Camera.main;
            if (xrCam != null) headTarget = xrCam.transform;

            foreach (Transform t in FindObjectsByType<Transform>(FindObjectsSortMode.None))
            {
                if (t.name == "Left Controller")  leftHandTarget  = t;
                if (t.name == "Right Controller") rightHandTarget = t;
            }

            Debug.Log("[NetworkPlayer] headTarget: " + headTarget +
                      " | leftHandTarget: " + leftHandTarget +
                      " | rightHandTarget: " + rightHandTarget);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Vector3 prevPos = transform.position;

            // Move avatar root with headset, locked to floor height
            if (headTarget != null)
            {
                Vector3 pos = headTarget.position;
                pos.y = floorY;
                transform.position = pos;

                Vector3 fwd = headTarget.forward;
                fwd.y = 0;
                if (fwd != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(fwd);
            }

            // Animate based on actual movement
            float moveDist = Vector3.Distance(transform.position, prevPos);
            float speed = (moveDist / Time.deltaTime) > 0.1f ? 1f : 0f;
            if (anim != null) anim.SetFloat("Speed", speed);

            // Drive bones from controllers
            if (headBone != null && headTarget != null)
            {
                headBone.position = headTarget.position;
                headBone.rotation = headTarget.rotation;
            }
            if (leftHandBone != null && leftHandTarget != null)
            {
                leftHandBone.position = leftHandTarget.position;
                leftHandBone.rotation = leftHandTarget.rotation;
            }
            if (rightHandBone != null && rightHandTarget != null)
            {
                rightHandBone.position = rightHandTarget.position;
                rightHandBone.rotation = rightHandTarget.rotation;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, netRootPos, Time.deltaTime * 15);
            transform.rotation = Quaternion.Lerp(transform.rotation, netRootRot, Time.deltaTime * 15);

            if (anim != null) anim.SetFloat("Speed", netSpeed);

            if (headBone != null)
            {
                headBone.position = Vector3.Lerp(headBone.position, netHeadPos, Time.deltaTime * 15);
                headBone.rotation = Quaternion.Lerp(headBone.rotation, netHeadRot, Time.deltaTime * 15);
            }
            if (leftHandBone != null)
            {
                leftHandBone.position = Vector3.Lerp(leftHandBone.position, netLeftPos, Time.deltaTime * 15);
                leftHandBone.rotation = Quaternion.Lerp(leftHandBone.rotation, netLeftRot, Time.deltaTime * 15);
            }
            if (rightHandBone != null)
            {
                rightHandBone.position = Vector3.Lerp(rightHandBone.position, netRightPos, Time.deltaTime * 15);
                rightHandBone.rotation = Quaternion.Lerp(rightHandBone.rotation, netRightRot, Time.deltaTime * 15);
            }
        }
    }
    void LateUpdate()
{
    if (photonView.IsMine)
    {
        if (headBone != null && headTarget != null)
        {
            headBone.position = headTarget.position;
            headBone.rotation = headTarget.rotation;
        }
        if (leftHandBone != null && leftHandTarget != null)
        {
            leftHandBone.position = leftHandTarget.position;
            leftHandBone.rotation = leftHandTarget.rotation;
        }
        if (rightHandBone != null && rightHandTarget != null)
        {
            rightHandBone.position = rightHandTarget.position;
            rightHandBone.rotation = rightHandTarget.rotation;
        }
    }
}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(anim != null ? anim.GetFloat("Speed") : 0f);

            stream.SendNext(headBone != null ? headBone.position : Vector3.zero);
            stream.SendNext(headBone != null ? headBone.rotation : Quaternion.identity);
            stream.SendNext(leftHandBone != null ? leftHandBone.position : Vector3.zero);
            stream.SendNext(leftHandBone != null ? leftHandBone.rotation : Quaternion.identity);
            stream.SendNext(rightHandBone != null ? rightHandBone.position : Vector3.zero);
            stream.SendNext(rightHandBone != null ? rightHandBone.rotation : Quaternion.identity);
        }
        else
        {
            netRootPos = (Vector3)stream.ReceiveNext();
            netRootRot = (Quaternion)stream.ReceiveNext();
            netSpeed   = (float)stream.ReceiveNext();

            netHeadPos  = (Vector3)stream.ReceiveNext();  netHeadRot  = (Quaternion)stream.ReceiveNext();
            netLeftPos  = (Vector3)stream.ReceiveNext();  netLeftRot  = (Quaternion)stream.ReceiveNext();
            netRightPos = (Vector3)stream.ReceiveNext();  netRightRot = (Quaternion)stream.ReceiveNext();
        }
    }
}