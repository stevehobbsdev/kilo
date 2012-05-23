using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kilo.Configuration
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreSettingAttribute : Attribute
    {
    }
}
