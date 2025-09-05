using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomControl.ViewModel
{
    public static class CommonProps
    {
        public static readonly DependencyProperty IsLoginProperty =
            DependencyProperty.RegisterAttached(
                "IsLogin",                // 속성 이름
                typeof(bool),             // 속성 타입
                typeof(CommonProps),      // 등록자 (소유자 클래스)
                new PropertyMetadata(false)
            );

        public static void SetIsLogin(DependencyObject element, bool value)
        {
            element.SetValue(IsLoginProperty, value);
        }

        public static bool GetIsLogin(DependencyObject element)
        {
            return (bool)element.GetValue(IsLoginProperty);
        }
    }
}
