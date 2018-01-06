namespace HETSAPI.Models
{
    /// <summary>
    /// User Extension (to support authorization)
    /// </summary>
    public sealed partial class User
    {
        /// <summary>
        /// User Permission Claim Property
        /// </summary>
        public const string PERMISSION_CLAIM = "permission_claim";

        /// <summary>
        /// UserId Claim Property
        /// </summary>
        public const string USERID_CLAIM = "userid_claim";
    }
}
