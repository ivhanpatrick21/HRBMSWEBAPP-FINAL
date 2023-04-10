﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HRBMSWEBAPP.Models
{
    public class RoomCategories
    {
       /* [DisplayName("Category ID")]
        [Key]
        public int Id { get; set; }
*/
        [DisplayName("Room Name")]
        [Required]
        public string Room_Name { get; set; }

        [DisplayName("Room Description")]
        [Required]
        public string Description { get; set; } //Room Features

        [DisplayName("Price")]
        [DisplayFormat(DataFormatString = "₱{0:N}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        [Required]
        public int Price { get; set; }

        public int NoOfRooms { get; set; }

        public RoomCategories() { }
        public RoomCategories(string room_Name, string description, int price, int noOfRooms)
        {
            Room_Name = room_Name;
            Description = description;
            Price = price;
            NoOfRooms = noOfRooms;
        }

    }
}
