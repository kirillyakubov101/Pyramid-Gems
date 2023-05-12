using System;
using System.Collections;
using UnityEngine;

public enum MoveState
{
    IDLE,
    RUN
}

public enum TransformationState
{
    HUMAN,
    EAGLE
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 10f;
    [SerializeField] private float MaxVelocity = 7f;
    [SerializeField] private float m_deadZoneY = -10;

    [SerializeField] private GameObject m_body;
    [SerializeField] private GameObject m_EagleObj;
    [SerializeField] private GameObject m_humanObj;
    [SerializeField] private CapsuleCollider m_capsuleCollider;
    [SerializeField] private SphereCollider m_sphereCollider; //for collecting
    [SerializeField] private float m_ShapeShiftDuration = 4f;

    private MoveState m_moveState;
    private TransformationState m_transformationState;

    private readonly int c_idleHashIndex = Animator.StringToHash("Idle");
    private readonly int c_runHashIndex = Animator.StringToHash("Running");

    private Animator m_animator;
    private Rigidbody m_rb;
    private Vector3 m_negativeScale = new Vector3(-1,1,-1);
    private Vector3 m_positiveScale = new Vector3(1,1,1);

    public static event Action<float,bool> OnShapeShiftCalled;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        m_animator.CrossFadeInFixedTime(c_idleHashIndex, 0.1f);
        m_moveState = MoveState.IDLE;
        m_transformationState = TransformationState.HUMAN;
    }

    private void OnEnable()
    {
        InputControls.OnTransform += ShapeShift;
    }

    private void OnDestroy()
    {
        InputControls.OnTransform -= ShapeShift;
    }

    private void Update()
    {
        AdjustState();
        AdjustOrientation();
        CheckDeadZone();
    }

    private void CheckDeadZone()
    {
        if(transform.position.y <= m_deadZoneY)
        {
            GameManager.Instance.GameLost();
        }
    }

    private void AdjustOrientation()
    {
        if (InputControls.Instance.DeltaX == 0) { return; }
        if (InputControls.Instance.DeltaX < 0)
        {
            m_body.transform.localScale = m_negativeScale;
        }
        else if(InputControls.Instance.DeltaX > 0)
        {
            m_body.transform.localScale = m_positiveScale;
        }
    }

    private void AdjustState()
    {
        if (InputControls.Instance.IsMoving)
        {
            SwitchState(MoveState.RUN);
        }
        else
        {
            SwitchState(MoveState.IDLE);
        }
    }

    private void FixedUpdate()
    {
        m_rb.AddForce(m_moveSpeed * Vector3.right * InputControls.Instance.DeltaX, ForceMode.Acceleration);
        m_rb.velocity = Vector3.ClampMagnitude(m_rb.velocity, MaxVelocity);
    }

    private void ShapeShift()
    {
        if(m_transformationState == TransformationState.EAGLE) { return; }

        StartCoroutine(ShapeShiftProcess());

    }

    private IEnumerator ShapeShiftProcess()
    {
        BecomeEagle();
        float timer = 0f;
        while(timer < m_ShapeShiftDuration)
        {
            timer += Time.deltaTime;
            OnShapeShiftCalled(Time.deltaTime / m_ShapeShiftDuration,false);
            yield return null;
        }
        OnShapeShiftCalled(0, true);
        BecomeHuman();
    }

    private void BecomeHuman()
    {
        m_capsuleCollider.enabled = true;
        m_sphereCollider.enabled = true;
        m_rb.useGravity = true;
        m_humanObj.SetActive(true);
        m_EagleObj.SetActive(false);
        m_transformationState = TransformationState.HUMAN;
    }

    private void BecomeEagle()
    {
        m_capsuleCollider.enabled = false;
        m_sphereCollider.enabled = false;
        m_rb.useGravity = false;
        m_humanObj.SetActive(false);
        m_EagleObj.SetActive(true);
        m_transformationState = TransformationState.EAGLE;
    }

    private void SwitchState(MoveState state)
    {
        if(m_moveState == state) { return; }

        switch(state)
        {
            case MoveState.IDLE:
                m_moveState = MoveState.IDLE;
                m_animator.CrossFadeInFixedTime(c_idleHashIndex, 0.1f);
                break;

            case MoveState.RUN:
                m_moveState = MoveState.RUN;
                m_animator.CrossFadeInFixedTime(c_runHashIndex, 0.1f);
                break;
        }
    }
}
