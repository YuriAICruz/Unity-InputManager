using UnityEditor;

namespace Graphene.InputManager
{
    public class InputManagerUtils
    {
        public enum AxisType
        {
            KeyOrMouseButton = 0,
            MouseMovement = 1,
            JoystickAxis = 2
        };

        public class InputAxis
        {
            public string name;
            public string descriptiveName;
            public string descriptiveNegativeName;
            public string negativeButton;
            public string positiveButton;
            public string altNegativeButton;
            public string altPositiveButton;

            public float gravity;
            public float dead;
            public float sensitivity;

            public bool snap = false;
            public bool invert = false;

            public AxisType type;

            public int axis;
            public int joyNum;

            public InputAxis()
            {
                
            }

            public InputAxis(string name, string positiveButton, int joyNum)
            {
                this.name = name;
                this.positiveButton = positiveButton;

                this.gravity = 1000;
                this.dead = 0.001f;
                this.sensitivity = 1000;

                this.joyNum = joyNum;
            }

            public InputAxis(string name, string positiveButton, string negativeButton)
            {
                this.name = name;
                this.positiveButton = positiveButton;
                this.negativeButton = negativeButton;

                this.gravity = 3;
                this.dead = 0.001f;
                this.sensitivity = 3;

                this.joyNum = joyNum;
            }

            public InputAxis(string name, AxisType type, int axis, int joyNum, bool invert = false)
            {
                this.name = name;
                this.type = type;

                this.gravity = 0;
                this.dead = 0.19f;
                this.sensitivity = 1;

                this.invert = invert;

                this.axis = axis;
                this.joyNum = joyNum;
            }
        }
        
        private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
        {
            SerializedProperty child = parent.Copy();
            child.Next(true);
            do
            {
                if (child.name == name) return child;
            } while (child.Next(false));
            return null;
        }

        private static bool AxisDefined(string axisName)
        {
            var inputsDatabase = GetInputsDatabase();
            var axesProperty = GetAllInputs(inputsDatabase);

            axesProperty.Next(true);
            axesProperty.Next(true);
            while (axesProperty.Next(false))
            {
                SerializedProperty axis = axesProperty.Copy();
                axis.Next(true);
                if (axis.stringValue == axisName) return true;
            }
            return false;
        }

        private static bool AxisDefined(string axisName, string positive)
        {
            var inputs = GetAllInputs();

            for (int i = 0, n = inputs.arraySize; i < n; i++)
            {
                var input = InputManagerUtils.Deserialize(inputs.GetArrayElementAtIndex(i));
                if (input.name == axisName && input.positiveButton == positive) return true;
            }
            
            return false;
        }

        public static SerializedProperty GetAllInputs(SerializedObject db = null)
        {
            if(db == null)
                db = GetInputsDatabase();
            
            return db.FindProperty("m_Axes");
        }

        public static SerializedObject GetInputsDatabase()
        {
            var serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            return serializedObject;
        }

        public static void ClearAllInputs()
        {
            var inputsDatabase = GetInputsDatabase();;
            var axesProperty = GetAllInputs(inputsDatabase);
            
            axesProperty.ClearArray();
            inputsDatabase.ApplyModifiedProperties();
        }

        public static InputAxis Deserialize(SerializedProperty input)
        {
            var i = new InputAxis();
            i.name = input.FindPropertyRelative("m_Name").stringValue;
            i.descriptiveName = input.FindPropertyRelative("descriptiveName").stringValue;
            i.descriptiveNegativeName = input.FindPropertyRelative("descriptiveNegativeName").stringValue;
            i.negativeButton = input.FindPropertyRelative("negativeButton").stringValue;
            i.positiveButton = input.FindPropertyRelative("positiveButton").stringValue;
            i.altNegativeButton = input.FindPropertyRelative("altNegativeButton").stringValue;
            i.altPositiveButton = input.FindPropertyRelative("altPositiveButton").stringValue;
            i.gravity = input.FindPropertyRelative("gravity").floatValue;
            i.dead = input.FindPropertyRelative("dead").floatValue;
            i.sensitivity = input.FindPropertyRelative("sensitivity").floatValue;
            i.snap = input.FindPropertyRelative("snap").boolValue;
            i.invert = input.FindPropertyRelative("invert").boolValue;
            i.type = (AxisType) input.FindPropertyRelative("type").intValue;
            i.axis = input.FindPropertyRelative("axis").intValue;
            i.joyNum = input.FindPropertyRelative("joyNum").intValue;

            return i;
        }

        public static void AddAxis(InputAxis axis)
        {
            if (AxisDefined(axis.name, axis.positiveButton)) return;

            var inputsDatabase = GetInputsDatabase();
            var axesProperty = GetAllInputs(inputsDatabase);

            axesProperty.arraySize++;
            inputsDatabase.ApplyModifiedProperties();

            SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

            GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
            GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
            GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
            GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
            GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
            GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
            GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
            GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
            GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
            GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
            GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
            GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
            GetChildProperty(axisProperty, "type").intValue = (int) axis.type;
            GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
            GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

            inputsDatabase.ApplyModifiedProperties();
        }
    }
}