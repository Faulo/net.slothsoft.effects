using System;
using Slothsoft.Effects.Triggers;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UEditor = UnityEditor.Editor;

namespace Slothsoft.Effects.Editor.Triggers {
    /// <summary>
    /// <seealso cref="UnityEditor.EventSystems.EventTriggerEditor"/>
    /// </summary>
    [CustomEditor(typeof(EventSystemEffectTrigger))]
    sealed class EventSystemEffectTriggerEditor : UEditor {
        SerializedProperty m_DelegatesProperty;

        GUIContent m_IconToolbarMinus;
        GUIContent m_EventIDName;
        GUIContent[] m_EventTypes;
        GUIContent m_AddButonContent;

        void OnEnable() {
            m_DelegatesProperty = serializedObject.FindProperty(nameof(EventSystemEffectTrigger.entries));
            m_AddButonContent = EditorGUIUtility.TrTextContent("Add New Event Type");
            m_EventIDName = new GUIContent("");
            // Have to create a copy since otherwise the tooltip will be overwritten.
            m_IconToolbarMinus = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus")) {
                tooltip = "Remove all events in this list."
            };

            string[] eventNames = Enum.GetNames(typeof(EventTriggerType));
            m_EventTypes = new GUIContent[eventNames.Length];
            for (int i = 0; i < eventNames.Length; ++i) {
                m_EventTypes[i] = new GUIContent(eventNames[i]);
            }
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            int toBeRemovedEntry = -1;

            EditorGUILayout.Space();

            var removeButtonSize = GUIStyle.none.CalcSize(m_IconToolbarMinus);

            for (int i = 0; i < m_DelegatesProperty.arraySize; ++i) {
                var delegateProperty = m_DelegatesProperty.GetArrayElementAtIndex(i);
                var eventProperty = delegateProperty.FindPropertyRelative(nameof(EventSystemEffectTrigger.Entry.eventID));
                var callbacksProperty = delegateProperty.FindPropertyRelative(nameof(EventSystemEffectTrigger.Entry.callback));
                m_EventIDName.text = eventProperty.enumDisplayNames[eventProperty.enumValueIndex];

                EditorGUILayout.PropertyField(callbacksProperty, m_EventIDName);
                var callbackRect = GUILayoutUtility.GetLastRect();

                var removeButtonPos = new Rect(callbackRect.xMax - removeButtonSize.x - 8, callbackRect.y + 1, removeButtonSize.x, removeButtonSize.y);
                if (GUI.Button(removeButtonPos, m_IconToolbarMinus, GUIStyle.none)) {
                    toBeRemovedEntry = i;
                }

                EditorGUILayout.Space();
            }

            if (toBeRemovedEntry > -1) {
                RemoveEntry(toBeRemovedEntry);
            }

            var btPosition = GUILayoutUtility.GetRect(m_AddButonContent, GUI.skin.button);
            const float addButonWidth = 200f;
            btPosition.x += (btPosition.width - addButonWidth) / 2;
            btPosition.width = addButonWidth;
            if (GUI.Button(btPosition, m_AddButonContent)) {
                ShowAddTriggermenu();
            }

            serializedObject.ApplyModifiedProperties();
        }

        void RemoveEntry(int toBeRemovedEntry) {
            m_DelegatesProperty.DeleteArrayElementAtIndex(toBeRemovedEntry);
        }

        void ShowAddTriggermenu() {
            // Now create the menu, add items and show it
            var menu = new GenericMenu();
            for (int i = 0; i < m_EventTypes.Length; ++i) {
                bool active = true;

                // Check if we already have a Entry for the current eventType, if so, disable it
                for (int p = 0; p < m_DelegatesProperty.arraySize; ++p) {
                    var delegateEntry = m_DelegatesProperty.GetArrayElementAtIndex(p);
                    var eventProperty = delegateEntry.FindPropertyRelative(nameof(EventSystemEffectTrigger.Entry.eventID));
                    if (eventProperty.enumValueIndex == i) {
                        active = false;
                    }
                }

                if (active) {
                    menu.AddItem(m_EventTypes[i], false, OnAddNewSelected, i);
                } else {
                    menu.AddDisabledItem(m_EventTypes[i]);
                }
            }

            menu.ShowAsContext();
            Event.current.Use();
        }

        void OnAddNewSelected(object index) {
            int selected = (int)index;

            m_DelegatesProperty.arraySize += 1;
            var delegateEntry = m_DelegatesProperty.GetArrayElementAtIndex(m_DelegatesProperty.arraySize - 1);
            var eventProperty = delegateEntry.FindPropertyRelative(nameof(EventSystemEffectTrigger.Entry.eventID));
            eventProperty.enumValueIndex = selected;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
