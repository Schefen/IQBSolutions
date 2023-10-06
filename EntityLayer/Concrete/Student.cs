﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Full_Name { get; set; }
        [StringLength(20)]
        public string Number { get; set; }
        [StringLength(3)]
        public string Age { get; set; }
        [StringLength(30)]
        public string Gsm_Number { get; set; }
    }
}
