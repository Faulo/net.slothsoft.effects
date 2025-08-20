using System;
using System.Collections.Generic;
using System.Linq;
using Slothsoft.Effects.Triggers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UEditor = UnityEditor.Editor;

namespace Slothsoft.Effects.Editor.Triggers {
    /// <summary>
    /// UI Toolkit Port des IMGUI-Editors aus EventTriggerEditor.
    /// Zeigt die vorhandenen Entries mit Callback an und erlaubt das Hinzufügen weiterer Event-Typen.
    /// </summary>
    [CustomEditor(typeof(EffectTriggerBase<>), true)]
    sealed class EffectTriggerEditorUITK : UEditor {
        sealed class TriggerElement : VisualElement {
            readonly PropertyField callbackField;

            public TriggerElement(string label, Action onRemove) {
                style.marginTop = EditorGUIUtility.singleLineHeight * 0.5f;

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

                var removeButton = new Button { };
                removeButton.Add(new Image { image = iconMinus });

                removeButton.RegisterCallback<ClickEvent>(_ => onRemove?.Invoke());

                header.Add(eventLabel);
                header.Add(removeButton);

                callbackField = new PropertyField { };

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

        static Texture iconMinus => EditorGUIUtility.IconContent("Toolbar Minus").image;

        SerializedProperty entriesProperty => serializedObject.FindProperty(nameof(PointerEffectTrigger.entries));

        bool TryGetEntry(int type, out ushort index, out SerializedProperty property) {
            for (index = 0; index < entriesProperty.arraySize; index++) {
                property = entriesProperty.GetArrayElementAtIndex(index);
                if (property.FindPropertyRelative(nameof(PointerEffectTrigger.Entry.eventID)).enumValueIndex == type) {
                    return true;
                }
            }

            property = default;
            return false;
        }

        Dictionary<int, string> _eventTypes;
        Dictionary<int, TriggerElement> _eventElements;

        Type triggerEnumType => target.GetType().BaseType.GenericTypeArguments[0];

        void OnEnable() {
            _eventTypes = Enum
                .GetValues(triggerEnumType)
                .Cast<int>()
                .ToDictionary(i => i, i => Enum.GetName(triggerEnumType, i));
        }

        public override VisualElement CreateInspectorGUI() {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _eventElements = new();

            foreach ((int type, string name) in _eventTypes) {
                var trigger = new TriggerElement(name, () => RemoveTrigger(type));

                if (TryGetEntry(type, out _, out var delegateProperty)) {
                    var callbackProperty = delegateProperty.FindPropertyRelative(nameof(PointerEffectTrigger.Entry.callback));
                    trigger.Bind(callbackProperty);
                }

                _eventElements[type] = trigger;

                root.Add(trigger);
            }

            var addButton = new Button(() => ShowAddTriggerMenu()) {
                text = $"Add {triggerEnumType.Name}"
            };
            addButton.style.alignSelf = Align.Center;
            addButton.style.width = 200;

            root.Add(addButton);

            return root;
        }

        void RemoveTrigger(int type) {
            if (TryGetEntry(type, out ushort delegateIndex, out _)) {
                serializedObject.Update();
                entriesProperty.DeleteArrayElementAtIndex(delegateIndex);
                serializedObject.ApplyModifiedProperties();

                _eventElements[type].Unbind();
            }
        }

        void ShowAddTriggerMenu() {
            var menu = new GenericMenu();

            foreach ((int type, string name) in _eventTypes) {
                if (TryGetEntry(type, out _, out _)) {
                    menu.AddDisabledItem(new GUIContent(name));
                } else {
                    menu.AddItem(new GUIContent(name), false, () => AddTrigger(type));
                }
            }

            menu.ShowAsContext();
        }

        void AddTrigger(int type) {
            entriesProperty.arraySize++;

            var entry = entriesProperty.GetArrayElementAtIndex(entriesProperty.arraySize - 1);

            var eventProp = entry.FindPropertyRelative(nameof(PointerEffectTrigger.Entry.eventID));
            eventProp.enumValueIndex = type;

            var callbackProp = entry.FindPropertyRelative(nameof(PointerEffectTrigger.Entry.callback));
            callbackProp.managedReferenceValue = new EffectEvent();

            serializedObject.ApplyModifiedProperties();

            _eventElements[type].Bind(callbackProp);
        }
    }
}
