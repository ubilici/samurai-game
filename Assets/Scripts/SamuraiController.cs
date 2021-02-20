using DG.Tweening;
using UnityEngine;

public class SamuraiController : MonoBehaviour
{
    [SerializeField] private Animator AnimatorController;

    private Sequence _runSequence;
    private Sequence _jumpSequence;

    private const float RunSpeed = 0.2f;
    private const float JumpPower = 1.5f;
    private const float JumpDuration = 0.5f;
    private const float RotateDuration = 0.2f;

    private static readonly int IdleTrigger = Animator.StringToHash("Idle");
    private static readonly int RunTrigger = Animator.StringToHash("Run");
    private static readonly int DrawTrigger = Animator.StringToHash("Draw");
    private static readonly int JumpTrigger = Animator.StringToHash("Jump");

    public void Run(Vector3 target)
    {
        var current = transform.position;
        var distance = Vector3.Distance(target, current);
        target.y = current.y;

        _runSequence?.Kill();
        _runSequence = DOTween.Sequence();

        _runSequence.AppendCallback(() => { SetTrigger(RunTrigger); });
        _runSequence.Join(transform.DOLookAt(target, RotateDuration));
        _runSequence.Join(transform.DOMove(target, distance * RunSpeed));

        _runSequence.onComplete = () => { SetTrigger(IdleTrigger); };
        _runSequence.onKill = () => { SetTrigger(IdleTrigger); };
    }

    public void Jump(Vector3 target)
    {
        var current = transform.position;
        var lookTarget = current * 2 - target;
        lookTarget.y = current.y;
        target.y = current.y;

        _jumpSequence?.Kill();
        _jumpSequence = DOTween.Sequence();

        _jumpSequence.AppendCallback(() => { SetTrigger(JumpTrigger); });
        _jumpSequence.Join(transform.DOLookAt(lookTarget, RotateDuration));
        _runSequence.Join(transform.DOJump(target, JumpPower, 1, JumpDuration));

        _jumpSequence.onComplete = () => { SetTrigger(IdleTrigger); };
        _jumpSequence.onKill = () => { SetTrigger(IdleTrigger); };
    }

    private void SetTrigger(int trigger)
    {
        AnimatorController.SetTrigger(trigger);
    }
}