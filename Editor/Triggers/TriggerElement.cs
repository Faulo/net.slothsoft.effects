using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Slothsoft.Effects.Editor.Triggers {
    sealed class TriggerElement : VisualElement {
        readonly PropertyField callbackField;

        public TriggerElement(string label, Action onRemove) {
            style.marginTop = EditorGUIUtility.standardVerticalSpacing;

            var header = new VisualElement {
                style = {
                        flexDirection = FlexDirection.Row,
                    }
            };

            var eventLabel = new Label(label) {
                style = {
                        unityFontStyleAndWeight = FontStyle.Bold,
                        flexGrow = 1,
                    }
            };

            var removeButton = new Button(() => onRemove?.Invoke());
            removeButton.Add(new Image { image = EffectUtils.iconMinus });

            header.Add(eventLabel);
            header.Add(removeButton);

            callbackField = new PropertyField {
                style = {
                    paddingLeft = EditorGUIUtility.singleLineHeight,
                },
            };

            Add(header);
            Add(callbackField);

            Hide();
        }

        void Hide() {
            style.display = DisplayStyle.None;
        }

        void Show() {
            style.display = DisplayStyle.Flex;
        }

        public void Unbind() {
            callbackField.Unbind();

            Hide();
        }

        public void Bind(SerializedProperty property) {
            callbackField.Unbind();
            callbackField.BindProperty(property);

            Show();
        }
    }
}
