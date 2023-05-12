using DG.Tweening;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action OnGameLost;
    [SerializeField] private float m_patrolTime = 2;
    [SerializeField] private float m_patrolSpeed = 1f;
    [SerializeField] private GameObject m_body;

    private Vector3 m_negativeScale = new Vector3(-1, 1, -1);
    private Vector3 m_positiveScale = new Vector3(1, 1, 1);

    private Sequence sequence;

    private void Start()
    {
        InitPatrol();
    }

    private void InitPatrol()
    {
        if (transform == null || m_body == null) { return; }
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveX(transform.position.x + m_patrolSpeed, m_patrolTime).SetEase(Ease.Linear));
        sequence.Join(m_body.transform.DOScale(m_positiveScale, 0f));
        sequence.Append(transform.DOMoveX(transform.position.x, m_patrolTime).SetEase(Ease.Linear));
        sequence.Join(m_body.transform.DOScale(m_negativeScale, 0f));
        sequence.SetLoops(-1, LoopType.Restart);
       
    }

    private void OnDestroy()
    {
        sequence.Kill();
    }


    private void OnTriggerEnter(Collider other)
    {
        OnGameLost?.Invoke();
    }
}
