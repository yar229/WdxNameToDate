using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using TcPluginBase.Content;

namespace YaR.TotalCommander.Wdx.NameToDate.Fields
{
    internal class TcFieldFileSize : TcField
    {
        public TcFieldFileSize()
        {
            SortOrder = DefaultSortOrder.Asc;
        }

        public override ContentFieldType ContentType => ContentFieldType.NumericFloating;

        public override ValueResult GetValue(string fileName, GetValueFlags flags, ref bool getAborted)
        {
            if (Directory.Exists(fileName))
            {
                if ((flags & GetValueFlags.DelayIfSlow) != 0) 
                    return ValueResult.OnDemand("?");

                try 
                {
                    long size = GetDirectorySize(fileName, ref getAborted);
                    return ValueResult.Success(GetSizeValue(size));
                } catch (IOException) 
                {
                    // Directory changed, stop long operation
                    return ValueResult.FieldEmpty();
                }
            }
            
            if (File.Exists(fileName))
            {
                var info = new FileInfo(fileName);
                return ValueResult.Success(GetSizeValue(info.Length));
            }

            return new ValueResult { Result = GetValueResult.FileError, Value = string.Empty };
        }

        private static string GetSizeValue(long size) 
        {
            string result = size.ToString(CultureInfo.InvariantCulture);
            double dSize = size;
            string altStr = null;
            if (dSize > 1024.0) {
                dSize /= 1024.0;
                altStr = $"|{dSize:0} Kb";
            }
            if (dSize > 1024.0) {
                dSize /= 1024.0;
                altStr = $"|{dSize:0} Mb";
            }
            if (dSize > 1024.0) {
                dSize /= 1024.0;
                altStr = $"|{dSize:0} Gb";
            }
            if (!string.IsNullOrEmpty(altStr))
                result += altStr;
            return result;
        }

        private static long GetDirectorySize(string dirPath, ref bool getAborted) 
        {
            if (getAborted)
                throw new IOException();

            long dirSize = 0;
            try 
            {
                var di = new DirectoryInfo(dirPath);
                dirSize += di.GetFiles().Sum(fi => fi.Length);
                foreach (var cd in di.GetDirectories())
                    dirSize += GetDirectorySize(cd.FullName, ref getAborted);
            } 
            catch (SecurityException) 
            { 
            }
            
            return dirSize;
        }
    }
}
