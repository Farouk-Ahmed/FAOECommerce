using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.DataAccess.Models
{
    public class ProductImagesDto
    {
        public int ProductId { get; set; }
        public List<string> ImageUrls { get; set; }

    }
}
