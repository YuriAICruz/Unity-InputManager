using Graphene.InputManager.ComboSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Graphene.InputManager.Presentation
{
    public class ButtonRemap : MonoBehaviour
    {
        public Text title;
        public Text keyConsole;
        public Text keyKeyboard;
        public Button remapConsole;
        public Button remapKeyboard;

        public void Setup(ComboChecker combo)
        {
            title.text = combo.hint;
            if (combo.Combo.Count == 1)
            {
                keyConsole.text = combo.Combo[0].input.ToString();
                keyKeyboard.text = combo.Combo[0].input.ToString();
            }
        }
    }
}