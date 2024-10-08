﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AdventOfCode2021
{
    public static class Extensions
    {
        public static T ToEnum<T>(this string value, T defaultValue)
        {
            T result = defaultValue;

            try { result = (T)Enum.Parse(typeof(T), value, true); }
            catch (Exception) { }

            return result;
        }
        public static string ToJson(this object obj, Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }
        public static T? FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { MaxDepth = null });
        }

        public static T Clone<T>(this T obj)
        {
            if (obj == null)
                return obj;
            else
                return obj.ToJson().FromJson<T>();
        }
    }
}
