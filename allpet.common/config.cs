﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllPet.Common
{
    public static class JsonConvert
    {

        public static string AsString(this JToken json)
        {
           return (string)json;
        }
        public static string[] AsStringArray(this JToken json)
        {
            var list = json as JArray;
            string[] result = new string[list.Count];
            for(var i=0;i<result.Length;i++)
            {
                result[i] = list[i].AsString();
            }
            return result;
        }
        public static System.Net.IPEndPoint AsIPEndPoint(this JToken json)
        {
            var ip = ((string)json).Split(':');
            return new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip[0]), int.Parse(ip[1]));
        }
        public static System.Net.IPEndPoint AsIPEndPoint(this string str)
        {
            var ip = str.Split(':');
            return new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ip[0]), int.Parse(ip[1]));
        }
        public static System.Net.IPEndPoint[] AsIPEndPointArray(this JToken json)
        {
            var list = json as JArray;
            System.Net.IPEndPoint[] result = new System.Net.IPEndPoint[list.Count];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = list[i].AsIPEndPoint();
            }
            return result;
        }
    }
    public class Config
    {
        AllPet.Common.ILogger logger;
        public Config(AllPet.Common.ILogger logger)
        {
            this.logger = logger;
        }
        public System.Collections.Generic.Dictionary<string, JObject> files = new System.Collections.Generic.Dictionary<string, JObject>();
        public static bool IsOpen(JObject group)
        {
            if (group == null)
                return false;
            if(group.ContainsKey("Open"))
            {
                return (bool)group["Open"];
            }
            else
            {
                return false;
            }
        }
        public JToken GetJson(string filename, string configpath)
        {
            JObject config = null;
            lock (files)
            {
                try
                {
                    var f = System.IO.Path.GetFullPath(filename);
                    if (files.ContainsKey(f) == false)
                    {
                        var json = JObject.Parse(System.IO.File.ReadAllText(filename));
                        files[f] = json;
                    }
                    config = files[f];
                }
                catch (Exception err)
                {
                    logger.Error("get config error from:" + filename + " err=" + err.ToString());
                    return null;
                }
            }
            try
            {
                var token = config.SelectToken(configpath);
                return token;
            }
            catch (Exception err)
            {
                logger.Error("get config error from:" + filename + "[" + configpath + "] err=" + err.ToString());
                return null;
            }
        }

        public Int64 GetInt64(string filename, string configpath)
        {
            var json = GetJson(filename, configpath);
            return (Int64)json;
        }
        public string GetString(string filename, string configpath)
        {
            var json = GetJson(filename, configpath);
            return (string)json;
        }
        public System.Net.IPEndPoint GetIPEndPoint(string filename, string configpath)
        {
            var json = GetJson(filename, configpath);
            if (json == null)
                return null;
            return json.AsIPEndPoint();
        }

    }
}