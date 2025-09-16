namespace Vanfist.Constants;

public class Role
{
    public const string Admin = "Admin";
    public const string User = "User";
    public const string Guest = "Guest";
    public const string UserAndAdmin = "User,Admin";

    public static List<string> List()
    {
        return new List<string> { Admin, User, Guest };
    }
}