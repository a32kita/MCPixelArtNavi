﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPArtNavi.Importer.ImageImporterInternals
{
    internal static class MathEx
    {
        public static double ToRadians(double angle)
        {
            return (double)(angle * Math.PI / 180);
        }
    }
}
