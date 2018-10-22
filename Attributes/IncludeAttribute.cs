using System;

namespace Starship.Data.Attributes {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class IncludeAttribute : Attribute {
        public IncludeAttribute(string propertyName) {
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }
    }
}