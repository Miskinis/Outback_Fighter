﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Plugins.SerializableDictionary
{
    [Serializable]
    public class StringBoolDictionary : SerializableDictionaryBase<string, bool>
    {
    }

    [Serializable]
    public class IntBoolDictionary : SerializableDictionaryBase<int, bool>
    {
    }

    [Serializable]
    public class StringAnimationCurveDictionary : SerializableDictionaryBase<string, AnimationCurve>
    {
    }

    [Serializable]
    public class IntAnimationCurveDictionary : SerializableDictionaryBase<int, AnimationCurve>
    {
    }

    [Serializable]
    public class StringAnimatorDictionary : SerializableDictionaryBase<string, Animator>
    {
    }

    public abstract class DrawableDictionary
    {
    }

    [Serializable]
    public class SerializableDictionaryBase<TKey, TValue> : DrawableDictionary, IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        #region Fields

        [NonSerialized] private Dictionary<TKey, TValue> _dict;

        #endregion Fields

        #region IDictionary Interface

        public int Count => _dict?.Count ?? 0;

        public void Add(TKey key, TValue value)
        {
            (_dict ?? (_dict = new Dictionary<TKey, TValue>())).Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            if (_dict == null) return false;
            return _dict.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get
            {
                if (_dict == null) _dict = new Dictionary<TKey, TValue>();
                return _dict.Keys;
            }
        }

        public bool Remove(TKey key)
        {
            if (_dict == null) return false;
            return _dict.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_dict == null)
            {
                value = default(TValue);
                return false;
            }

            return _dict.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get
            {
                if (_dict == null) _dict = new Dictionary<TKey, TValue>();
                return _dict.Values;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (_dict == null) throw new KeyNotFoundException();
                return _dict[key];
            }
            set
            {
                if (_dict == null) _dict = new Dictionary<TKey, TValue>();
                _dict[key] = value;
            }
        }

        public void Clear()
        {
            _dict?.Clear();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            if (_dict == null) _dict = new Dictionary<TKey, TValue>();
            (_dict as ICollection<KeyValuePair<TKey, TValue>>)?.Add(item);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (_dict == null) return false;
            return (_dict as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (_dict == null) return;
            (_dict as ICollection<KeyValuePair<TKey, TValue>>)?.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_dict == null) return false;
            return (_dict as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            if (_dict == null) return default(Dictionary<TKey, TValue>.Enumerator);
            return _dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_dict == null) return Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
            return _dict.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            if (_dict == null) return Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator();
            return _dict.GetEnumerator();
        }

        #endregion IDictionary Interface

        #region ISerializationCallbackReceiver

        [SerializeField] private TKey[] _keys;

        [SerializeField] private TValue[] _values;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (_keys != null && _values != null)
            {
                if (_dict == null) _dict = new Dictionary<TKey, TValue>(_keys.Length);
                else _dict.Clear();
                for (var i = 0; i < _keys.Length; i++)
                    if (i < _values.Length)
                        _dict[_keys[i]] = _values[i];
                    else
                        _dict[_keys[i]] = default(TValue);
            }

            _keys   = null;
            _values = null;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (_dict == null || _dict.Count == 0)
            {
                _keys   = null;
                _values = null;
            }
            else
            {
                var cnt = _dict.Count;
                _keys   = new TKey[cnt];
                _values = new TValue[cnt];
                var i = 0;
                var e = _dict.GetEnumerator();
                while (e.MoveNext())
                {
                    _keys[i]   = e.Current.Key;
                    _values[i] = e.Current.Value;
                    i++;
                }
            }
        }

        #endregion ISerializationCallbackReceiver
    }
}