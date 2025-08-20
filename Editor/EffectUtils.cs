using System.Collections.Generic;
using Slothsoft.UnityExtensions;
using UnityEditor;
using UnityEngine;

namespace Slothsoft.Effects.Editor {
    static class EffectUtils {
        internal static Texture iconMinus => EditorGUIUtility.IconContent("Toolbar Minus").image;
        internal static Texture iconPlus => EditorGUIUtility.IconContent("Toolbar Plus").image;

        static readonly ImplementationLocator<IEffect> locator = new();
        static IReadOnlyList<Implementation<IEffect>> creators => locator.implementations;

        internal static void CreateAddEffectMenu(SerializedProperty effectsProperty) {
            var menu = new GenericMenu();
            if (creators.Count == 0) {
                menu.AddDisabledItem(new($"Implement {typeof(IEffect)} and register via {typeof(ImplementationForAttribute)}!"));
            }

            foreach (var creator in creators) {
                menu.AddItem(
                    new(creator.label),
                    false,
                    () => {
                        int index = effectsProperty.arraySize;
                        effectsProperty.arraySize++;
                        var element = effectsProperty.GetArrayElementAtIndex(index);
                        element.managedReferenceValue = creator.CreateInstance();
                        effectsProperty.serializedObject.ApplyModifiedProperties();
                    }
                );
            }

            menu.ShowAsContext();
        }
    }
}