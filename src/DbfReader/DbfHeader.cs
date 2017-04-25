namespace DbfReader
{
    public class DbfHeader
    {
        /// <summary>
        /// The column index
        /// </summary>
        public int Index { get; set; }
        public string Name { get; set; }
        public char FieldType { get; set; }
        public int ColumnDisplacement { get; set; }
        public int NumDecimalPlaces { get; set; }
        public int Length { get; set; }

        public int ColumnOffset { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Header:\tName: {0}\n\tField Type: {1}\n\tIndex: {2}\n\tDisplacement: {3}\n\tNum dec place: {4}\n\tLength: {5}\n\tOffset: {6}",
                    Name, FieldType, Index, ColumnDisplacement, NumDecimalPlaces, Length,
                    ColumnOffset
                );
        }
    }
}
