using System;
using System.Collections;
using Boo.Lang;
using Debuging;
using UnityEngine;
using Utils;

namespace Shooter.InputManage.ComboSystem
{
    public class ComboChecker
    {
        private readonly InputEvent[] _combo;
        private static readonly float _allowedTimeBetweenButtons = 0.3f; //the amount of time allowed to press between buttons to keep combo buildup alive

        private InputKey _lastInput = InputKey.Null;

        private int _currentIndex = 0;
        private float _timeLastButtonPressed;
        private Coroutine _holder;

        public enum State
        {
            Fail = 0,
            Success = 1,
            Waiting
        }

        public ComboChecker(InputEvent[] combo)
        {
            _combo = combo;
        }

        public void CheckCombo(InputEvent input, Action<State> response)
        {
            KillHolder(response);

            if (_currentIndex >= _combo.Length || Time.time > _timeLastButtonPressed + _combo[_currentIndex].betweenMaxTime)
            {
                _currentIndex = 0;
            }

            if (Time.time < _timeLastButtonPressed + _combo[_currentIndex].betweenMinTime)
            {
                response(State.Fail);
                return;
            }

            if (CompareInput(input))
            {
                if (_combo[_currentIndex].hold)
                {
                    Hold(_combo[_currentIndex], response);
                    return;
                }

                _timeLastButtonPressed = Time.time;
                _currentIndex++;
            }
            else
            {
                if (_combo[_currentIndex].hold)
                {
                    KillHolder(response);
                    return;
                }
            }

            if (_currentIndex < _combo.Length)
            {
                response(State.Fail);
                return;
            }

            response(State.Success);
            _lastInput = InputKey.Null;
        }

        private void KillHolder(Action<State> response)
        {
            if (_holder != null)
            {
                GlobalCoroutineManager.Instance.StopCoroutine(_holder);
                _holder = null;
                response(State.Fail);
            }
        }

        private void Hold(InputEvent input, Action<State> response)
        {
            _holder = GlobalCoroutineManager.Instance.StartCoroutine(HoldRoutine(input, response));
        }

        IEnumerator HoldRoutine(InputEvent input, Action<State> response)
        {
            yield return new WaitForSeconds(input.holdTime);

            yield return 0;

            response(State.Success);
        }

        private bool CompareInput(InputEvent input)
        {
            return _combo[_currentIndex].input == input.input && _combo[_currentIndex].down == input.down;
        }
    }
}