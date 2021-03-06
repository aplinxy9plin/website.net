﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActiveCitizen.Model.StaticContent.Faq
{
    public class FaqListCategory
    {
        public FaqListCategory()
        {
            Items = new HashSet<FaqListItem>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        public virtual ICollection<FaqListItem> Items { get; set; }
    }
}
