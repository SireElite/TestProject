using UnityEngine;
using UnityEngine.UI;
using System;

public class Clock : MonoBehaviour
{
    [SerializeField] private ClockHand _secondHand;
    [SerializeField] private ClockHand _minuteHand;
    [SerializeField] private ClockHand _hourHand;
    [SerializeField] private Toggle _editingModeToggle;
    
    public static Action<bool> OnEditingToggleValueChanged;

    private readonly float _oneHourInSeconds = 3600f;

    private TimeProvider _timeProvider = new TimeProvider();

    private void Awake()
    {
        _secondHand.NextClockHandToRotate = _minuteHand;
        _minuteHand.NextClockHandToRotate = _hourHand;
    }

    private void OnEnable()
    {
        _timeProvider.OnNewTimeReceived += SetNewHandPositions;
        _editingModeToggle.onValueChanged.AddListener(HandleEditingToggleValue);
        InvokeRepeating(nameof(SynchronizeTime), 0f, _oneHourInSeconds);
    }

    private void OnDisable()
    {
        _timeProvider.OnNewTimeReceived -= SetNewHandPositions;
        _editingModeToggle.onValueChanged.RemoveListener(HandleEditingToggleValue);
        CancelInvoke();
    }

    private void HandleEditingToggleValue(bool isInEditingMode)
    {
        OnEditingToggleValueChanged?.Invoke(isInEditingMode);

        if(isInEditingMode)
        {
            CancelInvoke(nameof(RotateSecondHand));
        }
        else
        {
            InvokeRepeating(nameof(RotateSecondHand), 1f, 1f);
        }
    }

    private void SetNewHandPositions(TimeResponse time)
    {
        _secondHand.SetValue(time.seconds);
        _minuteHand.SetValue(time.minute);
        _hourHand.SetValue(time.hour);

        CancelInvoke(nameof(RotateSecondHand));
        InvokeRepeating(nameof(RotateSecondHand), 1f, 1f);
    }

    private void SynchronizeTime() => StartCoroutine(_timeProvider.RequestCurrentTimeCoroutine());
    private void RotateSecondHand() => _secondHand.RotateHand();
}
