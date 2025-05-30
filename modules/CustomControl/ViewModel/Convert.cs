using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CustomControl.ViewModel
{
    public class POSConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> list)
            {
                // 리스트의 항목들을 콤마로 구분한 문자열로 변환
                return string.Join(", ", list);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ConvertBack은 사용하지 않으므로, 기본적으로 null 반환
            return null;
        }
    }

}
