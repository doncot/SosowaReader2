using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SosowaReader.Converters
{
    public class DateTimeDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = (DateTime)value;
            var test = DateTime.Today;
            if (date.Date == DateTime.Today.Date)
            {
                //今日分は時分のみ
                return date.ToString("H:mm");
            }

            //同年は年を抜く
            if(date.Year == DateTime.Today.Year)
            {
                return date.ToString("M/d H:mm");
            }

            //デフォルト
            return date.ToString("yy/mMd H:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
