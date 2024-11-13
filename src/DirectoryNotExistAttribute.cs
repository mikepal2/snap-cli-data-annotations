using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SnapCLI.DataAnnotations
{
    sealed class DirectoryNotExistAttribute : ValidationAttribute
    {
        public bool AllowNullValue { get; set; }
        public override bool RequiresValidationContext => false;
        public override bool IsValid(object? value)
        {
            if (value == null)
                return AllowNullValue;

            switch (value)
            {
                case string str:
                    return !Directory.Exists(str);
                case DirectoryInfo DirectoryInfo:
                    return !DirectoryInfo.Exists;
                default:
                    throw new NotSupportedException($"The {nameof(DirectoryNotExistAttribute)} doesn't support validation of {value.GetType()}");
            }
        }
    }
}