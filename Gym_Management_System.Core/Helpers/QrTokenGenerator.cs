using System.Security.Cryptography;

namespace GymManagementSystem.Core.Helpers;

public static class QrTokenGenerator
{
    public static string Create()
    {
        byte[] bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToHexString(bytes);
    }
}