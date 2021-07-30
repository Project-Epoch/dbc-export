using System.Collections.Generic;

namespace dbc_export
{
    /// <summary>
    /// Entrypoint for parsing our definitions from JSON.
    /// </summary>
    class ParsedDefinitions
    {
        /// <summary>
        /// All of the DBC definitions we have parsed.
        /// </summary>
        /// <value></value>
        public List<Definition> definitions { get; set; }
    }
}