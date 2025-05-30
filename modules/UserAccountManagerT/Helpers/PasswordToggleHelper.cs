using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace UserAccountManager.Helpers
{
    public static class PasswordToggleHelper
    {
        /// <summary>
        /// 비밀번호 보기/숨기기 토글
        /// </summary>
        public static void ToggleVisibility(
            PasswordBox passwordBox,
            TextBox visibleBox,
            ref bool isVisible)
        {
            if (isVisible)
            {
                // 숨기기
                passwordBox.Password = visibleBox.Text;
                passwordBox.Visibility = Visibility.Visible;
                visibleBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                // 보이기
                visibleBox.Text = passwordBox.Password;
                passwordBox.Visibility = Visibility.Collapsed;
                visibleBox.Visibility = Visibility.Visible;
            }

            isVisible = !isVisible;
        }

        /// <summary>
        /// 현재 비밀번호 값 추출
        /// </summary>
        public static string GetPassword(
            PasswordBox passwordBox,
            TextBox visibleBox,
            bool isVisible)
        {
            return isVisible
                ? visibleBox.Text.Trim()
                : passwordBox.Password.Trim();
        }
    }
}
