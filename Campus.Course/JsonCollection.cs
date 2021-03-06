﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Campus.Course
{
    public class JsonCollection : JsonEncoding
    {
        public List<JsonEncoding> dic = new List<JsonEncoding>();


        public void AppendObject(JsonEncoding e)
        {
            dic.Add(e);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var d in dic)
            {
                sb.Append(d.ToString());
                sb.Append(",");
            }

            return "[" + sb.ToString().TrimEnd(',') + "]";

        }
    }
}