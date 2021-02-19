using DG.Tweening;
using UnityEngine;

public class SamuraiController : MonoBehaviour
{
    [SerializeField] private Animator AnimatorController;

    private Sequence _runSequence;
    private Sequence _jumpSequence;

    private const float RunSpeed = 0.2f;
    private const float RotateDuration = 0.2f;
    private const float RandomRadius = 4f;
    private const float RandomWaitTime = 1.75f;

    private static readonly int IdleTrigger = Animator.StringToHash("Idle");
    private static readonly int RunTrigger = Animator.StringToHash("Run");
    private static readonly int DrawTrigger = Animator.StringToHash("Draw");
    private static readonly int JumpTrigger = Animator.StringToHash("Jump");

    public void RunToRandomPosition()
    {
        var randomX = Random.Range(-RandomRadius, RandomRadius);
        var randomZ = Random.Range(-RandomRadius, RandomRadius);
        Run(new Vector3(randomX, 0, randomZ), RandomWaitTime);
    }

    public void Run(Vector3 target, float waitTime)
    {
        var currentPosition = transform.position;
        var distance = Vector3.Distance(target, currentPosition);
        target.y = currentPosition.y;

        _runSequence?.Kill();
        _runSequence = DOTween.Sequence();
        _runSequence.SetDelay(waitTime);

        _runSequence.AppendCallback(() => { AnimatorController.SetTrigger(RunTrigger); });
        _runSequence.Join(transform.DOLookAt(target, RotateDuration));
        _runSequence.Join(transform.DOMove(target, distance * RunSpeed));

        _runSequence.onComplete = () =>
        {
            AnimatorController.SetTrigger(IdleTrigger);
            RunToRandomPosition();
        };
    }
}