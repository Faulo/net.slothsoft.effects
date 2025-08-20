using NUnit.Framework;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace Slothsoft.Effects.Editor {
    [CustomPropertyDrawer(typeof(EffectEvent))]
    sealed class EffectEventDrawer : PropertyDrawer {
        static float lineHeight => EditorGUIUtility.singleLineHeight;
        static float spacing => EditorGUIUtility.standardVerticalSpacing;

        ReorderableList effectsList;

        void UpdateEffectsList(SerializedProperty property, GUIContent label) {
            label.text += $" ({nameof(EffectEvent)})";

            if (effectsList is null) {
                var effectsProperty = property.FindPropertyRelative(nameof(EffectEvent.effects));

                Assert.IsNotNull(effectsProperty, $"Failed to find '{nameof(EffectEvent.effects)}'");

                effectsList = new ReorderableList(property.serializedObject, effectsProperty) {
                    drawHeaderCallback = (Rect rect) => {
                        EditorGUI.LabelField(rect, label);
                    },
                    elementHeightCallback = (int index) => {
                        var property = effectsProperty.GetArrayElementAtIndex(index);
                        int startingDepth = property.depth;

                        float height = lineHeight;
                        bool enterChildren = true;
                        while (property.NextVisible(enterChildren) && property.depth > startingDepth) {
                            enterChildren = false;
                            height += EditorGUI.GetPropertyHeight(property);
                        }

                        return height;
                    },
                    drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                        var property = effectsProperty.GetArrayElementAtIndex(index);
                        int startingDepth = property.depth;

                        string name = property.managedReferenceFullTypename.Split('.')[^1];

                        EditorGUI.BeginProperty(rect, new(name), property);

                        rect.height = lineHeight;
                        EditorGUI.indentLevel = property.depth;
                        EditorGUI.LabelField(rect, name, EditorStyles.boldLabel);
                        rect.y += rect.height;

                        bool enterChildren = true;
                        while (property.NextVisible(enterChildren) && property.depth > startingDepth) {
                            enterChildren = false;
                            rect.height = EditorGUI.GetPropertyHeight(property);
                            EditorGUI.indentLevel = property.depth;
                            EditorGUI.PropertyField(rect, property);
                            rect.y += rect.height;
                        }

                        EditorGUI.EndProperty();
                    },
                    onAddDropdownCallback = (_, _) => EffectUtils.CreateAddEffectMenu(effectsProperty),
                    elementHeight = lineHeight + spacing,
                };
            }
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(rect, label, property);

            UpdateEffectsList(property, label);

            effectsList.DoLayoutList();

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return 0;
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property) => new EffectEventElement(property);
    }
}