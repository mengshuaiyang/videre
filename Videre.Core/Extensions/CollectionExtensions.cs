﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace Videre.Core.Extensions
{
    public static class CollectionExtensions
    {
        //todo: move to CodeEndeavors.Extensions
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> copyFrom, bool newDict = false)
        {
            //todo: use linq with SelectMany?
            IDictionary<TKey, TValue> result;
            result = newDict ? new Dictionary<TKey, TValue>() : source;
            if (newDict)
            {
                foreach (var x in source)
                    result[x.Key] = x.Value;
            }
            foreach (var x in copyFrom)
                result[x.Key] = x.Value;
            return result;
        }

        public static IDictionary<TKey, TValue> AddSafe<TKey, TValue>(this IDictionary<TKey, TValue> data, TKey key, TValue value, bool replace = false)
        {
            if (!data.ContainsKey(key))
                data.Add(key, value);
            else if (replace)
                data[key] = value;
            return data;
        }

        //http://blog.spontaneouspublicity.com/associating-strings-with-enums-in-c
        //todo: move to CodeEndeavors.Extensions
        //todo: review perf impact of this approach!!!
        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static Dictionary<string, object> AnonymousToDictionary(this object obj)
        {
            var dict = new Dictionary<string, object>();

            var expandoDict = obj as IDictionary<string, object>;//expandos convert directly
            if (expandoDict != null)
            {
                foreach (var key in expandoDict.Keys)
                    dict[key] = expandoDict[key];
            }
            else
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj))
                    dict[property.Name] = property.GetValue(obj);
            }
            return dict;
        }
    }
}