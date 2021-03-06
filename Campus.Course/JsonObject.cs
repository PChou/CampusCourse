﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Campus.Course
{
    public class JsonObject : JsonEncoding
    {
        public Dictionary<string, JsonEncoding> dic = new Dictionary<string, JsonEncoding>();

        public void MergeProperty(string name, JsonEncoding e)
        {
            dic[name] = e;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var d in dic)
            {
                sb.Append("\"");
                sb.Append(d.Key);
                sb.Append("\":");
                sb.Append(d.Value.ToString());
                sb.Append(",");
            }

            return "{" + sb.ToString().TrimEnd(',') + "}";

        }
    }
}