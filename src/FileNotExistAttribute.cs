using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SnapCLI.DataAnnotations
{
    sealed class FileNotExistAttribute : ValidationAttribute
    {
        public bool AllowNullValue {  get; set; }
        public override bool RequiresValidationContext => false;
        public override bool IsValid(object? value)
        {
            if (value == null)
                return AllowNullValue;

            switch (value)
            {
                case string str:
                    return !File.Exists(str);
                case FileInfo fileInfo:
                    return !fileInfo.Exists;
                default:
                    throw new NotSupportedException($"The {nameof(FileNotExistAttribute)} doesn't support validation of {value.GetType()}");
            }
        }
    }
}