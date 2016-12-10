namespace DbfReader
{
    internal static class DbfByteOffset
    {
        public static readonly Range<byte> DbfFileType = new Range<byte>(0);
        public static readonly Range<byte> LastUpdate = new Range<byte>(1, 3);
        public static readonly Range<byte> NumRecords = new Range<byte>(4, 7);
        public static readonly Range<byte> PositionFirstDataRecord = new Range<byte>(8, 9);
        public static readonly Range<byte> LengthOfDataRecord = new Range<byte>(10, 11);
        public static readonly Range<byte> TableFlags = new Range<byte>(28);
        public static readonly Range<byte> FieldSubrecords = new Range<byte>(32);

        // subrecord offsets
        public static readonly Range<byte> FieldName = new Range<byte>(0, 10);
        public static readonly Range<byte> FieldType = new Range<byte>(11);
        public static readonly Range<byte> ColumnDisplacement = new Range<byte>(12, 15);
        public static readonly Range<byte> LengthOfField = new Range<byte>(16);
        public static readonly Range<byte> NumDecimalPlaces = new Range<byte>(17);
    }
}
