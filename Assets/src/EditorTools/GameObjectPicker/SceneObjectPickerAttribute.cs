using System;

namespace JamalArouna.EditorTools
{
    /// <summary>
    /// A custom attribute drawer.
    /// This attribute can be used on GameObjects, MonoBehaviours, and Components,  
    /// as well as on classes that inherit from these.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SceneObjectPickerAttribute : Attribute { }   
}