using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bow : MonoBehaviour
{
    [Header("Who to read input from")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Local-space offsets for each facing")]
    [Tooltip("Position when player is looking right (Z-rot = 0)")]
    [SerializeField] private Vector2 rightOffset;
    [Tooltip("Position when player is looking up (Z-rot =  90)")]
    [SerializeField] private Vector2   upOffset;
    [Tooltip("Position when player is looking left (Z-rot = 180)")]
    [SerializeField] private Vector2 leftOffset;
    [Tooltip("Position when player is looking down (Z-rot = –90)")]
    [SerializeField] private Vector2 downOffset;

    private void Update()
    {
        Vector2 dir = playerMovement.LastMovement;
        if (dir == Vector2.zero) return;

        // decide which way they’re “aiming”
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            // horizontal
            if (dir.x > 0f)
                ApplyFacing(0f, rightOffset);
            else
                ApplyFacing(180f, leftOffset);
        }
        else
        {
            // vertical
            if (dir.y > 0f)
                ApplyFacing(90f, upOffset);
            else
                ApplyFacing(-90f, downOffset);
        }
    }

    private void ApplyFacing(float zAngle, Vector2 offset)
    {
        transform.localEulerAngles   = new Vector3(0f, 0f, zAngle);
        transform.localPosition      = new Vector3(offset.x, offset.y, transform.localPosition.z);
    }
}
