using UnityEngine;
using UnityEngine.UI;

public class ShapeShiftUI : MonoBehaviour
{
    [SerializeField] private Image m_shapeShiftProgressImage;

    private void OnEnable()
    {
        PlayerMovement.OnShapeShiftCalled += UpdateUI;
    }

    private void OnDestroy()
    {
        PlayerMovement.OnShapeShiftCalled -= UpdateUI;
    }

    private void UpdateUI(float val,bool reset)
    {
        if(reset)
        {
            m_shapeShiftProgressImage.fillAmount = 1;
        }

        m_shapeShiftProgressImage.fillAmount -= val;
    }
}
