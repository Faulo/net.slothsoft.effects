using System.Collections.Generic;
using System.Linq;
using Slothsoft.Effects.ObjectLocators;
using Slothsoft.UnityExtensions;
using UnityEditor;
using UnityEngine;

namespace Slothsoft.Effects.Editor.ObjectLocators {
    [CustomPropertyDrawer(typeof(TransformReference))]
    sealed class TransformReferenceDrawer : PropertyDrawer {
        static readonly ImplementationLocator<ITransformLocator> locator = new();
        static IReadOnlyList<Implementation<ITransformLocator>> locators => locator.implementations;

        ITransformLocator Instantiate(int index) {
            return locators[index].CreateInstance();
        }
        int DetermineType(ITransformLocator instance) {
            for (int i = 0; i < locators.Count; i++) {
                if (locators[i].IsInstanceOfType(instance)) {
                    return i;
                }
            }

            return 0;
        }

        float lineHeight = EditorGUIUtility.singleLineHeight;
        int referenceType = -1;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            label = EditorGUI.BeginProperty(rect, label, property);

            var locatorProperty = GetLocatorProperty(property);

            if (referenceType == -1) {
                if (locatorProperty.managedReferenceValue is ITransformLocator instance) {
                    referenceType = DetermineType(instance);
                } else {
                    referenceType = 0;
                    locatorProperty.managedReferenceValue = Instantiate(referenceType);
                }
            }

            rect.height = lineHeight;
            int newType = EditorGUI.Popup(rect, label, referenceType, locators.Select(l => new GUIContent(l.label)).ToArray());
            rect.y += rect.height;

            if (referenceType != newType) {
                referenceType = newType;
                locatorProperty.managedReferenceValue = Instantiate(referenceType);
            }

            EditorGUI.indentLevel++;
            foreach (var childProperty in GetChildProperties(locatorProperty)) {
                rect.height = EditorGUI.GetPropertyHeight(locatorProperty, false);
                EditorGUI.PropertyField(rect, locatorProperty);
                rect.y += rect.height;
            }

            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float height = lineHeight;

            var locatorProperty = GetLocatorProperty(property);
            foreach (var childProperty in GetChildProperties(locatorProperty)) {
                height += EditorGUI.GetPropertyHeight(childProperty, false);
            }

            return height;
        }

        SerializedProperty GetLocatorProperty(SerializedProperty property) {
            return property.FindPropertyRelative(nameof(TransformReference.locator));
        }

        IEnumerable<SerializedProperty> GetChildProperties(SerializedProperty property) {
            var end = property.GetEndProperty();
            for (bool enterChildren = true; property.NextVisible(enterChildren) && !SerializedProperty.EqualContents(end, property); enterChildren = false) {
                yield return property;
            }
        }
    }
}
