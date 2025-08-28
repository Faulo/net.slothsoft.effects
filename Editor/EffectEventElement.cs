using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Slothsoft.Effects.Editor {
    sealed class EffectEventElement : VisualElement {
        readonly SerializedProperty eventProperty;
        readonly SerializedProperty effectsProperty;

        public EffectEventElement(SerializedProperty eventProperty) {
            if (eventProperty.managedReferenceValue is not EffectEvent) {
                Debug.LogError($"Not an {typeof(EffectEvent)}: {eventProperty.managedReferenceValue}");
                return;
            }

            this.eventProperty = eventProperty;

            effectsProperty = eventProperty.FindPropertyRelative(nameof(EffectEvent.effects));

            Add(CreateListView());
        }

        ListView CreateListView() {
            var source = new SerializedPropertyArraySource(effectsProperty);

            var list = new ListView {
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                selectionType = SelectionType.Multiple,
                reorderable = true,
                reorderMode = ListViewReorderMode.Animated,
                showAddRemoveFooter = true,
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                showBorder = true,
                showFoldoutHeader = false,
                makeHeader = () => new Label($"{eventProperty.displayName} ({nameof(EffectEvent)})") {
                    style = {
                        unityFontStyleAndWeight = FontStyle.Bold,
                    }
                },
                onAdd = list => EffectUtils.CreateAddEffectMenu(effectsProperty, list.Rebuild),
                itemsSource = source,
                makeItem = () => new EffectElement(),
                bindItem = (root, index) => {
                    if (root is EffectElement element) {
                        element.Bind(source[index] as SerializedProperty);
                    }
                },
                unbindItem = (root, _) => {
                    if (root is EffectElement element) {
                        element.Unbind();
                    }
                },
            };

            list.itemIndexChanged += (_, _) => source.ApplyBuffer();

            return list;
        }
    }
}
