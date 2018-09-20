using System.Collections.Generic;
using System.Xml.Schema;
using Graphene.InputManager.ComboSystem;
using UnityEditor;
using UnityEngine;

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
            new InputManagerUtils.InputAxis("Vertical", InputManagerUtils.AxisType.JoystickAxis, 1, 0),
            new InputManagerUtils.InputAxis("Horizontal", InputManagerUtils.AxisType.JoystickAxis, 0, 0),
            new InputManagerUtils.InputAxis("Submit",  "joystick button 0", 0),
            new InputManagerUtils.InputAxis("Cancel",  "joystick button 1", 0)
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
            new InputManagerUtils.InputAxis("Vertical",  "w", "s"),
            new InputManagerUtils.InputAxis("Horizontal",  "d", "a"),
            new InputManagerUtils.InputAxis("Submit",  "d", "a"),
            new InputManagerUtils.InputAxis("Cancel",  "d", "a")
        };

        private List<bool> _opened;

        private void Awake()
        {
            _self = (InputData) target;
            
            _opened = new List<bool>();
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

            _indentation = EditorGUI.indentLevel;

            DrawComboList();
            
            EditorGUI.indentLevel = _indentation;
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
        
        
        public static void SetupInputManager()
        {
            // Add mouse definitions
            InputManagerUtils.AddAxis(new InputManagerUtils.InputAxis() { name = "myMouseX",        sensitivity = 1f, type = InputManagerUtils.AxisType.MouseMovement, axis = 1 });
            InputManagerUtils.AddAxis(new InputManagerUtils.InputAxis() { name = "myMouseY",        sensitivity = 1f, type = InputManagerUtils.AxisType.MouseMovement, axis = 2 });
            InputManagerUtils.AddAxis(new InputManagerUtils.InputAxis() { name = "myScrollWheel", sensitivity = 1f, type = InputManagerUtils.AxisType.MouseMovement, axis = 3 });

            // Add gamepad definitions
            int i = 1;
            //for (int i = 1; i <= (int)InputBind.Gamepad.Gamepad4; i++)
            //{
//            for (int j = 0; j <= (int)InputManagerUtils.InputBind.GamepadAxis.Axis10; j++)
//            {
//                InputManagerUtils.AddAxis(new InputManagerUtils.InputAxis() 
//                { 
//                    name = "myPad" + i + "A" + (j + 1).ToString(), 
//                    dead = 0.2f,
//                    sensitivity = 1f,
//                    type = InputManagerUtils.AxisType.JoystickAxis,
//                    axis = (j + 1),
//                    joyNum = i,
//                });
//            }
            //}
        }

        private void DrawComboList()
        {
            EditorGUILayout.BeginVertical();
            
            if(_opened == null)
                _opened = new List<bool>();
            if(_self.Inputs == null)
                _self.Inputs = new List<ComboChecker>();

            var count = _self.Inputs.Count - _opened.Count;
            for (int x = _opened.Count-1; x < count; x++)
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
                
                EditorGUI.indentLevel++;
                var j = 0;
                foreach (var inputEvent in input.Combo)
                {
                    if (DrawInputEvent(i, j, inputEvent)) return;

                    j++;
                }
                if (GUILayout.Button("Add")) AddInputEvent(i);
                EditorGUI.indentLevel--;
                if (GUILayout.Button("Remove"))
                {
                    RemoveInput(i);
                    return;
                }
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

        private bool FoldButtons(int j, ComboChecker input)
        {
            EditorGUILayout.BeginHorizontal();

            var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
            
            if (_opened[j])
                input.hint = EditorGUILayout.TextField(input.hint);
            else
                EditorGUILayout.LabelField(input.hint, style, GUILayout.ExpandWidth(true));
            
            if (!_opened[j])
            {
                if (GUILayout.Button("\\/", GUILayout.MinWidth(20), GUILayout.MaxWidth(40)))
                {
                    _opened[j] = true;
                }
                EditorGUILayout.EndHorizontal();
                
                return true;
            }
            
            if (GUILayout.Button("/\\", GUILayout.MinWidth(20), GUILayout.MaxWidth(40)))
            {
                _opened[j] = false;
            }
            EditorGUILayout.EndHorizontal();

            return false;
        }

        private void AddInput()
        {
            _self.Inputs.Add(new ComboChecker(new List<InputEvent>()));
        }

        private void RemoveInput(int i)
        {
            _self.Inputs.RemoveAt(i);
        }

        private void AddInputEvent(int i)
        {
            _self.Inputs[i].Combo.Add(new InputEvent());
        }

        private void RemoveInputEvent(int i, int j)
        {
            _self.Inputs[i].Combo.RemoveAt(j);
        }
    }
}