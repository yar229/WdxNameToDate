using TcPluginBase;
using TcPluginBase.Content;
using YaR.TotalCommander.Wdx.NameToDate.Fields;

namespace YaR.TotalCommander.Wdx.NameToDate
{
    public class Content : ContentPlugin
    {
        private static readonly TcField[] Fields =
        {
            new TcFieldFileType{ Name = "FileType" },
            new TcFieldFileSize{ Name = "Size_F" },
            new TcFieldCreateDateTime(ContentFieldType.Date) { Name = "CreateDate" },
            new TcFieldCreateDateTime(ContentFieldType.Time) { Name = "CreateTime" },
            new TcFieldCreateDateTime(ContentFieldType.DateTime) { Name = "CreateDateTime" },
            new TcFieldFilenameAsDate("yyyy-MM-dd ddd") { Name = "2021-01-28 Wed" },
        };

        private int FieldCount => Fields.Length;


        #region Constructors

        public Content(Settings pluginSettings)
            : base(pluginSettings) 
        {
            Title = "ContentZ";
            //z DetectString = "!(EXT=\"TXT\" | EXT=\"PDF\")";
        }

        #endregion

        #region IContentPlugin Members

        public override ContentFieldType GetSupportedField(int fieldIndex, out string fieldName, out string units, int maxLen) 
        {
            fieldName = string.Empty;
            units = string.Empty;
            
            if (fieldIndex < 0 || fieldIndex >= FieldCount)
                return ContentFieldType.NoMoreFields;

            var field = Fields[fieldIndex];

            fieldName = field.Name;
            if (fieldName.Length > maxLen)
                fieldName = fieldName.Substring(0, maxLen);

            units = field.Unit;
            if (units.Length > maxLen)
                units = units.Substring(0, maxLen);

            return field.ContentType;
        }

        public override GetValueResult GetValue(string fileName, int fieldIndex, int unitIndex,
                int maxLen, GetValueFlags flags, out string fieldValue, out ContentFieldType fieldType) 
        {
            getAborted = false;
            GetValueResult result = GetValueResult.FieldEmpty;
            fieldType = ContentFieldType.NoMoreFields;
            fieldValue = null;

            if (string.IsNullOrEmpty(fileName))
                return result;

            if (fieldIndex < 0 || fieldIndex > FieldCount - 1)
                return GetValueResult.NoSuchField;

            var field = Fields[fieldIndex];
            if (null == field)
                return GetValueResult.NoSuchField;

            fieldType = field.ContentType;

            var res = field.GetValue(fileName, flags, ref getAborted);
            fieldValue = res.Value;
            
            return res.Result;
        }

        


        public override void StopGetValue(string fileName) 
        {
            getAborted = true;
        }

        public override DefaultSortOrder GetDefaultSortOrder(int fieldIndex) 
        {
            var field = Fields[fieldIndex];
            return field?.SortOrder ?? DefaultSortOrder.Asc;
        }

        //public override  void PluginUnloading()
        //{
        //    // do nothing
        //}

        public override SupportedFieldOptions GetSupportedFieldFlags(int fieldIndex) 
        {
            if (fieldIndex == -1)
                return SupportedFieldOptions.Edit | SupportedFieldOptions.SubstMask;

            var field = Fields[fieldIndex];
            return field?.Options ?? SupportedFieldOptions.None;
        }

        //public override SetValueResult SetValue(string fileName, int fieldIndex, int unitIndex,
        //        ContentFieldType fieldType, string fieldValue, SetValueFlags flags) 
        //{
        //    if (string.IsNullOrEmpty(fileName) && fieldIndex < 0)    // change attributes operation has ended
        //        return SetValueResult.NoSuchField;
        //    if (string.IsNullOrEmpty(fieldValue))
        //        return SetValueResult.NoSuchField;

        //    SetValueResult result = SetValueResult.NoSuchField;
        //    DateTime created = DateTime.Parse(fieldValue);
        //    bool dateOnly = (flags & SetValueFlags.OnlyDate) != 0;
        //    if (Directory.Exists(fileName)) {
        //        DirectoryInfo dirInfo = new DirectoryInfo(fileName);
        //        if (SetCombinedDateTime(ref created, dirInfo.CreationTime, fieldType, dateOnly)) {
        //            Directory.SetCreationTime(fileName, created);
        //            result = SetValueResult.Success;
        //        }
        //    } else if (File.Exists(fileName)) {
        //        FileInfo fileInfo = new FileInfo(fileName);
        //        if (SetCombinedDateTime(ref created, fileInfo.CreationTime, fieldType, dateOnly)) {
        //            File.SetCreationTime(fileName, created);
        //            result = SetValueResult.Success;
        //        }
        //    } else
        //        result = SetValueResult.FileError;
        //    return result;
        //}

        public override bool GetDefaultView(out string viewContents, out string viewHeaders, out string viewWidths,
                out string viewOptions, int maxLen) 
        {
            viewContents = "[=<fs>.FileType]\\n[=<fs>.Size_F]\\n[=<fs>.CreateDateTime]";
            viewHeaders = "File Type\\nSize(Float)\\nCreated";
            viewWidths = "80,30,80,-80,-80";
            viewOptions = "-1|1";
            return true;
        }

        public override void SendStateInformation(StateChangeInfo state, string path) 
        {
            // do nothing, just for trace
        }

        public override ContentCompareResult CompareFiles(int compareIndex, string fileName1, 
                string fileName2, ContentFileDetails fileDetails, out int iconResourceId) 
        {
            iconResourceId = -1;
            // You can call ContentProgressProc(int) here to inform TC about comparing progress. 
            return ContentCompareResult.CannotCompare;
        }

        #endregion IContentPlugin Members

        #region Private Methods

        private bool getAborted;

        //private static bool SetCombinedDateTime(ref DateTime newTime, DateTime currentTime,
        //        ContentFieldType fieldType, bool dateOnly) {
        //    bool result = true;
        //    switch (fieldType) {
        //        case ContentFieldType.DateTime:
        //            if (dateOnly) {
        //                newTime = new DateTime(
        //                    newTime.Year, newTime.Month, newTime.Day,
        //                    currentTime.Hour, currentTime.Minute, currentTime.Second);
        //            }
        //            break;
        //        case ContentFieldType.Date:
        //            newTime = new DateTime(
        //                newTime.Year, newTime.Month, newTime.Day,
        //                currentTime.Hour, currentTime.Minute, currentTime.Second);
        //            break;
        //        case ContentFieldType.Time:
        //            newTime = new DateTime(
        //                currentTime.Year, currentTime.Month, currentTime.Day,
        //                newTime.Hour, newTime.Minute, newTime.Second);
        //            break;
        //        default:
        //            result = false;
        //            break;
        //    }
        //    return result;
        //}





        #endregion Private Methods
    }

}
