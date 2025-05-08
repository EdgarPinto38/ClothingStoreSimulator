using UnityEngine;

public class HeadSkinManager : MonoBehaviour
{
    public SpriteRenderer headRenderer;
    public Animator headAnimator;

    // Animators por color
    public RuntimeAnimatorController whiteHeadAnimator;
    public RuntimeAnimatorController yellowHeadAnimator;
    public RuntimeAnimatorController blackHeadAnimator;

    // Sprites por color
    public Sprite whiteHeadSprite;
    public Sprite yellowHeadSprite;
    public Sprite blackHeadSprite;

    public void ChangeHeadSkin(int skinIndex)
    {
        switch (skinIndex)
        {
            case 0:
                headRenderer.sprite = whiteHeadSprite;
                headAnimator.runtimeAnimatorController = whiteHeadAnimator;
                break;
            case 1:
                headRenderer.sprite = yellowHeadSprite;
                headAnimator.runtimeAnimatorController = yellowHeadAnimator;
                break;
            case 2:
                headRenderer.sprite = blackHeadSprite;
                headAnimator.runtimeAnimatorController = blackHeadAnimator;
                break;
        }
    }
}