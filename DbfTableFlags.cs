namespace DbfReader
{
    internal static class DbfTableFlags
    {
        public static byte HasStructuralCdx = 0x01;
        public static byte HasMemoField = 0x02;
        public static byte IsDatabase = 0x04;
    }
}
