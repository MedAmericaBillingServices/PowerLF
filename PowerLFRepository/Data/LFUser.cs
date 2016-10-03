using System;
using Laserfiche.RepositoryAccess;

namespace PowerLFRepository.Data
{
    public class LFUser : ILFPrincipal
    {
        private readonly UserInfo _userInfo;

        public string Name => _userInfo.Name;
        public string FeatureRights => _userInfo.FeatureRights.ToString();
        public DateTime LastLogOn => _userInfo.LastLogOn;

        internal LFUser(UserInfo userInfo)
        {
            _userInfo = userInfo;
        }
    }
}