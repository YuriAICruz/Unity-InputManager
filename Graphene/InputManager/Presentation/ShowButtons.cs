using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Graphene.InputManager.ComboSystem;
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
            var keys = Enum.GetValues(typeof(InputKey)).Cast<InputKey>().ToList();
            for (int i = 0, n = keys.Count; i < n; i++)
            {
                if(keys[i] == InputKey.Null) continue;
                var cb = Instantiate(prefab, transform);
                cb.Setup(keys[i], _inputData);
                cb.transform.SetSiblingIndex(i);
            }
        }
    }
}