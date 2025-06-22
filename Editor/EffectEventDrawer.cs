using System.Collections.Generic;
using NUnit.Framework;
using Slothsoft.UnityExtensions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Slothsoft.Effects.Editor {
    [CustomPropertyDrawer(typeof(EffectEvent))]
    sealed class EffectEventDrawer : PropertyDrawer {
        #region EffectCreator
        static readonly ImplementationLocator<IEffect> locator = new();
        static IReadOnlyList<Implementation<IEffect>> creators => locator.implementations;

        static GenericMenu CreateMenu(ReorderableList list) {
            var menu = new GenericMenu();
            if (creators.Count == 0) {
                menu.AddDisabledItem(new($"Implement {typeof(IEffect)} and register via {typeof(ImplementationForAttribute)}!"));
            }

            foreach (var creator in creators) {
                menu.AddItem(
                    new(creator.label),
                    false,
                    () => {
                        int index = list.serializedProperty.arraySize;
                        list.serializedProperty.arraySize++;
                        list.index = index;
                        var element = list.serializedProperty.GetArrayElementAtIndex(index);
                        element.managedReferenceValue = creator.CreateInstance();
                        list.serializedProperty.serializedObject.ApplyModifiedProperties();
                    }
                );
            }

            menu.ShowAsContext();
            return menu;
        }
        #endregion

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        ReorderableList effectsList;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            label.text += $" ({nameof(EffectEvent)})";

            EditorGUI.BeginProperty(rect, label, property);

            EditorGUI.indentLevel = property.depth;

            var effectsProperty = property.FindPropertyRelative(nameof(EffectEvent.effects));

            Assert.IsNotNull(effectsProperty, $"Failed to find '{nameof(EffectEvent.effects)}'");

            effectsList ??= new ReorderableList(property.serializedObject, effectsProperty) {
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
                onAddDropdownCallback = (Rect buttonRect, ReorderableList list) => {
                    var menu = CreateMenu(list);
                    menu.ShowAsContext();
                },
                elementHeight = lineHeight + spacing,
            };

            effectsList.DoLayoutList();

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return lineHeight;
        }
    }
}