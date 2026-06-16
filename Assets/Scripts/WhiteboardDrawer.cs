using UnityEngine;
using UnityEngine.XR;

public class WhiteboardDrawer : MonoBehaviour
{
    public float rayLength = 5f;

    void Update()
    {
        InputDevice controller =
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        bool triggerPressed = false;

        controller.TryGetFeatureValue(
            CommonUsages.triggerButton,
            out triggerPressed
        );

        if (!triggerPressed)
            return;

        RaycastHit hit;

        if (Physics.Raycast(
            transform.position,
            transform.forward,
            out hit,
            rayLength))
        {
            WhiteboardManager wb =
                hit.collider.GetComponent<WhiteboardManager>();

            if (wb != null)
            {
                wb.Draw(hit.textureCoord);
            }
        }
    }
}