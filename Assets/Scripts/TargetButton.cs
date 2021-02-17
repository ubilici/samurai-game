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

    private Sequence _fadeOutSequence;

    public void Initialize(KeyCode key)
    {
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

    public void FadeOut()
    {
        _fadeOutSequence = DOTween.Sequence();
        _fadeOutSequence.Append(Background.DOFade(0, GameSettings.Instance.ButtonFadeOutTime));
        _fadeOutSequence.Join(ButtonText.DOFade(0, GameSettings.Instance.ButtonFadeOutTime));
    }

    public void Destroy()
    {
        _fadeOutSequence?.Kill();
        Destroy(gameObject);
    }
}