﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheClient = CodeEndeavors.Distributed.Cache.Client;
using CodeEndeavors.Extensions;

namespace Videre.Core.Services
{
    public class Caching
    {
        private static bool _initialized = false;
        public static void Initialize() //todo: keep this lazy load?
        {
            if (!_initialized)
            {
                CacheClient.Service.LogLevel = CacheClient.Service.LoggingLevel.Minimal;
                CacheClient.Service.OnLoggingMessage += (message) =>
                {
                    System.Diagnostics.Debug.WriteLine("LOG: " + message);
                };
                CacheClient.Service.RegisterCache("VidereRequestCache", "{cacheType: 'CodeEndeavors.Distributed.Cache.Client.Web.HttpRequestCache'}");
                CacheClient.Service.RegisterCache("VidereFileCache", "{cacheType: 'CodeEndeavors.Distributed.Cache.Client.InMemory.InMemoryCache'}");
                _initialized = true;
            }
        }

        public static T GetRequestCacheEntry<T>(string cacheKey, Func<T> lookupFunc)
        {
            Initialize();
            return CacheClient.Service.GetCacheEntry("VidereRequestCache", cacheKey, lookupFunc);
        }

        public static T GetFileJSONObject<T>(string fileName)
        {
            return CacheClient.Service.GetCacheEntry<T>("VidereFileCache", fileName, () =>
                {
                    var json = fileName.GetFileContents();
                    return json.ToObject<T>();
                }, new { monitorType = "CodeEndeavors.Distributed.Cache.Client.File.FileMonitor", fileName = fileName, uniqueProperty = "fileName" });
        }

    }
}
