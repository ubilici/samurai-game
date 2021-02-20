using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetButton : MonoBehaviour
{
    [SerializeField] private Image Background;
    [SerializeField] private TextMeshProUGUI ButtonText;

    private const float SingleKeyFontSize = 60;
    private const float MultipleKeyFontSize = 35;
    private const float FadeInTime = 0.4f;

    private Sequence _fadeSequence;

    public void Initialize(KeyCode key)
    {
        SetAlpha(Background, 0);
        SetAlpha(ButtonText, 0);

        var text = key.ToString();
        if (text.Length == 1)
        {
            ButtonText.fontSize = SingleKeyFontSize;
        }
        else
        {
            ButtonText.fontSize = MultipleKeyFontSize;

            if (text.Contains("Arrow"))
            {
                text = text.Substring(0, text.Length - 5);
            }
        }

        ButtonText.text = text;
    }

    public void FadeIn()
    {
        _fadeSequence = DOTween.Sequence();
        _fadeSequence.Append(Background.DOFade(1, FadeInTime));
        _fadeSequence.Join(ButtonText.DOFade(1, FadeInTime));
    }

    public void FadeOut()
    {
        _fadeSequence = DOTween.Sequence();
        _fadeSequence.Append(Background.DOFade(0, GameSettings.Instance.ButtonFadeOutTime));
        _fadeSequence.Join(ButtonText.DOFade(0, GameSettings.Instance.ButtonFadeOutTime));
    }

    private static void SetAlpha(Graphic image, float alpha)
    {
        var color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public void Destroy()
    {
        _fadeSequence?.Kill();
        Destroy(gameObject);
    }
}