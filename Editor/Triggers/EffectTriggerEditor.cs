using System;
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
        SerializedProperty _entriesProperty;

        bool TryGetEntry(int index, out int arrayIndex, out SerializedProperty property) {
            for (arrayIndex = 0; arrayIndex < _entriesProperty.arraySize; arrayIndex++) {
                property = _entriesProperty.GetArrayElementAtIndex(arrayIndex);
                if (property.FindPropertyRelative(nameof(PointerEffectTrigger.Entry.eventID)).enumValueIndex == index) {
                    return true;
                }
            }

            property = default;
            return false;
        }

        (int, string)[] _eventTypes;

        Texture iconMinus => EditorGUIUtility.IconContent("Toolbar Minus").image;
        Type triggerEnumType => target.GetType().BaseType.GenericTypeArguments[0];

        void OnEnable() {
            _entriesProperty = serializedObject.FindProperty(nameof(PointerEffectTrigger.entries));
            _eventTypes = Enum
                .GetValues(triggerEnumType)
                .Cast<int>()
                .Select(i => (i, Enum.GetName(triggerEnumType, i)))
                .ToArray();
        }

        public override VisualElement CreateInspectorGUI() {
            var root = new VisualElement();

            var list = BuildListView();
            root.Add(list);

            var addButton = new Button(() => ShowAddTriggerMenu(list)) {
                text = $"Add {triggerEnumType.Name}"
            };
            addButton.style.alignSelf = Align.Center;
            addButton.style.width = 200;

            root.Add(addButton);

            return root;
        }

        ListView BuildListView() {
            var list = new ListView {
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                reorderable = false,
                selectionType = SelectionType.None,
                itemsSource = _eventTypes,
            };

            list.makeItem = () => {
                var row = new VisualElement { };

                var header = new VisualElement { style = { flexDirection = FlexDirection.Row } };
                var eventLabel = new Label { style = { unityFontStyleAndWeight = FontStyle.Bold, flexGrow = 1 } };

                var removeButton = new Button { };
                removeButton.Add(new Image { image = iconMinus });

                removeButton.RegisterCallback<ClickEvent>(_ => {
                    if (removeButton.userData is int index) {
                        RemoveEntry(index, list);
                    }
                });

                header.Add(eventLabel);
                header.Add(removeButton);

                var callbackField = new PropertyField { };

                row.Add(header);
                row.Add(callbackField);
                return row;
            };

            list.bindItem = (row, index) => {
                var (eventIndex, eventName) = _eventTypes[index];

                var label = row.Q<Label>();
                label.text = eventName;

                var removeButton = row.Q<Button>();
                removeButton.userData = eventIndex;

                var callbackField = row.Q<PropertyField>();
                callbackField.Unbind();

                if (TryGetEntry(eventIndex, out int delegateIndex, out var delegateProperty)) {
                    var callbackProperty = delegateProperty.FindPropertyRelative(nameof(PointerEffectTrigger.Entry.callback));
                    callbackField.BindProperty(callbackProperty);

                    row.style.display = DisplayStyle.Flex;
                    row.visible = true;
                } else {
                    row.style.display = DisplayStyle.None;
                    row.visible = false;
                }
            };

            list.unbindItem = (row, index) => {
                row.Q<PropertyField>().Unbind();
            };

            return list;
        }

        void RemoveEntry(int eventIndex, ListView list) {
            if (TryGetEntry(eventIndex, out int delegateIndex, out _)) {
                serializedObject.Update();
                _entriesProperty.DeleteArrayElementAtIndex(delegateIndex);
                serializedObject.ApplyModifiedProperties();

                list.Rebuild();
            }
        }

        void ShowAddTriggerMenu(ListView list) {
            var menu = new GenericMenu();

            foreach ((int eventIndex, string eventName) in _eventTypes) {
                if (TryGetEntry(eventIndex, out _, out _)) {
                    menu.AddDisabledItem(new GUIContent(eventName));
                } else {
                    menu.AddItem(new GUIContent(eventName), false, () => AddEntry(eventIndex, list));
                }
            }

            menu.ShowAsContext();
        }

        void AddEntry(int eventIndex, ListView list) {
            serializedObject.Update();

            _entriesProperty.arraySize++;

            var entry = _entriesProperty.GetArrayElementAtIndex(_entriesProperty.arraySize - 1);

            var eventProp = entry.FindPropertyRelative(nameof(PointerEffectTrigger.Entry.eventID));
            eventProp.enumValueIndex = eventIndex;

            var callbackProp = entry.FindPropertyRelative(nameof(PointerEffectTrigger.Entry.callback));
            callbackProp.managedReferenceValue = new EffectEvent();

            serializedObject.ApplyModifiedProperties();

            list.RefreshItems();
        }
    }
}
