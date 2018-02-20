using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace ProjectThief.Editor {   

    [CustomEditor(typeof(Guard))]
    public class GuardInspector : UnityEditor.Editor
    {
        private const string Direction = "m_iDirection";


        private SerializedProperty _directionIntProperty;

        public MyDirections _direction;
        [MenuItem("Examples/Editor GUILayout Enum Popup usage")]

        protected void OnEnable()
        {
            _directionIntProperty = serializedObject.FindProperty( Direction );
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //EditorGUILayout.BeginVertical();


            //_direction = (MyDirections)EditorGUILayout.EnumPopup("Direction:", _direction);

            //serializedObject.ApplyModifiedProperties();

            //EditorGUILayout.EndVertical();
        }

        public void Update()
        {
            Debug.Log(_direction);
        }
    }
}
