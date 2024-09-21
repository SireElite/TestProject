using TMPro;
using UnityEngine;

public class ClockHand : MonoBehaviour
{
    [SerializeField] protected int _maxValue;
    [SerializeField] protected int _rotationDegree;
    [SerializeField] protected TMP_InputField _valueInputField;

    public ClockHand NextClockHandToRotate { get; set; }
    public int Value { get; protected set; }

    protected int _maxFirstDigitInInputField => (int)char.GetNumericValue(_maxValue.ToString()[0]);

    private void OnEnable() => Clock.OnEditingToggleValueChanged += HandleEditingMode;

    private void OnDisable() => Clock.OnEditingToggleValueChanged -= HandleEditingMode;

    protected void HandleEditingMode(bool isEditingModeEnabled)
    {
        if(isEditingModeEnabled)
        {
            EnterEditMode();
        }
        else
        {
            ExitEditMode();
        }
    }

    protected void EnterEditMode()
    {
        _valueInputField.interactable = true;

        _valueInputField.onEndEdit.AddListener((value) =>
        {
            if(!string.IsNullOrEmpty(value))
                SetValue(int.Parse(value));
        });

        _valueInputField.onValueChanged.AddListener(ConfirmFirstDigit);
    }

    protected void ExitEditMode()
    {
        _valueInputField.interactable = false;
        _valueInputField.onEndEdit.RemoveAllListeners();
        _valueInputField.onValueChanged.RemoveAllListeners();
    }

    protected virtual void ConfirmFirstDigit(string ss)
    {
        if(string.IsNullOrEmpty(ss))
        {
            return;
        }
        else if(ss.Length == 1 && int.Parse(ss) > _maxFirstDigitInInputField - 1)
        {
            _valueInputField.text = string.Empty;
        }
    }

    public void RotateHand()
    {
        Value++;
        transform.eulerAngles += new Vector3(0f, 0f, -_rotationDegree);

        if(Value == _maxValue)
        {
            Value = 0;

            if(NextClockHandToRotate != null)
                NextClockHandToRotate.RotateHand();
        }

        _valueInputField.text = Value.ToString("D2");
    }

    public virtual void SetValue(int newValue)
    {
        Value = newValue;
        transform.eulerAngles = new Vector3(0f, 0f, -((float)Value / (float)_maxValue)) * 360f;
        _valueInputField.text = Value.ToString("D2");
    }
}
