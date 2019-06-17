using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Graphene.InputManager.ComboSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace Graphene.InputManager
{
    [CustomEditor(typeof(InputData))]
    public class InputDataInspector : Editor
    {
        private InputData _self;
        private int _indentation;

        private List<InputManagerUtils.InputAxis> _xboxControllerInputs = new List<InputManagerUtils.InputAxis>()
        {
            new InputManagerUtils.InputAxis("Button_A", "joystick button 0", 0),
            new InputManagerUtils.InputAxis("Button_B", "joystick button 1", 0),
            new InputManagerUtils.InputAxis("Button_X", "joystick button 2", 0),
            new InputManagerUtils.InputAxis("Button_Y", "joystick button 3", 0),
            new InputManagerUtils.InputAxis("Button_RB", "joystick button 5", 0),
            new InputManagerUtils.InputAxis("Button_RT", InputManagerUtils.AxisType.JoystickAxis, 3, 0),
            new InputManagerUtils.InputAxis("Button_LB", "joystick button 4", 0),
            new InputManagerUtils.InputAxis("Button_LT", InputManagerUtils.AxisType.JoystickAxis, 3, 0),
            new InputManagerUtils.InputAxis("Button_Start", "joystick button 7", 0),
            new InputManagerUtils.InputAxis("Button_Select", "joystick button 6", 0),
            new InputManagerUtils.InputAxis("Button_DPad_Vertical", InputManagerUtils.AxisType.JoystickAxis, 7, 0),
            new InputManagerUtils.InputAxis("Button_DPad_Horizontal", InputManagerUtils.AxisType.JoystickAxis, 6, 0),
            new InputManagerUtils.InputAxis("Right_Stick_Vertical", InputManagerUtils.AxisType.JoystickAxis, 5, 0),
            new InputManagerUtils.InputAxis("Right_Stick_Horizontal", InputManagerUtils.AxisType.JoystickAxis, 4, 0),
            new InputManagerUtils.InputAxis("Vertical", InputManagerUtils.AxisType.JoystickAxis, 2, 0),
            new InputManagerUtils.InputAxis("Horizontal", InputManagerUtils.AxisType.JoystickAxis, 1, 0),
            new InputManagerUtils.InputAxis("Submit", "joystick button 0", 0),
            new InputManagerUtils.InputAxis("Cancel", "joystick button 1", 0),
#if UNITY_XR
            new InputManagerUtils.InputAxis("Oculus_Grip_L", "joystick button 11", 0),
            new InputManagerUtils.InputAxis("Oculus_Grip_R", "joystick button 12", 0),
            new InputManagerUtils.InputAxis("Oculus_Index_Touch_L", "joystick button 14", 0),
            new InputManagerUtils.InputAxis("Oculus_Index_Touch_R", "joystick button 15", 0),
            new InputManagerUtils.InputAxis("Oculus_Trigger_L_Axis", InputManagerUtils.AxisType.JoystickAxis, 9, 0),
            new InputManagerUtils.InputAxis("Oculus_Trigger_R_Axis", InputManagerUtils.AxisType.JoystickAxis, 10, 0),
            
            new InputManagerUtils.InputAxis("Vive_Thumb_L", "joystick button 8", 0),
            new InputManagerUtils.InputAxis("Vive_Thumb_R", "joystick button 9", 0),
            new InputManagerUtils.InputAxis("Vive_Trigger_L", "joystick button 14", 0),
            new InputManagerUtils.InputAxis("Vive_Trigger_R", "joystick button 15", 0),
            new InputManagerUtils.InputAxis("Vive_Thumb_Touch_L", "joystick button 16", 0),
            new InputManagerUtils.InputAxis("Vive_Thumb_Touch_R", "joystick button 17", 0),
            new InputManagerUtils.InputAxis("Vive_Thumb_L_Horizontal", InputManagerUtils.AxisType.JoystickAxis, 1, 0),
            new InputManagerUtils.InputAxis("Vive_Thumb_L_Vertical", InputManagerUtils.AxisType.JoystickAxis, 2, 0),
            new InputManagerUtils.InputAxis("Vive_Thumb_R_Horizontal", InputManagerUtils.AxisType.JoystickAxis, 4, 0),
            new InputManagerUtils.InputAxis("Vive_Thumb_R_Vertical", InputManagerUtils.AxisType.JoystickAxis, 5, 0),
            
            new InputManagerUtils.InputAxis("Vive_Trigger_L_Axis", InputManagerUtils.AxisType.JoystickAxis, 9, 0),
            new InputManagerUtils.InputAxis("Vive_Trigger_R_Axis", InputManagerUtils.AxisType.JoystickAxis, 10, 0),
            new InputManagerUtils.InputAxis("Vive_Grip_L_Average", InputManagerUtils.AxisType.JoystickAxis, 11, 0),
            new InputManagerUtils.InputAxis("Vive_Grip_R_Average", InputManagerUtils.AxisType.JoystickAxis, 12, 0),
            new InputManagerUtils.InputAxis("Axis 13", InputManagerUtils.AxisType.JoystickAxis, 13, 0),
            new InputManagerUtils.InputAxis("Axis 14", InputManagerUtils.AxisType.JoystickAxis, 14, 0),
            new InputManagerUtils.InputAxis("Axis 15", InputManagerUtils.AxisType.JoystickAxis, 15, 0),
            new InputManagerUtils.InputAxis("Axis 16", InputManagerUtils.AxisType.JoystickAxis, 16, 0),
            new InputManagerUtils.InputAxis("Axis 17", InputManagerUtils.AxisType.JoystickAxis, 17, 0),
            new InputManagerUtils.InputAxis("Axis 18", InputManagerUtils.AxisType.JoystickAxis, 18, 0),
            new InputManagerUtils.InputAxis("Axis 19", InputManagerUtils.AxisType.JoystickAxis, 19, 0),
            new InputManagerUtils.InputAxis("Axis 20", InputManagerUtils.AxisType.JoystickAxis, 20, 0),
            new InputManagerUtils.InputAxis("Axis 21", InputManagerUtils.AxisType.JoystickAxis, 21, 0),
            new InputManagerUtils.InputAxis("Axis 22", InputManagerUtils.AxisType.JoystickAxis, 22, 0),
            new InputManagerUtils.InputAxis("Axis 23", InputManagerUtils.AxisType.JoystickAxis, 23, 0),
            new InputManagerUtils.InputAxis("Axis 24", InputManagerUtils.AxisType.JoystickAxis, 24, 0),
            new InputManagerUtils.InputAxis("Axis 25", InputManagerUtils.AxisType.JoystickAxis, 25, 0),
            new InputManagerUtils.InputAxis("Axis 26", InputManagerUtils.AxisType.JoystickAxis, 26, 0),
            new InputManagerUtils.InputAxis("Axis 27", InputManagerUtils.AxisType.JoystickAxis, 27, 0),
#endif
        };

        private List<InputManagerUtils.InputAxis> _mouseKeyboardControllerInputs = new List<InputManagerUtils.InputAxis>()
        {
            new InputManagerUtils.InputAxis("Button_A", "space", 0),
            new InputManagerUtils.InputAxis("Button_A", "mouse 0", 0),
            new InputManagerUtils.InputAxis("Button_B", "left alt", 0),
            new InputManagerUtils.InputAxis("Button_B", "mouse 1", 0),
            new InputManagerUtils.InputAxis("Button_X", "left ctrl", 0),
            new InputManagerUtils.InputAxis("Button_Y", "r", 0),
            new InputManagerUtils.InputAxis("Button_RB", "e", 0),
            new InputManagerUtils.InputAxis("Button_RT", "c", 0),
            new InputManagerUtils.InputAxis("Button_LB", "q", 0),
            new InputManagerUtils.InputAxis("Button_LT", "z", 0),
            new InputManagerUtils.InputAxis("Button_Start", "return", 0),
            new InputManagerUtils.InputAxis("Button_Select", "escape", 0),
            new InputManagerUtils.InputAxis("Button_DPad_Vertical", "up", "down"),
            new InputManagerUtils.InputAxis("Button_DPad_Horizontal", "right", "left"),
            new InputManagerUtils.InputAxis("Right_Stick_Vertical", "[8]", "[5]"),
            new InputManagerUtils.InputAxis("Right_Stick_Horizontal", "[6]", "[4]"),
            new InputManagerUtils.InputAxis("Vertical", "w", "s"),
            new InputManagerUtils.InputAxis("Horizontal", "d", "a"),
            new InputManagerUtils.InputAxis("Submit", "d", "a"),
            new InputManagerUtils.InputAxis("Cancel", "d", "a")
        };

        private List<bool> _opened;
        private bool _dirty;

        private void Awake()
        {
            _self = (InputData) target;

            _opened = new List<bool>();

            SetupDefiningSymbols();
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();

            if (GUILayout.Button("Clar All InputManager Entries"))
                ClearAllInputManagerEntries();

            EditorGUILayout.Space();

            if (GUILayout.Button("Generate InputManager Entries"))
                GenerateInputManagerEntries();

            EditorGUILayout.Space();
            
            if (PlayerSettings.virtualRealitySupported)
                EditorGUILayout.LabelField("VR Enabled - Add Inputs - Detected: " + XRDevice.model);
            
            EditorGUILayout.Space();

            _indentation = EditorGUI.indentLevel;

            _dirty = false;

            DrawComboList();

            EditorGUI.indentLevel = _indentation;

            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(_self);

            if (_dirty)
                OrderById();
        }

        private void SetupDefiningSymbols()
        {
            if (PlayerSettings.virtualRealitySupported)
            {
                var set = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
                
                Debug.Log(set.Contains("UNITY_XR"));
                
                if (!set.Contains("UNITY_XR"))
                {
                    set += ";UNITY_XR";
                }
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, set);
            }
            else
            {
                var set = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
                if (set.Contains("UNITY_XR"))
                {
                    set = set.Replace("UNITY_XR", "");
                }
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, set);
            }
        }

        private void OrderById()
        {
            _self.Inputs = _self.Inputs.OrderBy(x => x.Id).ToList();
        }

        private void ClearAllInputManagerEntries()
        {
            InputManagerUtils.ClearAllInputs();
        }

        private void GenerateInputManagerEntries()
        {
            foreach (var controllerInput in _xboxControllerInputs)
            {
                InputManagerUtils.AddAxis(controllerInput);
            }
            foreach (var controllerInput in _mouseKeyboardControllerInputs)
            {
                InputManagerUtils.AddAxis(controllerInput);
            }
        }

        private void DrawComboList()
        {
            EditorGUILayout.BeginVertical();

            if (_opened == null)
                _opened = new List<bool>();
            if (_self.Inputs == null)
                _self.Inputs = new List<ComboChecker>();

            var count = _self.Inputs.Count - _opened.Count;
            for (int x = 0; x < count; x++)
            {
                _opened.Add(false);
            }

            EditorGUI.indentLevel++;

            var i = 0;
            foreach (var input in _self.Inputs)
            {
                var rect = EditorGUILayout.BeginVertical();

                CreateBg(rect);

                if (FoldButtons(i, input))
                {
                    i++;
                    EditorGUILayout.EndVertical();
                    continue;
                }

                input.Id = EditorGUILayout.IntField("Id: ", input.Id);
                
                EditorGUI.indentLevel++;
                var j = 0;
                foreach (var inputEvent in input.Combo)
                {
                    if (DrawInputEvent(i, j, inputEvent)) return;

                    j++;
                }
                if (GUILayout.Button("Add")) AddInputEvent(i);
                EditorGUI.indentLevel--;
                i++;
                EditorGUILayout.EndVertical();
            }
            EditorGUI.indentLevel--;

            if (GUILayout.Button("Add")) AddInput();

            EditorGUILayout.EndVertical();
        }

        private bool DrawInputEvent(int i, int j, InputEvent inputEvent)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("X", GUILayout.MaxWidth(20)))
            {
                RemoveInputEvent(i, j);
                return true;
            }

            var width = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 80;

            inputEvent.input = (InputKey) EditorGUILayout.EnumFlagsField("Input", inputEvent.input);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Min: " + inputEvent.betweenMinTime + " Max: " + inputEvent.betweenMaxTime);
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.MinMaxSlider("Time", ref inputEvent.betweenMinTime, ref inputEvent.betweenMaxTime, 0, 2);


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();

            GUI.enabled = inputEvent.down;
            if (GUILayout.Button("Up", GUILayout.MaxWidth(120)))
                inputEvent.down = !inputEvent.down;

            EditorGUILayout.Space();
            GUI.enabled = !inputEvent.down;
            if (GUILayout.Button("Down", GUILayout.MaxWidth(120)))
                inputEvent.down = !inputEvent.down;

            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;

            EditorGUILayout.BeginHorizontal();
            inputEvent.hold = EditorGUILayout.Toggle("Hold", inputEvent.hold);
            if (inputEvent.hold)
            {
                EditorGUIUtility.labelWidth = 140;
                inputEvent.holdTime = EditorGUILayout.Slider("Duration", inputEvent.holdTime, 0, 2);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUIUtility.labelWidth = width;
            return false;
        }

        private static void CreateBg(Rect rect)
        {
            GUI.enabled = false;
            GUI.Button(rect, GUIContent.none);
            GUI.enabled = true;
        }

        private bool FoldButtons(int i, ComboChecker input)
        {
            EditorGUILayout.BeginHorizontal();

            var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};

            if (_opened[i])
            {
                if (GUILayout.Button("X", GUILayout.MaxWidth(28)))
                {
                    RemoveInput(i);
                    return true;
                }
                input.hint = EditorGUILayout.TextField(input.hint);
            }
            else
                EditorGUILayout.LabelField(input.Id + ": " + input.hint, style, GUILayout.ExpandWidth(true));

            if (!_opened[i])
            {
                if (GUILayout.Button("\\/", GUILayout.MinWidth(20), GUILayout.MaxWidth(40)))
                {
                    _dirty = true;
                    _opened[i] = true;
                }
                EditorGUILayout.EndHorizontal();

                return true;
            }

            if (GUILayout.Button("/\\", GUILayout.MinWidth(20), GUILayout.MaxWidth(40)))
            {
                _dirty = true;
                _opened[i] = false;
            }
            EditorGUILayout.EndHorizontal();

            return false;
        }

        private void AddInput()
        {
            _self.Inputs.Add(new ComboChecker(new List<InputEvent>()));
            _dirty = true;
        }

        private void RemoveInput(int i)
        {
            _self.Inputs.RemoveAt(i);
            _dirty = true;
        }

        private void AddInputEvent(int i)
        {
            _self.Inputs[i].Combo.Add(new InputEvent());
            _dirty = true;
        }

        private void RemoveInputEvent(int i, int j)
        {
            _self.Inputs[i].Combo.RemoveAt(j);
            _dirty = true;
        }
    }
}