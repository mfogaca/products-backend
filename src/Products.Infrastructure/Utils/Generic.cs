﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValidaDoc.Infrastructure.Utils
{
    public class Generic
    {

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
