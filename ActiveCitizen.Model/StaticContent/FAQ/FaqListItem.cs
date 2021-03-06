﻿using System.ComponentModel.DataAnnotations;

namespace ActiveCitizen.Model.StaticContent.Faq
{
    public class FaqListItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public virtual FaqListCategory Category { get; set; }
    }
}
