using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Runit.Backend.Models;

namespace Runit.Backend
{
    internal class ICalSerializerOutputFormatter : TextOutputFormatter
    {

        protected static readonly string DateTimeFormat = "yyyyMMddTHHmmssZ";
        protected static readonly string DateFormat = "yyyyMMdd";
        public ICalSerializerOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/calendar"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        protected override bool CanWriteType(Type type)
        {
            if (typeof(Activity).IsAssignableFrom(type) || typeof(IEnumerable<Activity>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;

            StringBuilder buffer = new StringBuilder();


            var now = DateTime.Now;

            buffer.AppendLine("BEGIN:VCALENDAR");
            buffer.AppendLine("PRODID:-//RunIt//Activity calendar//EN");
            buffer.AppendLine("VERSION:2.0");
            buffer.AppendLine("METHOD:PUBLISH");

            if (context.Object is IEnumerable<Activity>)
            {
                foreach (Activity activity in context.Object as IEnumerable<Activity>)
                {
                    ActivityToICalString(buffer, activity, now, now, now);
                }
            }
            else
            {
                var activity = context.Object as Activity;
                ActivityToICalString(buffer, activity, now, now, now);
            }

            buffer.AppendLine("END:VCALENDAR");

            return response.WriteAsync(buffer.ToString());
        }

        private static void ActivityToICalString(StringBuilder buffer, Activity activity, DateTime now, DateTime created, DateTime modified)
        {
            buffer.AppendLine("BEGIN:VEVENT");
            // Set Date type dtStart for all-day event
            buffer.AppendLine("DTSTART;VALUE=DATE:" + activity.Date.ToUniversalTime().ToString(DateFormat));
            buffer.AppendLine("DTSTAMP:" + now.ToUniversalTime().ToString(DateTimeFormat));
            buffer.AppendLine("UID:" + Guid.NewGuid());
            buffer.AppendLine("CREATED:" + created.ToUniversalTime().ToString(DateTimeFormat));
            buffer.AppendLine("LAST-MODIFIED:" + modified.ToUniversalTime().ToString(DateTimeFormat));
            buffer.AppendLine("SUMMARY:" + activity.Title);
            buffer.AppendLine("SEQUENCE:0");
            buffer.AppendLine("STATUS:CONFIRMED");
            buffer.AppendLine("TRANSP:OPAQUE");
            buffer.AppendLine("END:VEVENT");
        }
    }


}