using System.IO;
using TcPluginBase.Content;

namespace YaR.TotalCommander.Wdx.NameToDate.Fields
{
    class TcFieldFileType : TcField
    {
        public override ContentFieldType ContentType => ContentFieldType.WideString;

        public override ValueResult GetValue(string fileName, GetValueFlags flags, ref bool getAborted)
        {
            if (Directory.Exists(fileName))
                return ValueResult.Success("Folder");
            
            if (File.Exists(fileName))
            {
                var info = new FileInfo(fileName);

                string fileType;
                switch (info.Extension.ToLower()) {
                    case ".exe":
                    case ".dll":
                    case ".sys":
                    case ".com":
                        fileType = "Program";
                        break;
                    case ".zip":
                    case ".rar":
                    case ".cab":
                    case ".7z":
                        fileType = "Archive";
                        break;
                    case ".bmp":
                    case ".jpg":
                    case ".png":
                    case ".gif":
                        fileType = "Image";
                        break;
                    case ".mp3":
                    case ".avi":
                    case ".wav":
                        fileType = "Multimedia";
                        break;
                    case ".htm":
                    case ".html":
                        fileType = "Web Page";
                        break;
                    default:
                        fileType = "File";
                        break;
                }
                return ValueResult.Success(fileType);
            };

            return new ValueResult { Result = GetValueResult.FileError, Value = string.Empty };
        }
    }
}
