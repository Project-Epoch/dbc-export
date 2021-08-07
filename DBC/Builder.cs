using MySqlConnector;
using System;
using System.Collections.Generic;

namespace dbc_export
{
    class Builder
    {
        private Definition definition;

        private MySqlConnection connection;

        /// <summary>
        /// Create the DBC File Builder instance.
        /// </summary>
        /// <param name="connection">The active MySQL Connection.</param>
        /// <param name="definition">The DBC definition we're using.</param>
        public Builder(MySqlConnection connection, Definition definition)
        {
            this.connection = connection;
            this.definition = definition;
        }

        /// <summary>
        /// Execute the Builder.
        /// </summary>
        public void Run()
        {
            if (! TableExists())
            {
                Console.WriteLine(String.Format("Could not find matching table for {0} ({1})! Skipping.", definition.Name, definition.Table));

                return;
            }

            Console.WriteLine(String.Format("Beginning Export of {0}", definition.Name));

            GenerateExtraFields();

            List<Entry> entries;

            try
            {
                entries = GetFromDatabase();
            }
            catch (DataNotFoundException)
            {
                Console.WriteLine(String.Format("No Data found for {0} in table {1}", definition.Name, definition.Table));

                return;
            }
            
            Writer writer = new Writer(definition, entries);
            writer.Run();
        }

        /// <summary>
        /// Parses the definitions fields to figure out if we need to add extra fields eg for localisation or array fields.
        /// </summary>
        private void GenerateExtraFields()
        {
            List<Field> newFields = new List<Field>();

            foreach (Field field in definition.Fields)
            {
                if (! field.Array)
                {
                    /** Not an array or localisation, just add as a singular field. */
                    if (field.Type.ToLower() != "loc")
                    {
                        newFields.Add(field);

                        continue;
                    }
                }

                /** Not localisation but is an array. Add array fields. */
                if (field.Type.ToLower() != "loc")
                {
                    for (int i = 0; i < field.Size; i++)
                    {
                        newFields.Add(new Field {
                            Name = String.Format("{0}_{1}", field.Name, i + 1),
                            Type = field.Type,
                            Array = false,
                            Size = 1,
                        });
                    }

                    continue;
                }
                
                /** Must be localisation now, get the languages. */
                Array languages = Enum.GetValues(typeof(Languages));

                /** For each language add a new field that has the format "{field_name}_{language}" */
                for (int i = 0; i < languages.Length; i++)
                {
                    newFields.Add(new Field {
                        Name = String.Format("{0}_{1}", field.Name, languages.GetValue(i).ToString()),
                        Type = "string",
                        Array = false,
                        Size = 1,
                    });
                }

                /** Localisation always ends with a "Mask" field */
                newFields.Add(new Field {
                    Name = String.Format("{0}_Mask", field.Name),
                    Type = "uint",
                    Array = false,
                    Size = 1,
                });
            }

            definition.Fields = newFields;
        }

        /// <summary>
        /// Queries the data from the DB and then parses it into entries.
        /// </summary>
        /// <returns>A list of each new DBC file entry (row).</returns>
        private List<Entry> GetFromDatabase()
        {
            string fields = "";

            int columnCount = definition.Fields.Count;

            foreach (Field field in definition.Fields)
            {
                fields = fields + String.Format("{0}, ", field.Name);
            }

            fields = fields.Substring(0, fields.Length - 2); // Hacky. Removes the final comma and space.

            MySqlCommand query = new MySqlCommand(String.Format("SELECT {0} FROM {1}", fields, definition.Table), connection);

            MySqlDataReader result = query.ExecuteReader();

            if (! result.HasRows) 
            {
                result.Close();

                throw new DataNotFoundException();
            }

            List<Entry> entries = new List<Entry>();

            int rows = 0;
            while (result.Read())
            {
                Entry entry = new Entry();
                entry.Values = new List<Value>();

                for (int i = 0; i < columnCount; i++)
                {
                    entry.Values.Add(new Value {
                        Field = definition.Fields[i],
                        Data = result[i],
                    });
                }

                entries.Add(entry);

                rows++;
            }

            Console.WriteLine("Retrieved {0} Rows for {1}", rows, definition.Name);

            result.Close();

            return entries;
        }

        /// <summary>
        /// Check to see if the table exists for a definition.
        /// </summary>
        /// <returns></returns>
        private bool TableExists()
        {
            MySqlCommand query = new MySqlCommand(string.Format("SHOW TABLES LIKE '{0}'", definition.Table), connection);

            MySqlDataReader result = query.ExecuteReader();

            bool found = result.HasRows;

            result.Close();

            return found;
        }
    }
}