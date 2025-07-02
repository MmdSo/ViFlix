using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViFlix.Data.Users;

namespace ViFlix.Data.RefreshTokens
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public long UserId { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

        #region Relations
        [ForeignKey("UserId")]
        public SiteUsers User { get; set; }
        #endregion
    }
}
