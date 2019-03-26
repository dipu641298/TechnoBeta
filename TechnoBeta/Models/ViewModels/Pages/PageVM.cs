using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TechnoBeta.Models.Data;
using System.Web.Mvc;

namespace TechnoBeta.Models.ViewModels.Pages
{
    public class PageVM
    {
        public PageVM()
        {

        }

        public PageVM(PageDTO row)
        {
            Id = row.Id;
            Title = row.Title;
            Body = row.Body;
            Slug = row.Slug;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar;

        }

        public int Id { get; set; }
        [Required]
        [StringLength (50, MinimumLength = 3)]
        public string Title { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        [AllowHtml]
        public string Body { get; set; }
        public bool HasSidebar { get; set; }
    }
}