namespace StudentPortal.Web.Constants
{
    public static class RoleConstants
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string User = "User";

        public static bool IsValidRole(string role) =>
            role == SuperAdmin || role == Admin || role == User;
    }
}