using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Slothsoft.Effects.Editor {
    sealed class SerializedPropertyArraySource : IList {
        readonly SerializedProperty property;

        public SerializedPropertyArraySource(SerializedProperty property) {
            this.property = property;
        }

        public int Add(object value) => throw new NotImplementedException();
        public void Clear() => throw new NotImplementedException();
        public bool Contains(object value) => throw new NotImplementedException();
        public int IndexOf(object value) {
            for (int i = 0; i < Count; i++) {
                if (this[i] == value) {
                    return i;
                }
            }

            throw new KeyNotFoundException(value.ToString());
        }
        public void Insert(int index, object value) => throw new NotImplementedException();
        public void Remove(object value) => throw new NotImplementedException();
        public void RemoveAt(int index) {
            property.DeleteArrayElementAtIndex(index);
            property.serializedObject.ApplyModifiedProperties();
        }

        public bool IsFixedSize => false;
        public bool IsReadOnly => false;

        public object this[int index] {
            get => property.GetArrayElementAtIndex(index);
            set {
                int oldIndex = IndexOf(value);

                if (oldIndex == index) {
                    return;
                }

                property.MoveArrayElement(oldIndex, index);
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        public void CopyTo(Array array, int index) {
            for (int i = 0; i < Count; i++) {
                array.SetValue(this[i], index + i);
            }
        }

        public int Count => property.arraySize;
        public object SyncRoot => this;
        public bool IsSynchronized => false;

        public IEnumerator GetEnumerator() {
            for (int i = 0; i < Count; i++) {
                yield return this[i];
            }
        }
    }
}
