using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.Model
{
    internal class EnumComboBoxItem<T> where T : Enum
    {
        public T Value { get; set; }
        public string Display { get; set; }

        public override bool Equals(object obj)
        {
            return obj is EnumComboBoxItem<T> item &&
                   EqualityComparer<T>.Default.Equals(Value, item.Value);
        }

        public override string ToString()
        {
            return Display;
        }


    }
}
