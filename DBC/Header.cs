using System.Collections.Generic;
using System.IO;

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

        public void Write(BinaryWriter writer)
        {
            writer.Write(System.Text.Encoding.UTF8.GetBytes("WDBC"));

            writer.Write(RecordCount);

            writer.Write(FieldCount);

            writer.Write((uint)1);
        }
    }
}