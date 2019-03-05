namespace ApplicationCore
{
    public static class DefaultSettings
    {
        public static int MaximumTake { get; set; } = 200;
        public static int DefaultTake { get; set; } = 60;

        public static string RootFileDirectory { get; set; } = @"C:\files\";
    }
}
