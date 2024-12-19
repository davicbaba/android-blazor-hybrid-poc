﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidNetMauiPoC.Service
{
    internal interface IServiceUnderTest
    {
        Task ExecuteAsync();

        Task<string> GetAppSettings();
    }
}
