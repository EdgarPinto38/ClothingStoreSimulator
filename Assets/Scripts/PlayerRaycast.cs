using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    public SpriteRenderer headRenderer;
    public SpriteRenderer bodyRenderer;
    public Animator headAnimator;
    public Animator bodyAnimator;

    private Vector2 lastDirection = Vector2.down;
    private bool isMoving = false;

    private enum SkinColor { White, Yellow, Black, Azul, Rojo, Rosa, Verde }
    private SkinColor currentHeadSkin = SkinColor.White;
    private SkinColor currentBodySkin = SkinColor.White;

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
    }

    void SetWalkAnimation()
    {
        ResetTriggers();
        ActivateHeadTrigger("WalkRight", "WalkUp", "WalkDown");
        ActivateBodyTrigger("WalkRight", "WalkUp", "WalkDown");
    }

    void PlayIdleAnimation()
    {
        ResetTriggers();
        ActivateHeadTrigger("IdleRight", "IdleUp", "IdleDown");
        ActivateBodyTrigger("IdleRight", "IdleUp", "IdleDown");
    }

    void ActivateHeadTrigger(string rightTrigger, string upTrigger, string downTrigger)
    {
        string headState = GetStateTrigger(rightTrigger, upTrigger, downTrigger, currentHeadSkin);
        if (!string.IsNullOrEmpty(headState))
        {
            headAnimator.SetTrigger(headState);
        }
    }

    void ActivateBodyTrigger(string rightTrigger, string upTrigger, string downTrigger)
    {
        string bodyState = GetStateTrigger(rightTrigger, upTrigger, downTrigger, currentBodySkin);
        if (!string.IsNullOrEmpty(bodyState))
        {
            bodyAnimator.SetTrigger(bodyState);
        }
    }

    string GetStateTrigger(string rightTrigger, string upTrigger, string downTrigger, SkinColor skin)
    {
        if (lastDirection == Vector2.right || lastDirection == Vector2.left)
        {
            return $"{rightTrigger}_{skin}";
        }
        else if (lastDirection == Vector2.up)
        {
            return $"{upTrigger}_{skin}";
        }
        else
        {
            return $"{downTrigger}_{skin}";
        }
    }

    void ResetTriggers()
    {
        string[] colors = { "White", "Yellow", "Black", "Azul", "Rojo", "Rosa", "Verde" };
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

    public void ChangeHeadSkin(int skinIndex)
    {
        currentHeadSkin = (SkinColor)skinIndex;
    }

    public void ChangeBodySkin(int skinIndex)
    {
        currentBodySkin = (SkinColor)skinIndex;
    }

    public Vector2 GetLastDirection()
    {
        return lastDirection;
    }

}