﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace eyediseases
{
    /**
     * Unity Editor UI interface.
     * 
     * Adds a menu for adding the color blindness component to the selected object/camera
     * and shows the components settings in the inspector.
     */
    [CustomEditor(typeof(ColorBlindnessSimulator), true)]
    public class ColorBlindnessSimulatorEditor : Editor
    {
        private ColorBlindnessSimulator Target {
            get {
                if (__Target == null)
                    __Target = (ColorBlindnessSimulator)target;
                return __Target;
            }
        }

        private ColorBlindnessSimulator __Target;
    
        public override void OnInspectorGUI ()
        {
            Debug.Log ("ColorBlindnessSimulatorEditor::OnInspectorGUI");

            Target.BlindAlgorithm = (ColorBlindnessSimulator.ColorBlindAlgorithm)EditorGUILayout.EnumPopup ("Color blind algorithm:", Target.BlindAlgorithm);
            Target.BlindMode = (ColorBlindnessSimulator.ColorBlindMode)EditorGUILayout.EnumPopup ("Color Blind Type:", Target.BlindMode);
            switch (Target.BlindMode) {
            case ColorBlindnessSimulator.ColorBlindMode.Protanope:
                GUILayout.BeginVertical ("box");
                GUILayout.Label ("Protanope(Type I):\nRed Weakness.\n1% of males, 0.01% of females.");
                GUILayout.EndVertical ();
                break;
            case ColorBlindnessSimulator.ColorBlindMode.Deuteranope:
                GUILayout.BeginVertical ("box");
                GUILayout.Label ("Deuteranope(Type II):\nGreen Weakness.\nmost common—6% of males, 0.4% of females");
                GUILayout.EndVertical ();
                break;
            default:
                break;
            }
            

            Target.BlindIntensity = EditorGUILayout.Slider ("Simulate Intensity", Target.BlindIntensity, 0.0f, 1.0f);

            if (GUI.changed) {
                EditorUtility.SetDirty (Target);
            }
        }

        //Menu Item
        [MenuItem("GULTI/Attach Color Blindness Simulator")]
        public static void AttachCBSimulatorFromMenu ()
        {
            var sel = Selection.activeGameObject;
            if (sel != null)
                sel.AddComponent<ColorBlindnessSimulator> ();
            else
                Debug.LogError ("Select any GameObject.");
        }
    }
}