﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;



namespace ViFlix.Core.Tools
{
    public static class ImageValidator
    {
        public static bool IsImage(this IFormFile file)
        {
            try
            {
                //var img = System.Drawing.Image.FromStream(file.OpenReadStream());
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
