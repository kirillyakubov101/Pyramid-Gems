using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TurorialHandler : MonoBehaviour
{
    [SerializeField] private Image m_movementImagePointer;
    [SerializeField] private Image m_shapeShiftImagePointer;
    [SerializeField] private GameObject m_avoidEnemyContainer;
    [SerializeField] private float m_fadeDuration = 2f;
    [SerializeField] private float m_fadeDelay = 0.2f;

    private Sequence sequence;
    private bool m_waitingForShapeShiftPress = false;
    private bool m_hasShapeShiftPressed = false;

    private async void Start()
    {
        await ShowMovementHandle();
        await Task.Delay(1000);
        await ShowShapeShiftHandle();
        await Task.Delay(1000);
        await ShowAvoidEnemyInfo();
    }

    private void OnEnable()
    {
        InputControls.OnTransform += ShapeShiftPressHandle;
    }

    private void OnDisable()
    {
        InputControls.OnTransform -= ShapeShiftPressHandle;
    }

    private void ShapeShiftPressHandle()
    {
        if(m_waitingForShapeShiftPress)
        {
            m_hasShapeShiftPressed = true;
        }
    }

    private async Task ShowMovementHandle()
    {
       
        sequence = DOTween.Sequence();
        sequence.Append(m_movementImagePointer.DOFade(1f, m_fadeDuration));
        sequence.AppendInterval(m_fadeDelay);
        sequence.Append(m_movementImagePointer.DOFade(0f, m_fadeDuration));
        sequence.SetLoops(-1, LoopType.Restart);

        while (InputControls.Instance.DeltaX == 0)
        {
            await Task.Yield();
        }

        m_movementImagePointer.gameObject.SetActive(false);
        sequence.Kill();
    }

    private async Task ShowShapeShiftHandle()
    {
        m_waitingForShapeShiftPress = true;
        m_shapeShiftImagePointer.gameObject.SetActive(true);

        sequence = DOTween.Sequence();
        sequence.Append(m_shapeShiftImagePointer.DOFade(1f, m_fadeDuration));
        sequence.AppendInterval(m_fadeDelay);
        sequence.Append(m_shapeShiftImagePointer.DOFade(0f, m_fadeDuration));
        sequence.SetLoops(-1, LoopType.Restart);

        while (!m_hasShapeShiftPressed)
        {
            await Task.Yield();
        }

        m_shapeShiftImagePointer.gameObject.SetActive(false);
        sequence.Kill();
    }

    private async Task ShowAvoidEnemyInfo()
    {
        m_avoidEnemyContainer.SetActive(true);
        await Task.Delay(1000); //wait 4 sec..
        m_avoidEnemyContainer.SetActive(false);
    }

    private void OnDestroy()
    {
        if(sequence != null)
        {
            sequence.Kill();
        }
       
    }
}
