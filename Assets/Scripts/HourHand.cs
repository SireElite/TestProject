using UnityEngine;

// Created own script for HourHand, because it has some unique features:
// Max value which you can set in inputField is 23, while other hands have 60 (it ends on zero, so you can put any second digit from 0 to 9, but hour hand only from 0 to 3)
// The clock face can show only 12, while max value is 24 (it breaks it's rotation, so in line 14 max value divided by 2)
// Max first digit for other hands is 5 (max value you can set in input field is 59), but for hour hand its 2 (max is 23);

public class HourHand : ClockHand
{
    protected int _maxSecondDigitInInputField => (int)char.GetNumericValue(_maxValue.ToString()[1]) - 1;

    public override void SetValue(int newValue)
    {
        Value = newValue;
        transform.eulerAngles = new Vector3(0f, 0f, -((float)Value / ((float)_maxValue / 2f))) * 360f;
        _valueInputField.text = Value.ToString("D2");
    }

    protected override void ConfirmFirstDigit(string valueInputFieldText)
    {
        if(string.IsNullOrEmpty(valueInputFieldText))
        {
            return;
        }
        else if(valueInputFieldText.Length == 1 && int.Parse(valueInputFieldText) > _maxFirstDigitInInputField)
        {
            _valueInputField.text = string.Empty;
        }
        else if(valueInputFieldText.Length == 2 && char.GetNumericValue(valueInputFieldText[1]) > _maxSecondDigitInInputField)
        {
            _valueInputField.text = valueInputFieldText[0].ToString();
        }
    }
}
