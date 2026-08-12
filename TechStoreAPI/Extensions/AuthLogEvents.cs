namespace TechStoreAPI.Extensions
{
    public static class AuthLogEvents
    {
        public static readonly EventId LoginSuccess =
            new(1001, "CustomerLoginSuccess");

        public static readonly EventId LoginFailed =
            new(1002, "CustomerLoginFailed");
    }
}
