using UnityEngine;
using DG.Tweening;

public class Gem : MonoBehaviour
{
    [SerializeField][Tooltip("Lower number = Faster")] private float m_rotationSpeed = 10f;
    [SerializeField] private GameObject m_body;

    private Tween rotateTween;

    private void Start()
    {
        rotateTween =  m_body.transform.DORotate(new Vector3(0, 360, 0),
            m_rotationSpeed, 
            RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear
            );
    }

    private void OnDestroy()
    {
        rotateTween.Kill();
    }

}
