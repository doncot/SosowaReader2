using SosowaReader.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SosowaReader.Converters
{
    public class SortTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var sortType = (SortEnum) value;

            switch(sortType)
            {
                case SortEnum.Title:
                    return "タイトル";

                case SortEnum.Author:
                    return "名前";

                case SortEnum.UploadDate:
                    return "投稿時間";
                    
                case SortEnum.Point:
                    return "POINT";

                default:
                    throw new NotSupportedException();

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
