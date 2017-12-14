namespace SasTokenService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    ///  Class to Hold Details for Device
    /// </summary>
    public partial class Device : EntityBase
    {
        /// <summary>
        /// Getter and Setter For DeviceID Field
        /// </summary>
        [Key]
        public long DeviceID { get; set; }
        /// <summary>
        /// Getter and Setter For Name Field
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        /// <summary>
        /// Getter and Setter For DeviceKey Field
        /// </summary>
        [Required]
        [StringLength(255)]
        public string DeviceKey { get; set; }
        /// <summary>
        /// Getter and Setter For KeyValidity Field
        /// </summary>
        public DateTime KeyValidity { get; set; }
        /// <summary>
        /// Getter and Setter For Ip Field
        /// </summary>
        [Required]
        [StringLength(255)]
        public string IP { get; set; }
        /// <summary>
        /// Getter and Setter For Owner Field
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Owner { get; set; }
        /// <summary>
        /// Getter and Setter For GeoPosition Field
        /// </summary>
        [StringLength(255)]
        public string GeoPosition { get; set; }
        /// <summary>
        /// Getter and Setter For Uri Field
        /// </summary>
        [StringLength(10)]
        public string Uri { get; set; }


    }
}
