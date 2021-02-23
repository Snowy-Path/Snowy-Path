using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
 
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.InputSystem.Editor;
#endif
 
namespace UnityEngine.InputSystem.Composites
{
 
#if UNITY_EDITOR
    [InitializeOnLoad] // Automatically register in editor.
#endif
 
    [DisplayStringFormat("{MouseButton}+{MouseVector}")]
    public class ClickAndDragBinding : InputBindingComposite<Vector2>
    {
        [InputControl(layout = "Button")]
        public int MouseButton;
 
        [InputControl(layout = "Vector2")]
        public int MouseVector;
 
        public override Vector2 ReadValue(ref InputBindingCompositeContext context)
        {
            if (context.ReadValueAsButton(MouseButton))
                return context.ReadValue<Vector2,Vector2MagnitudeComparer>(MouseVector); //hack
         
            return default;
        }
 
        public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
        {
            return ReadValue(ref context).magnitude;
        }
 
        static ClickAndDragBinding()
        {
            InputSystem.RegisterBindingComposite<ClickAndDragBinding>();
        }
 
        [RuntimeInitializeOnLoadMethod]
        static void Init() { } // Trigger static constructor.
    }
}