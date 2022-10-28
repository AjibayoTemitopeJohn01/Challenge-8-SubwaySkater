using UnityEngine;
using UnityEditor;

/*
 * 
 *  Transform Colours
 *							  
 *	Transform Component Inspector Editor
 *      The editor script that edits the look of the transform component.
 *			
 *  Warning:
 *	    Please refrain from editing this script as it will cause issues to the asset...
 *			
 *  Written by:
 *      Jonathan Carter
 *
 *  Published By:
 *      Carter Games
 *      E: hello@carter.games
 *      W: https://www.carter.games
 *		
 *  Version: 1.1.6
 *	Last Updated: 11/09/2021 (d/m/y)							
 * 
 */

namespace CarterGames.Assets.TransformColours
{
    /// <summary>
    /// Editor Class - Controls the custom inspector for the transform component, just comment out or remove the script to get the normal inspector back.
    /// </summary>
    [CanEditMultipleObjects, CustomEditor(typeof(Transform))]
    public class TransformColours : Editor
    {
        /// <summary>
        /// A reference to the T component
        /// </summary>
        private Transform _t;

        // Hidden Colour
        private Color _hidden = new Color(0, 0, 0, .3f);

        // Bool values for if something has changed
        private bool _posChange;
        private bool _rotChange;
        private bool _scaleChange;
            

        // Vector3 Values for initial ( Position / Rotation / Scale ) Values
        private Vector3 _initPos;
        private Vector3 _initRot;
        private Vector3 _initScale;


        private SerializedProperty posX;
        private SerializedProperty posY;
        private SerializedProperty posZ;
        
        private SerializedProperty scaleX;
        private SerializedProperty scaleY;
        private SerializedProperty scaleZ;
        
#if UNITY_2021_2_OR_NEWER
        private SerializedProperty scaleConstrain;
#endif
     

        /// <summary>
        /// Shows the Menu item that changes between 2D&3D view on the inspector
        /// </summary>
        [MenuItem("Tools/Transform Colours | CG/Switch 2D-3D View", priority = 2)]
        public static void ToggleTwoD()
        {
            if (EditorPrefs.GetBool("CarterGames-TransformColours-Use2D"))
            {
                EditorPrefs.SetBool("CarterGames-TransformColours-Use2D", false);
            }
            else
            {
                EditorPrefs.SetBool("CarterGames-TransformColours-Use2D", true);
            }

            TransformColours _temp = new TransformColours();
            _temp.UpdateInspector();
        }


        private void OnEnable()
        {
            posX = serializedObject.FindProperty("m_LocalPosition.x");
            posY = serializedObject.FindProperty("m_LocalPosition.y");
            posZ = serializedObject.FindProperty("m_LocalPosition.z");
            
            scaleX = serializedObject.FindProperty("m_LocalScale.x");
            scaleY = serializedObject.FindProperty("m_LocalScale.y");
            scaleZ = serializedObject.FindProperty("m_LocalScale.z");
            
#if UNITY_2021_2_OR_NEWER
            scaleConstrain = serializedObject.FindProperty("m_ConstrainProportionsScale");
#endif
        }


        /// <summary>
        /// Overrides the default inspector with the one made in this script...
        /// </summary>
        public override void OnInspectorGUI()
        {
            _t = targets[0] as Transform;          // Assigns the T componenet to the T Variable


            _posChange = false;              // Setting the Position change to be false ready for a fresh check
            _rotChange = false;              // Setting the Rotation change to be false ready for a fresh check
            _scaleChange = false;            // Setting the Scale change to be false ready for a fresh check

            _initPos = _t.localPosition;      // Setting the Initial Position of the Transform to what the current local position is
            _initRot = _t.localEulerAngles;   // Setting the Initial Rotation of the Transform to what the current local rotation is
            _initScale = _t.localScale;       // Setting the Initial Scale of the Transform to what the current local scale is


            PositionInspector();
            RotationInspector();
            ScaleInspector();

            ApplyChanges();
        }

        /// <summary>
        /// Defines the position elements in the inspector.
        /// </summary>
        private void PositionInspector()
        {
            // Position Label - Adjusts to whether or not the inspector is in wide mode
            if (EditorGUIUtility.wideMode)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Position", GUILayout.MinWidth(90), GUILayout.ExpandHeight(false));
            }
            else
            {
                GUILayout.Label("Position", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
            }
            
            // Only in 2021.2 or newer - Adjusts the layout to make it all match with the scale constrain in 2021.2...
#if UNITY_2021_2_OR_NEWER
            if (GUILayout.Button(GUIContent.none, GUIStyle.none, GUILayout.Width(17.5f))){}
#endif

            // Making the Position Vector3 Boxes in their colours 
            EditorGUIUtility.labelWidth = 15;
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

            // Position X - Red
            GUI.backgroundColor = Color.red;
            EditorGUILayout.PropertyField(posX, new GUIContent("X"));
            GUI.backgroundColor = Color.white;

            // Position Y - Green
            GUI.backgroundColor = Color.green;
            EditorGUILayout.PropertyField(posY, new GUIContent("Y"));
            GUI.backgroundColor = Color.white;


            if (EditorPrefs.GetBool("CarterGames-TransformColours-Use2D"))
            {
                // Position Z - Blue
                GUI.backgroundColor = _hidden;
                EditorGUILayout.PropertyField(posZ, new GUIContent("Z"));
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                // Position Z - Blue
                GUI.backgroundColor = Color.blue;
                SerializedProperty posZ = serializedObject.FindProperty("m_LocalPosition.z");
                EditorGUILayout.PropertyField(posZ, new GUIContent("Z"));
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
            }

            // Runs if a edit was made to one of the fields above
            if (EditorGUI.EndChangeCheck())
            {
                _posChange = true;
            }

            // Adjusts the editor Hoz grouping so the label shows above the boxes if the inspector is not in wide mode
            if (EditorGUIUtility.wideMode)
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// Defines the rotation elements in the inspector.
        /// </summary>
        private void RotationInspector()
        {
            // Rotation Label - Adjusts to whether or not the inspector is in wide mode
            if (EditorGUIUtility.wideMode)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Rotation", GUILayout.MinWidth(90), GUILayout.ExpandHeight(false));
            }
            else
            {
                GUILayout.Label("Rotation", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
            }

            
            // Only in 2021.2 or newer - Adjusts the layout to make it all match with the scale constrain in 2021.2...
#if UNITY_2021_2_OR_NEWER
            if (GUILayout.Button(GUIContent.none, GUIStyle.none, GUILayout.Width(17.5f))){}
#endif
            
            
            // Making the  RotationVector3 Boxes in their colours 
            EditorGUIUtility.labelWidth = 15;
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();


            // Hotfix 2 - try to stop the 90/270 issue
            Vector3 _newRot = TransformUtils.GetInspectorRotation(_t);

            if (EditorPrefs.GetBool("CarterGames-TransformColours-Use2D"))
            {
                // Rotation X - Red
                GUI.backgroundColor = _hidden;
                _newRot.x = EditorGUILayout.FloatField(new GUIContent("X"), _newRot.x, GUILayout.Width(70), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));

                // Rotation Y - Green
                _newRot.y = EditorGUILayout.FloatField(new GUIContent("Y"), _newRot.y, GUILayout.Width(70), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
                GUI.backgroundColor = Color.white;

                // Rotation Z - Blue
                GUI.backgroundColor = Color.blue;
                _newRot.z = EditorGUILayout.FloatField(new GUIContent("Z"), _newRot.z, GUILayout.Width(70), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
                GUI.backgroundColor = Color.white;
            }
            else
            {
                // Rotation X - Red
                GUI.backgroundColor = Color.red;
                _newRot.x = EditorGUILayout.FloatField(new GUIContent("X"), _newRot.x, GUILayout.Width(70), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
                GUI.backgroundColor = Color.white;

                // Rotation Y - Green
                GUI.backgroundColor = Color.green;
                _newRot.y = EditorGUILayout.FloatField(new GUIContent("Y"), _newRot.y, GUILayout.Width(70), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
                GUI.backgroundColor = Color.white;

                // Rotation Z - Blue
                GUI.backgroundColor = Color.blue;
                _newRot.z = EditorGUILayout.FloatField(new GUIContent("Z"), _newRot.z, GUILayout.Width(70), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
                GUI.backgroundColor = Color.white;
            }


            EditorGUILayout.EndHorizontal();


            // Hotfix 2 - try to stop the 90/270 issue
            TransformUtils.SetInspectorRotation(_t, _newRot);


            // Runs if a edit was made to one of the fields above
            if (EditorGUI.EndChangeCheck())
            {
                _rotChange = true;
            }


            // Adjusts the editor Hoz grouping so the label shows above the boxes if the inspector is not in wide mode
            if (EditorGUIUtility.wideMode)
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// Defines the scale elements in the inspector.
        /// </summary>
        private void ScaleInspector()
        {
            // Scale Label - Adjusts to whether or not the inspector is in wide mode
            if (EditorGUIUtility.wideMode)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Scale", GUILayout.MinWidth(90), GUILayout.ExpandHeight(false));
            }
            else
            {
                GUILayout.Label("Scale", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
            }
            
            
            // Scale Constrain (Only in 2021.2 or newer)
#if UNITY_2021_2_OR_NEWER
            EditorGUILayout.PropertyField(scaleConstrain, GUIContent.none, GUILayout.Width(17.5f));
#endif
            
            // Making the Position Vector3 Boxes in their colours 
            EditorGUIUtility.labelWidth = 15;
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

            // Scale X - Red
            GUI.backgroundColor = Color.red;
            EditorGUILayout.PropertyField(scaleX, new GUIContent("X"));
            GUI.backgroundColor = Color.white;

            // Scale Y - Green
            GUI.backgroundColor = Color.green;
            EditorGUILayout.PropertyField(scaleY, new GUIContent("Y"));
            GUI.backgroundColor = Color.white;


            if (EditorPrefs.GetBool("CarterGames-TransformColours-Use2D"))
            {
                // Scale Z - Blue
                GUI.backgroundColor = _hidden;
                EditorGUILayout.PropertyField(scaleZ, new GUIContent("Z"));
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                // Scale Z - Blue
                GUI.backgroundColor = Color.blue;
                SerializedProperty scaleZ = serializedObject.FindProperty("m_LocalScale.z");
                EditorGUILayout.PropertyField(scaleZ, new GUIContent("Z"));
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
            }

            // Runs if a edit was made to one of the fields above
            if (EditorGUI.EndChangeCheck())
            {
                _scaleChange = true;
            }

            // Adjusts the editor Hoz grouping so the label shows above the boxes if the inspector is not in wide mode
            if (EditorGUIUtility.wideMode)
            {
                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        /// Updates the inspector when called.
        /// </summary>
        private void UpdateInspector()
        {
            Repaint();
        }

        /// <summary>
        /// Does the colour change for each box as well as detects changes in each ( Position / Rotation / Scale ) - ( X / Y / Z ) 
        /// </summary>
        private void ApplyChanges()
        {
            // Gets all objects currently selected in the editor
            Transform[] SelectedTransforms = Selection.GetTransforms(SelectionMode.Editable);

            // If there is atleast 1 object selected
            if (SelectedTransforms.Length > 1)
            {
                // Go through them all and adjust their values where they have been changed
                for (int i = 0; i < SelectedTransforms.Length; i++)
                {
                    if (_posChange)
                    {
                        SelectedTransforms[i].localPosition = ApplyPositionWhatChange(SelectedTransforms[i].localPosition, _initPos, _t.localPosition);
                    }
                    if (_rotChange)
                    {
                        SelectedTransforms[i].localEulerAngles = ApplyRotationWhatChange(SelectedTransforms[i].localEulerAngles, _initRot, _t.localEulerAngles);
                    }
                    if (_scaleChange)
                    {
                        SelectedTransforms[i].localScale = ApplyScaleWhatChange(SelectedTransforms[i].localScale, _initScale, _t.localScale);
                    }
                }
            }

            // Apply the changes and update the editor (also fixed animation recording problems.... 1.1.3....)
            Undo.RecordObjects(this.targets, "Transform Colours - Transform Changed");
            serializedObject.ApplyModifiedProperties();
            //serializedObject.Update();
        }

        /// <summary>
        /// Updates the position for what values were changed...
        /// </summary>
        /// <param name="ToApply">What value is been changed</param>
        /// <param name="Init">What the value was before it was changed</param>
        /// <param name="Change">What the value is been changed too</param>
        /// <returns></returns>
        private Vector3 ApplyPositionWhatChange(Vector3 ToApply, Vector3 Init, Vector3 Change)
        {
            if (!Mathf.Approximately(Init.x, Change.x))
            {
                ToApply.x = _t.localPosition.x;
            }

            if (!Mathf.Approximately(Init.y, Change.y))
            {
                ToApply.y = _t.localPosition.y;
            }

            if (!Mathf.Approximately(Init.z, Change.z))
            {
                ToApply.z = _t.localPosition.z;
            }

            Undo.RecordObjects(this.targets, "Transform Colours - Position Change");

            return ToApply;
        }

        /// <summary>
        /// Updates the rotation for what values were changed...
        /// </summary>
        /// <param name="ToApply">What value is been changed</param>
        /// <param name="Init">What the value was before it was changed</param>
        /// <param name="Change">What the value is been changed too</param>
        /// <returns></returns>
        private Vector3 ApplyRotationWhatChange(Vector3 ToApply, Vector3 Init, Vector3 Change)
        {
            if (!Mathf.Approximately(Init.x, Change.x))
            {
                ToApply.x = _t.localEulerAngles.x;
            }
            else
            {
                ToApply.x = Init.x;
            }

            if (!Mathf.Approximately(Init.y, Change.y))
            {
                ToApply.y = _t.localEulerAngles.y;
            }
            else
            {
                ToApply.y = Init.y;
            }

            if (!Mathf.Approximately(Init.z, Change.z))
            {
                ToApply.z = _t.localEulerAngles.z;
            }
            else
            {
                ToApply.z = Init.z;
            }

            Undo.RecordObjects(this.targets, "Transform Colours - Rotation Change");

            return ToApply;
        }

        /// <summary>
        /// Updates the scale for what values were changed...
        /// </summary>
        /// <param name="ToApply">What value is been changed</param>
        /// <param name="Init">What the value was before it was changed</param>
        /// <param name="Change">What the value is been changed too</param>
        /// <returns></returns>
        private Vector3 ApplyScaleWhatChange(Vector3 ToApply, Vector3 Init, Vector3 Change)
        {
            if (!Mathf.Approximately(Init.x, Change.x))
            {
                ToApply.x = _t.localScale.x;
            }

            if (!Mathf.Approximately(Init.y, Change.y))
            {
                ToApply.y = _t.localScale.y;
            }

            if (!Mathf.Approximately(Init.z, Change.z))
            {
                ToApply.z = _t.localScale.z;
            }

            Undo.RecordObjects(this.targets, "Transform Colours - Scale Change");

            return ToApply;
        }
    }
}