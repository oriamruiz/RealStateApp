using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Account
{
    public class UpdateImageRequest
    {
        public string Id { get; set; }
        public string AccountImgUrl { get; set; }
    }
}
