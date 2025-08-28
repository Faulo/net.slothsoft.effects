using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Slothsoft.Effects.Editor {
    sealed class EffectElement : VisualElement {
        readonly Label header;
        readonly VisualElement body;

        public EffectElement() {
            style.marginTop = EditorGUIUtility.standardVerticalSpacing;

            header = new Label() {
                style = {
                        unityFontStyleAndWeight = FontStyle.Bold,
                        flexGrow = 1,
                    }
            };

            body = new() {
                style = {
                    paddingLeft = EditorGUIUtility.singleLineHeight,
                }
            };

            Add(header);
            Add(body);
        }

        PropertyField effectField;
        Type effectType;

        public void Unbind() {
            effectField?.Unbind();
        }

        public void Bind(SerializedProperty property) {
            Unbind();

            if (property.managedReferenceValue is IEffect effect) {
                if (effectType != effect.GetType()) {
                    RebuildProperty(property, effect);
                }

                effectField.BindProperty(property);
            }
        }

        void RebuildProperty(SerializedProperty property, IEffect effect) {
            body.Clear();
            effectField = new PropertyField(property, effect.GetType().Name);
            effectType = effect.GetType();
            body.Add(effectField);
        }
    }
}
