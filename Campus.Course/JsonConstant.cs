﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Campus.Course
{
    public class JsonConstant : JsonEncoding
    {
        public object _obejct;

        public JsonConstant(object o)
        {
            _obejct = o;
        }


        public override string ToString()
        {
            if (_obejct == null)
            {
                return "null";
            }
            else if (_obejct is Boolean)
            {
                return _obejct.ToString().ToLower();
            }
            else if (_obejct is ValueType && !(_obejct is DateTime))
            {
                return EncodeString(_obejct.ToString());
            }
            else
            {
                return "\"" + EncodeString(_obejct.ToString()) + "\"";
            }
        }
    }
}