using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Campus.Course
{
    public class JsonDateTime : JsonEncoding
    {
        private DateTime? _datetime;

        public JsonDateTime(DateTime? datetime)
        {
            _datetime = datetime;
        }

        public override string ToString()
        {
            if (_datetime == null)
            {
                return "null";
            }
            else
            {
                return "\"" + EncodeString("/Date(" + ((DateTime)_datetime - new DateTime(1970, 1, 1)).TotalMilliseconds.ToString("0") + ")/") + "\"";
            }
        }
    }
}