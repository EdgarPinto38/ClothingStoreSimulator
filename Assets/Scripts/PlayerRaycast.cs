using UnityEngine;
using TMPro;

public class PlayerRaycast : MonoBehaviour
{
    public float rayDistance = 2f;
    public LayerMask interactableLayer;
    public TextMeshProUGUI objectNameText;

    public SpriteRenderer headRenderer;
    public SpriteRenderer bodyRenderer;
    public Animator headAnimator;
    public Animator bodyAnimator;

    private Vector2 lastDirection = Vector2.down;
    private bool isMoving = false;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            isMoving = true;

            if (moveX > 0)
            {
                lastDirection = Vector2.right;
                headRenderer.flipX = false;
                bodyRenderer.flipX = false;
                SetAnimation("Horizontal");
            }
            else if (moveX < 0)
            {
                lastDirection = Vector2.left;
                headRenderer.flipX = true;
                bodyRenderer.flipX = true;
                SetAnimation("Horizontal");
            }
            else if (moveY > 0)
            {
                lastDirection = Vector2.up;
                SetAnimation("Up");
            }
            else if (moveY < 0)
            {
                lastDirection = Vector2.down;
                SetAnimation("Down");
            }
        }
        else
        {
            isMoving = false;
            PlayIdleAnimation();
        }

        // Disparar Raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastDirection, rayDistance, interactableLayer);
        if (hit.collider != null)
        {
            objectNameText.text = hit.collider.gameObject.name;
            objectNameText.color = Color.yellow;
        }
        else
        {
            objectNameText.text = "";
        }

        Debug.DrawRay(transform.position, lastDirection * rayDistance, Color.red);
    }

    void SetAnimation(string direction)
    {
        headAnimator.ResetTrigger("IdleHorizontal");
        headAnimator.ResetTrigger("IdleUp");
        headAnimator.ResetTrigger("IdleDown");

        bodyAnimator.ResetTrigger("IdleHorizontal");
        bodyAnimator.ResetTrigger("IdleUp");
        bodyAnimator.ResetTrigger("IdleDown");

        headAnimator.SetTrigger($"Head{direction}");
        bodyAnimator.SetTrigger($"Body{direction}");
    }

    void PlayIdleAnimation()
    {
        string idleTrigger = lastDirection == Vector2.right || lastDirection == Vector2.left ? "IdleHorizontal"
                         : lastDirection == Vector2.up ? "IdleUp"
                         : "IdleDown";

        headAnimator.SetTrigger(idleTrigger);
        bodyAnimator.SetTrigger(idleTrigger);
    }
}