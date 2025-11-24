using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inova.Application.DTOs.Specialization
{
    public class SpecializationUpdateRequestDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public int ConsultantCount { get; set; }

    }
}
