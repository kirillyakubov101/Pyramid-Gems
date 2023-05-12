using System;
using UnityEngine;

public class InputControls : Singleton<InputControls>
{
    [SerializeField] private Joystick m_joystick;

    public static event Action OnTransform;

    private float m_deltaX;
    private float m_deltaY;
    private bool m_isMoving = false;

    public float DeltaX { get => m_deltaX; }
    public float DeltaY { get => m_deltaY; }
    public bool IsMoving { get => m_isMoving; }

    private void Update()
    {
        m_deltaX = m_joystick.Horizontal;
        m_deltaY = m_joystick.Vertical;

        m_isMoving = m_deltaX != 0 ? true : false;
    }

    public void TransformMagicShift()
    {
        OnTransform?.Invoke();
    }

}
