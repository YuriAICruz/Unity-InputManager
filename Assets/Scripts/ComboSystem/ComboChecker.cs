using System;
using System.Collections;
using System.Collections.Generic;
using Graphene.Utils;
using UnityEngine;

namespace Graphene.InputManager.ComboSystem
{
    [Serializable]
    public class ComboChecker
    {
        public string hint;
        public List<InputEvent> Combo;

        public Action Invoke;
        
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

        public ComboChecker(List<InputEvent> combo)
        {
            Combo = combo;
        }

        public void CheckCombo(InputEvent input, Action<State> response)
        {
            KillHolder(response);

            if (_currentIndex >= Combo.Count || Time.time > _timeLastButtonPressed + Combo[_currentIndex].betweenMaxTime)
            {
                _currentIndex = 0;
            }

            if (Time.time < _timeLastButtonPressed + Combo[_currentIndex].betweenMinTime)
            {
                response(State.Fail);
                return;
            }

            if (CompareInput(input))
            {
                if (Combo[_currentIndex].hold)
                {
                    Hold(Combo[_currentIndex], response);
                    return;
                }

                _timeLastButtonPressed = Time.time;
                _currentIndex++;
            }
            else
            {
                if (Combo[_currentIndex].hold)
                {
                    KillHolder(response);
                    return;
                }
            }

            if (_currentIndex < Combo.Count)
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
            return Combo[_currentIndex].input == input.input && Combo[_currentIndex].down == input.down;
        }
    }
}