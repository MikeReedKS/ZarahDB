using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZarahDB_WebAPI
{
    /// <summary>
    /// Class GetKeysDto.
    /// </summary>
    public class GetKeysDto
    {
        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        [Required]
        public string Instance { get; set; }

        /// <summary>
        /// Gets or sets the table.
        /// </summary>
        /// <value>The table.</value>
        [Required]
        public string Table { get; set; }

        /// <summary>
        /// Gets or sets the list of columns.
        /// </summary>
        /// <value>The column.</value>
        [Required]
        public List<string> Keys { get; set; }
    }
}
