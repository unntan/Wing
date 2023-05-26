using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wing.Model
{
    class LoginInformation
    {
        // ログインユーザID
        public string UserId { get; set; }

        // パスワード
        public string Password { get; set; }

        // 作成日
        public string CreateDateTime { get; set; }

        // 作成ユーザ
        public string CreateUser { get; set; }

        // 更新日
        public string UpdateDateTime { get; set; }

        // 更新ユーザ
        public string UpdateUser { get; set; }
    }
}
