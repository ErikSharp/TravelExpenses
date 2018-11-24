using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelExpenses.Common
{
    public static class ByteCount
    {
        public const long KB = 1024;
        public const long MB = 1024 * 1024;
        public const long GB = 1024 * 1024 * 1024;
        public const long TB = 1024L * 1024 * 1024 * 1024;

        public static string GetByteCountString(this long bytes)
        {
            if (bytes < 0)
                throw new ArgumentOutOfRangeException(nameof(bytes), "Must be a positive number or zero");

            if (bytes < KB)
            {
                return $"{bytes.ToString("N0")} Bytes";
            }
            else if (bytes < MB)
            {
                var kb = (double)bytes / KB;
                return $"{RemoveTrailingZerosOrPeriod(kb.ToString("N" + GetPrecision(bytes, KB)))} KB";
            }
            else if (bytes < GB)
            {
                var mb = (double)bytes / MB;
                return $"{RemoveTrailingZerosOrPeriod(mb.ToString("N" + GetPrecision(bytes, MB)))} MB";
            }
            else if (bytes < TB)
            {
                var gb = (double)bytes / GB;
                return $"{RemoveTrailingZerosOrPeriod(gb.ToString("N" + GetPrecision(bytes, GB)))} GB";
            }
            else
            {
                var tb = (double)bytes / TB;
                return $"{RemoveTrailingZerosOrPeriod(tb.ToString("N2"))} TB";
            }

        }

        public static string GetByteCountString(this int bytes)
        {
            return ((long)bytes).GetByteCountString();
        }

        private static int GetPrecision(long bytes, long threshold)
        {
            int precision;
            if (bytes < threshold * 2)
            {
                precision = 2;
            }
            else if (bytes < threshold * 10)
            {
                precision = 1;
            }
            else
            {
                precision = 0;
            }

            return precision;
        }

        private static string RemoveTrailingZerosOrPeriod(string input)
        {
            if (!input.Contains('.'))
                return input;

            while (input.Last() == '0' || input.Last() == '.')
            {
                input = input.Substring(0, input.Length - 1);
            }

            return input;
        }
    }
}