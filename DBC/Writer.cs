using System;
using System.Collections.Generic;
using System.IO;

namespace dbc_export
{
    class Writer
    {
        List<Entry> entries;
        Definition definition;

        public Writer(Definition definition, List<Entry> entries)
        {
            this.definition = definition;
            this.entries = entries;
        }

        public void Run()
        {
            string outputPath = String.Format("{0}/built/{1}.dbc", Directory.GetCurrentDirectory(), definition.Name);

            Directory.CreateDirectory(String.Format("{0}/built", Directory.GetCurrentDirectory()));

            Header header = new Header((uint) entries.Count, (uint) definition.Fields.Count, 00000, 1);

            using (var fileStream = new FileStream(outputPath, FileMode.Create))
            using (var memoryStream = new MemoryStream())
            using (var binaryWriter = new BinaryWriter(memoryStream))
            {
                header.Write(binaryWriter);

                // Finish Up
                memoryStream.Position = 0;
				memoryStream.CopyTo(fileStream);
            }

            Console.WriteLine("Finished Writing to {0}", outputPath);
        }
    }
}