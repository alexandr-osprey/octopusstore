namespace ApplicationCore
{
    public static class DefaultSettings
    {
        public static int MaximumTake { get; set; } = 100;
        public static int DefaultTake { get; set; } = 20;
        public static int OnePageTake { get; set; } = 200;

        public static string RootFileDirectory { get; set; } = @"C:\files\";
    }
}
