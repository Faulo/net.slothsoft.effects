using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Slothsoft.Effects.Editor {
    sealed class SerializedPropertyArraySource : IList {
        readonly SerializedObject serialized;
        readonly string propertyPath;
        SerializedProperty property => serialized.FindProperty(propertyPath);
        readonly Dictionary<int, object> setterBuffer = new();

        public SerializedPropertyArraySource(SerializedProperty property) {
            serialized = property.serializedObject;
            propertyPath = property.propertyPath;
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
                if (value is SerializedProperty set) {
                    setterBuffer[index] = set.managedReferenceValue;
                }
            }
        }

        public void CopyTo(Array array, int index) {
            for (int i = 0; i < Count; i++) {
                array.SetValue(this[i], index + i);
            }
        }

        public int Count => property is { arraySize: int count }
            ? count
            : 0;
        public object SyncRoot => this;
        public bool IsSynchronized => false;

        public IEnumerator GetEnumerator() {
            for (int i = 0; i < Count; i++) {
                yield return this[i];
            }
        }

        internal void ApplyBuffer() {
            foreach (var (index, obj) in setterBuffer) {
                property.GetArrayElementAtIndex(index).managedReferenceValue = obj;
            }

            setterBuffer.Clear();
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
