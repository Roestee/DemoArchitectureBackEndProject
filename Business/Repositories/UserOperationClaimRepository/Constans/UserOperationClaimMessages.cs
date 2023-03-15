using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repositories.UserOperationClaimRepository.Constans
{
    public class UserOperationClaimMessages
    {
        public static string Added = "Yetki ataması başarıyla oluşturuldu";
        public static string Updated = "Yetki ataması başarıyla güncellendi";
        public static string Deleted = "Yetki ataması başarıyla silindi";
        public static string OperationClaimSetNotAvailable = "Bu kullanıcıya bu yetki daha önce atanmış";
    }
}
