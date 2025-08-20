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

        public void Unbind() {
            body.Clear();
        }

        public void Bind(SerializedProperty property) {
            Unbind();

            string name = property.managedReferenceFullTypename.Split('.')[^1];
            header.text = name;

            int startingDepth = property.depth;
            bool enterChildren = true;
            while (property.NextVisible(enterChildren) && property.depth > startingDepth) {
                enterChildren = false;
                body.Add(new Label(property.propertyPath));
                body.Add(new PropertyField(property));
            }
        }
    }
}
