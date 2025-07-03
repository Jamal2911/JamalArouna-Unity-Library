using UnityEditor;
using UnityEngine;

#if ODIN_INSPECTOR 
using Sirenix.OdinInspector.Editor;

namespace JamalArouna.EditorTools
{
    public class SceneObjectPickerDrawer<T> : OdinAttributeDrawer<SceneObjectPickerAttribute, T> where T : class
    {
        private Texture2D pickerIcon;
        private GameObject hoveredObject;
        private bool isPicking;

        private const string ExtensionPath = "Assets/GameContent/Scripts/Utils/Extensions/" + "Extension_GameObjectPicker/";
        private const int ModeTitleSize = 18;
        private const int HoveredTextSize = 15;
        private const int ParentSearchLevels = 5;

        /// <summary>
        /// Draws the property layout for the SceneObjectPicker.
        /// </summary>
        /// <param name="label">The label of the property.</param>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (label == null)
                label = new GUIContent(Property.ValueEntry.TypeOfValue.Name);
            
            if (typeof(GameObject).IsAssignableFrom(Property.ValueEntry.TypeOfValue) || typeof(MonoBehaviour).IsAssignableFrom(Property.ValueEntry.TypeOfValue))
            {
                EditorGUILayout.BeginHorizontal();
                Property.ValueEntry.WeakSmartValue = EditorGUILayout.ObjectField(label, (dynamic)Property.ValueEntry.WeakSmartValue, typeof(GameObject), true);
        
                pickerIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(ExtensionPath + "PickerIcon.png");
        
                GUIContent buttonContent = isPicking ? new GUIContent("X", "Cancel Picking Mode") : new GUIContent(pickerIcon, "Pick Object");
                if (GUILayout.Button(buttonContent, GUILayout.Width(50), GUILayout.Height(20)))
                    if (isPicking)
                        StopPicking();
                    else
                        StartPicking();
        
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                GUIStyle style = new GUIStyle { normal = {textColor = Color.red}};
                EditorGUILayout.LabelField("Invalid type. Expected: GameObject or MonoBehaviour. (SceneObjectPicker)", style);
            }
        }
    
    /// <summary>
    /// Starts the object picking mode. The SceneView is updated to detect objects.
    /// </summary>
    private void StartPicking()
    {
        if (!isPicking)
        {
            hoveredObject = null;
            isPicking = true;
            SceneView.duringSceneGui += OnSceneGUI;
            GlobalKeyEventHandler.OnKeyEvent += handleKeyPressEvents;
            SceneView.RepaintAll();
        }
    }

    /// <summary>
    /// Stops the object picking mode and removes the SceneView callback.
    /// </summary>
    private void StopPicking()
    {
        if (isPicking)
        {
            isPicking = false;
            SceneView.duringSceneGui -= OnSceneGUI;
            GlobalKeyEventHandler.OnKeyEvent -= handleKeyPressEvents;
            SceneView.RepaintAll();   
            hoveredObject = null;
        }
    }

    private void handleKeyPressEvents(Event current)
    {
        if (current.type == EventType.KeyDown && current.keyCode == KeyCode.Escape)
        {
            StopPicking();
            current.Use();
        }
    }
    
    /// <summary>
    /// Handles drawing and interactions during the object picking mode in the SceneView.
    /// </summary>
    /// <param name="sceneView">The SceneView where the interactions take place.</param>
    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (isPicking)
        {
            DrawText(sceneView);
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            
            if(e.isMouse)
                hoveredObject = HandleUtility.PickGameObject(e.mousePosition, false);
                
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                GameObject pickedObject = HandleUtility.PickGameObject(e.mousePosition, false);

                if (pickedObject)
                {
                    if (Property.ValueEntry.TypeOfValue == typeof(GameObject))
                    {
                        Debug.Log($"Picked: '<b><color=yellow>{pickedObject.name}</b></color>'");
                        Property.ValueEntry.WeakSmartValue = pickedObject;
                    }
                    else
                    {
                        bool found = false;
                        bool IsValidComponent(Component component) => Property.Info.TypeOfValue.IsAssignableFrom(component.GetType());
                        
                        //Children Search
                        Component[] components = pickedObject.GameObject().GetComponentsInChildren<Component>();
                        foreach (var component in components)
                        {
                            if (IsValidComponent(component))
                            {
                                Debug.Log($"Picked: '<b><color=yellow>{pickedObject.name}</b></color>'");
                                Property.ValueEntry.WeakSmartValue = component;
                                found = true;
                                break;
                            }
                        }
                        
                        //Parent Search
                        if (!found)
                        {
                            Transform parent = pickedObject.GameObject().transform.parent;
                            int level = 0;

                            while (parent != null && level < ParentSearchLevels)
                            {
                                components = parent.GetComponents<Component>();

                                foreach (var component in components)
                                {
                                    if (IsValidComponent(component))
                                    {
                                        Debug.Log($"Picked: '<b><color=yellow>{parent.name}</b></color>'");
                                        Property.ValueEntry.WeakSmartValue = component;
                                        found = true;
                                        break;
                                    }
                                }

                                if (found) break;
                                parent = parent.parent;
                                level++;
                            }
                        }

                        if (!found)
                        {
                            Debug.Log("<color=red>No compatible component found.</color>");
                        }
                    }
                }
                e.Use();
                StopPicking();
            }
        }
    }
    

    /// <summary>
    /// Draws additional text in the SceneView, such as the mode title and the name of the currently hovered object.
    /// </summary>
    /// <param name="sceneView">The SceneView where the text is displayed.</param>
    private void DrawText(SceneView sceneView)
    {
        // Mode Title
        Handles.BeginGUI();
        Vector2 size = sceneView.position.size;
        
        const float width = 200;
        const float height = 30;
        float x = (size.x - width) / 2;
        const float y = 10;

        Rect topCenterRect = new Rect(x, y, width, height); 
        GUIStyle style = new GUIStyle
        {
            normal = {textColor = Color.green},
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = ModeTitleSize
        };
        GUI.Label(topCenterRect, "Object Picker Mode" + "\n" + $"({Property.Info.TypeOfValue.Name})", style);
        
        // Mouse Hover
        Vector2 mousePos = Event.current.mousePosition;
        Vector2 offset = new Vector2(15, -15);
        Vector2 rectSize = new Vector2(500, 30);
        Rect hoverRect = new Rect(mousePos + offset, rectSize);
        
        GUIStyle styleHovered = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
            fontSize = HoveredTextSize
        };

        if (hoveredObject)
        {
            bool isCompatible = false;
            string labelText = "None";
            
            bool IsValidComponent(Component component) => Property.Info.TypeOfValue.IsAssignableFrom(component.GetType());
            
            if (Property.Info.TypeOfValue == typeof(GameObject))
            {
                styleHovered.normal.textColor = Color.green;
                labelText = hoveredObject.name;
                isCompatible = true;
            }
            else
            {
                styleHovered.normal.textColor = Color.red;

                //Children Search
                bool found = false;
                Component[] components = hoveredObject.GameObject().GetComponentsInChildren<Component>();
                foreach (var component in components)
                {
                    if (IsValidComponent(component))
                    {
                        styleHovered.normal.textColor = Color.green;
                        labelText = hoveredObject.name;
                        isCompatible = true;
                        found = true;
                        break;
                    }
                }

                //Parent Search
                if (!found)
                {
                    Transform parent = hoveredObject.GameObject().transform.parent;
                    int level = 0;

                    while (parent != null && level < ParentSearchLevels)
                    {
                        components = parent.GetComponents<Component>();

                        foreach (var component in components)
                        {
                            if (IsValidComponent(component))
                            {
                                styleHovered.normal.textColor = Color.green;
                                labelText = parent.name;
                                isCompatible = true;
                                found = true;
                                break;
                            }
                        }

                        if (found) break;

                        parent = parent.parent;
                        level++;
                    }
                }
            }
            
            if (!isCompatible)
            {
                styleHovered.normal.textColor = Color.red;
                GUI.Label(hoverRect, "<color=red>Incompatible Type</color>", styleHovered);
            }
            else 
                GUI.Label(hoverRect, labelText, styleHovered);
        }
        else
            GUI.Label(hoverRect, "<color=red>None</color>\"", styleHovered);

        Handles.EndGUI();
        }
    }   
}
#endif