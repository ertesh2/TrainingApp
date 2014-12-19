using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace TrainingApp
{

    [ContentProperty(Name = "Value")]
    public sealed class KeyValuePair
    {
        public Object Key { get; set; }

        public Object Value { get; set; }
    }

    [ContentProperty(Name = "LookupTable")]
    public sealed class KeyValueConverter : IValueConverter
    {
        private readonly List<KeyValuePair> _lookupTable;

        public object DefaultValue { get; set; }

        public KeyValueConverter()
        {
            _lookupTable = new List<KeyValuePair>();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // The xaml parser turns enums into boxed ints, so for comparisons to work we must also interpret enum values as ints
            if (value is Enum)
            {
                value = (int)value;
            }
            else if (value is uint)
            {
                value = (int)((uint)value);
            }

            object retVal = null;

            var keyValuePair = LookupTable.FirstOrDefault(kvp => kvp.Key.Equals(value));
            retVal = keyValuePair != null ? keyValuePair.Value : DefaultValue;

            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public List<KeyValuePair> LookupTable
        {
            get
            {
                return _lookupTable;
            }
        }
    }
} // namespace