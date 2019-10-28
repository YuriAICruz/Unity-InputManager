using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace Graphene.InputManager.Presentation
{
    public class ShowButtons : MonoBehaviour
    {
        [Tooltip("From the Resources Folder")] public string dataPath = "Input/InputData";
        
        private List<ButtonRemap> _remap;

        public ButtonRemap prefab;

        private InputData _inputData;

        private void Awake()
        {
            var path = string.IsNullOrEmpty(dataPath) ? "Input/InputData" : dataPath;
            _inputData = Resources.Load<InputData>(path);
        }

        private void Start()
        {
            var i = 0;
            foreach (var combo in _inputData.Inputs)
            {
                var cb = Instantiate(prefab, transform);
                cb.Setup(combo);
                cb.transform.SetSiblingIndex(i);
                i ++;
            }
        }
    }
}