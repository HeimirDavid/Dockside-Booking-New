using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace docside_bookingview_2.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Rumsnamn")]
        public string strRoomName { get; set; }

        [Required]
        //[DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18,2)")]
        [DisplayName("Hyra per heldag")]
        [Range(1, int.MaxValue)]
        public int dcmWholeDay { get; set; }

        [Required]
        //[DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Hyra per halvdag")]
        [Range(1, int.MaxValue)]
        public int dcmHalfDay { get; set; }

        [Required]
        //[DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Hyra per timme")]
        [Range(1, int.MaxValue)]
        public int dcmHour { get; set; }

        [Required]
        [Range(0, 100)]
        [DisplayName("Internrabatt")]
        public int dcmInternalDiscount { get; set; }


        [Required]
        [DisplayName("Kvadratmeter")]
        public string SquareMetres { get; set; }

        [DisplayName("Aktiv")]
        public bool Available { get; set; } = true;
        public ICollection<Booking> Bookings { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ImageTitle { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string ImageName { get; set; }

        [NotMapped]
        [DisplayName("")]
        public IFormFile ImageFile { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Våning")]
        public string Floor { get; set; }

        [DisplayName("Max antal personer")]
        public int MaxPeople { get; set; }

    }
}
