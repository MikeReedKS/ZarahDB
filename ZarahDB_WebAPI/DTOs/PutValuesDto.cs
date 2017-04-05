using System.ComponentModel.DataAnnotations;
using ZarahDB_Library.Types;

namespace ZarahDB_WebAPI
{
    /// <summary>
    /// Class PutValuesDto.
    /// </summary>
    public class PutValuesDto
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
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [Required]
        public KeyColumnValues KeyColumnValues { get; set; } = new KeyColumnValues();
    }
}
