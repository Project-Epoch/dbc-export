namespace dbc_export
{
    class Header
    {
        private uint Magic;

        private uint RecordCount;

        private uint FieldCount;

        private uint RecordSize;

        private uint StringBlockSize;

        public Header(uint RecordCount, uint FieldCount, uint RecordSize, uint StringBlockSize)
        {
            this.RecordCount = RecordCount;
            this.FieldCount = FieldCount;
            this.RecordSize = RecordSize;
            this.StringBlockSize = StringBlockSize;
        }

        public void Write()
        {
            // TODO FIGURE THIS OUT. BYTESTREAM?
        }
    }
}