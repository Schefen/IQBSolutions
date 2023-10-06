using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class ExamResult
    {
        [Key] 
        public int Id { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
        public byte Score { get; set; }
    }
}
