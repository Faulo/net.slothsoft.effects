using Slothsoft.UnityExtensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Slothsoft.Effects.Editor {
    sealed class EffectEventElement : VisualElement {
        public EffectEventElement(SerializedProperty property) {
            /*
            var header = new VisualElement {
                style = {
                    flexDirection = FlexDirection.Row,
                }
            };

            var title = new Label($"{property.displayName} ({nameof(EffectEvent)})") {
                style = {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    flexGrow = 1,
                }
            };

            var addButton = new Button(() => EffectUtils.CreateAddEffectMenu(property));
            addButton.Add(new Image { image = EffectUtils.iconPlus });

            header.Add(title);
            header.Add(addButton);
            Add(header);
            //*/

            var effectsProperty = property.FindPropertyRelative(nameof(EffectEvent.effects));
            Add(CreateListView(property, effectsProperty));
        }

        ListView CreateListView(SerializedProperty property, SerializedProperty effectsProperty) {
            var list = new ListView {
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                reorderable = true,
                reorderMode = ListViewReorderMode.Animated,
                selectionType = SelectionType.Single,
                showAddRemoveFooter = true,
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                showBorder = true,
                showFoldoutHeader = false,
                makeHeader = () => {
                    var header = new VisualElement();

                    header.Add(new Label($"{property.displayName} ({nameof(EffectEvent)})") {
                        style = {
                            unityFontStyleAndWeight = FontStyle.Bold,
                        }
                    });

                    return header;
                },
                onAdd = _ => EffectUtils.CreateAddEffectMenu(effectsProperty),
            };

            var source = new SerializedPropertyArraySource(effectsProperty);
            list.itemsSource = source;

            list.makeItem = () => new EffectElement();
            list.bindItem = (root, index) => {
                if (root is EffectElement element) {
                    element.Bind(source[index] as SerializedProperty);
                    // element.onRemove = RemoveEffect(index);
                }
            };
            list.unbindItem = (root, index) => { };

            list.itemIndexChanged += (oldIndex, newIndex) => {
                var so = effectsProperty.serializedObject;
                so.Update();
                effectsProperty.MoveArrayElement(oldIndex, newIndex);
                so.ApplyModifiedProperties();
                so.Update();
                list.RefreshItems();
            };

            return list;
        }

        static void AddEffect(SerializedProperty effectsProp, Implementation<IEffect> creator) {
            var so = effectsProp.serializedObject;
            so.Update();

            int index = effectsProp.arraySize;
            effectsProp.arraySize++;

            // WICHTIG: erst anwenden, dann Element holen und befüllen (sichere Referenz)
            so.ApplyModifiedProperties();
            so.Update();

            var element = effectsProp.GetArrayElementAtIndex(index);
            element.managedReferenceValue = creator.CreateInstance();

            so.ApplyModifiedProperties();
            so.Update();
        }
    }
}
