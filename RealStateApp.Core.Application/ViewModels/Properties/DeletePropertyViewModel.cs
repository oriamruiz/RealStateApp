﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.ViewModels.Properties
{
    public class DeletePropertyViewModel
    {
        public int Id { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
