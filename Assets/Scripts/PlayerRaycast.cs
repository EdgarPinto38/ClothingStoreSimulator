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

    // Identificación del color actual
    private enum SkinColor { White, Yellow, Black }
    private SkinColor currentSkin = SkinColor.White;

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
                SetWalkAnimation();
            }
            else if (moveX < 0)
            {
                lastDirection = Vector2.left;
                headRenderer.flipX = true;
                bodyRenderer.flipX = true;
                SetWalkAnimation();
            }
            else if (moveY > 0)
            {
                lastDirection = Vector2.up;
                SetWalkAnimation();
            }
            else if (moveY < 0)
            {
                lastDirection = Vector2.down;
                SetWalkAnimation();
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

    void SetWalkAnimation()
    {
        ResetTriggers();

        switch (currentSkin)
        {
            case SkinColor.White:
                if (lastDirection == Vector2.right || lastDirection == Vector2.left)
                {
                    headAnimator.SetTrigger("WalkRight_White");
                    bodyAnimator.SetTrigger("WalkRight_White");
                }
                else if (lastDirection == Vector2.up)
                {
                    headAnimator.SetTrigger("WalkUp_White");
                    bodyAnimator.SetTrigger("WalkUp_White");
                }
                else
                {
                    headAnimator.SetTrigger("WalkDown_White");
                    bodyAnimator.SetTrigger("WalkDown_White");
                }
                break;

            case SkinColor.Yellow:
                if (lastDirection == Vector2.right || lastDirection == Vector2.left)
                {
                    headAnimator.SetTrigger("WalkRight_Yellow");
                    bodyAnimator.SetTrigger("WalkRight_Yellow");
                }
                else if (lastDirection == Vector2.up)
                {
                    headAnimator.SetTrigger("WalkUp_Yellow");
                    bodyAnimator.SetTrigger("WalkUp_Yellow");
                }
                else
                {
                    headAnimator.SetTrigger("WalkDown_Yellow");
                    bodyAnimator.SetTrigger("WalkDown_Yellow");
                }
                break;

            case SkinColor.Black:
                if (lastDirection == Vector2.right || lastDirection == Vector2.left)
                {
                    headAnimator.SetTrigger("WalkRight_Black");
                    bodyAnimator.SetTrigger("WalkRight_Black");
                }
                else if (lastDirection == Vector2.up)
                {
                    headAnimator.SetTrigger("WalkUp_Black");
                    bodyAnimator.SetTrigger("WalkUp_Black");
                }
                else
                {
                    headAnimator.SetTrigger("WalkDown_Black");
                    bodyAnimator.SetTrigger("WalkDown_Black");
                }
                break;
        }
    }

    void PlayIdleAnimation()
    {
        ResetTriggers();

        switch (currentSkin)
        {
            case SkinColor.White:
                if (lastDirection == Vector2.right || lastDirection == Vector2.left)
                {
                    headAnimator.SetTrigger("IdleRight_White");
                    bodyAnimator.SetTrigger("IdleRight_White");
                }
                else if (lastDirection == Vector2.up)
                {
                    headAnimator.SetTrigger("IdleUp_White");
                    bodyAnimator.SetTrigger("IdleUp_White");
                }
                else
                {
                    headAnimator.SetTrigger("IdleDown_White");
                    bodyAnimator.SetTrigger("IdleDown_White");
                }
                break;

            case SkinColor.Yellow:
                if (lastDirection == Vector2.right || lastDirection == Vector2.left)
                {
                    headAnimator.SetTrigger("IdleRight_Yellow");
                    bodyAnimator.SetTrigger("IdleRight_Yellow");
                }
                else if (lastDirection == Vector2.up)
                {
                    headAnimator.SetTrigger("IdleUp_Yellow");
                    bodyAnimator.SetTrigger("IdleUp_Yellow");
                }
                else
                {
                    headAnimator.SetTrigger("IdleDown_Yellow");
                    bodyAnimator.SetTrigger("IdleDown_Yellow");
                }
                break;

            case SkinColor.Black:
                if (lastDirection == Vector2.right || lastDirection == Vector2.left)
                {
                    headAnimator.SetTrigger("IdleRight_Black");
                    bodyAnimator.SetTrigger("IdleRight_Black");
                }
                else if (lastDirection == Vector2.up)
                {
                    headAnimator.SetTrigger("IdleUp_Black");
                    bodyAnimator.SetTrigger("IdleUp_Black");
                }
                else
                {
                    headAnimator.SetTrigger("IdleDown_Black");
                    bodyAnimator.SetTrigger("IdleDown_Black");
                }
                break;
        }
    }

    void ResetTriggers()
    {
        string[] colors = { "White", "Yellow", "Black" };
        string[] states = { "WalkRight", "WalkUp", "WalkDown", "IdleRight", "IdleUp", "IdleDown" };

        foreach (string color in colors)
        {
            foreach (string state in states)
            {
                headAnimator.ResetTrigger($"{state}_{color}");
                bodyAnimator.ResetTrigger($"{state}_{color}");
            }
        }
    }

    public void ChangeSkin(int skinIndex)
    {
        switch (skinIndex)
        {
            case 0:
                currentSkin = SkinColor.White;
                break;
            case 1:
                currentSkin = SkinColor.Yellow;
                break;
            case 2:
                currentSkin = SkinColor.Black;
                break;
        }
    }
}