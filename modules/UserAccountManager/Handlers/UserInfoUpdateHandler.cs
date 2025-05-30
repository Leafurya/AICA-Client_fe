using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAccountManager.Interfaces;
using UserAccountManager.Services;

namespace UserAccountManager.Handlers
{
    public static class UserInfoUpdateHandler
    {
        public static async Task TryEnterEditModeAsync(IUserInfoEditView view)
        {
            var pw = view.PasswordForCheck;

            if (string.IsNullOrWhiteSpace(pw))
            {
                view.ShowMessage("비밀번호를 입력해주세요.");
                return;
            }

            var success = await UserInfoService.VerifyPasswordAsync(pw);

            if (!success)
            {
                view.ShowMessage("비밀번호가 일치하지 않습니다.");
                return;
            }

            // 비밀번호 검증 성공 → 편집모드 진입
            view.ShowPasswordCheckPanel(false);
            view.SetEditMode(true);
        }
    }
}
