using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MFFMS.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MFFMS.API.Dtos.TaiSanThietBiDto
{
    public class TaiSanThietBiForCreateDto:BaseDto
    {
        [Required]
        public int MaNhaCungCap{get;set;}
        
        [Required]
        public NhaCungCap NhaCungCap { get; set; } 

        [Required]
        public string TenTSTB { get; set; }

        [Required]
        public string TinhTrang {get;set;}

        [Required]
        public string ThongTinBaoHanh{get;set;}

    }
}
