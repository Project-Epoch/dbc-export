using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dbc_export
{
    class Writer
    {
        List<Entry> entries;
        Definition definition;

        /// <summary>
        /// Construct the Writer.
        /// </summary>
        /// <param name="definition">The DBC file definition we're using.</param>
        /// <param name="entries">The rows we'll be writing.</param>
        public Writer(Definition definition, List<Entry> entries)
        {
            this.definition = definition;
            this.entries = entries;
        }

        /// <summary>
        /// Actually executes the Writer.
        /// </summary>
        public void Run()
        {
            string outputPath = String.Format("{0}/built/{1}.dbc", Directory.GetCurrentDirectory(), definition.Name);

            Directory.CreateDirectory(String.Format("{0}/built", Directory.GetCurrentDirectory()));

            using (var fileStream = new FileStream(outputPath, FileMode.Create))
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                /** Write Initial Header */
                Header header = new Header(
                    (uint) entries.Count, 
                    (uint) entries[0].Values.Count, 
                    entries[0].CalculateSize(), 
                    0
                );
                header.StringBlockOffset = header.GetHeaderLength() + header.RecordCount * header.RecordSize;
                header.Write(binaryWriter);

                /** Write dummy string block */
                binaryWriter.Seek((int) header.StringBlockOffset, SeekOrigin.Begin);
                binaryWriter.Write(Encoding.UTF8.GetBytes("\0"));

                /** Go to end of header */
                binaryWriter.Seek((int) header.GetHeaderLength(), SeekOrigin.Begin);

                WriteFile(ref header, this.entries, binaryWriter);

                /** Write final header */
                binaryWriter.Seek(0, SeekOrigin.Begin);
                header.Write(binaryWriter);

                /** Save to disk */
                memoryStream.Position = 0;
				memoryStream.CopyTo(fileStream);

                fileStream.Close();
            }

            Console.WriteLine("Finished Writing to {0}", outputPath);
        }

        /// <summary>
        /// Write entries to the DBC file. Should cover the majority of DBC files and types.
        /// </summary>
        /// <param name="header">Reference to our header file in case we need to write extra data eg string block size / offsets.</param>
        /// <param name="entries">The rows we will be putting in the DBC.</param>
        /// <param name="binaryWriter">The binary stream we're writing out too. </param>
        private void WriteFile(ref Header header, List<Entry> entries, BinaryWriter binaryWriter)
        {
            foreach (Entry entry in entries)
            {
                foreach (object value in entry.Values)
                {
                    Value obj = (Value) value;

                    switch (obj.Field.Type.ToLower())
                    {
                        case "sbyte":
                            binaryWriter.Write((sbyte) obj.Data);
                            break;
                        case "byte":
                            binaryWriter.Write((byte) obj.Data);
                            break;
                        case "int32":
                        case "int":
                            binaryWriter.Write((int) obj.Data);
                            break;
                        case "uint32":
                        case "uint":
                            binaryWriter.Write((uint) obj.Data);
                            break;
                        case "int64":
                        case "long":
                            binaryWriter.Write((long) obj.Data);
                            break;
                        case "uint64":
                        case "ulong":
                            binaryWriter.Write((ulong) obj.Data);
                            break;
                        case "single":
                        case "float":
                            binaryWriter.Write((float) obj.Data);
                            break;
                        case "boolean":
                        case "bool":
                            binaryWriter.Write((bool) obj.Data);
                            break;
                        case "string":
                            string data = ((string) obj.Data).Replace("\'", "'");

                            binaryWriter.Write(header.StringBlockSize);
                            long startPoint = binaryWriter.BaseStream.Position;

                            binaryWriter.Seek((int) header.StringBlockOffset + (int) header.StringBlockSize, SeekOrigin.Begin);
                            string final = data + "\0";
                            byte[] converted = Encoding.UTF8.GetBytes(final);
                            binaryWriter.Write(converted);
                            header.StringBlockSize += (uint) converted.Length;
                            binaryWriter.Seek((int) startPoint, SeekOrigin.Begin);

                            break;
                        case "int16":
                        case "short":
                            binaryWriter.Write((short) obj.Data);
                            break;
                        case "uint16":
                        case "ushort":
                            binaryWriter.Write((ushort) obj.Data);
                            break;
                    }
                }
            }
        }
    }
}