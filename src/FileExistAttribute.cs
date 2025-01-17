using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SnapCLI.DataAnnotations
{
    /// <summary>
    /// Validates file exist
    /// </summary>
    public sealed class FileExistAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        public bool AllowNullValue { get; set; }
        /// <inheritdoc/>
        public override bool RequiresValidationContext => false;
        /// <inheritdoc/>
        public override bool IsValid(object? value)
        {
            if (value == null)
                return AllowNullValue;

            switch (value)
            {
                case string str:
                    return File.Exists(str);
                case FileInfo fileInfo:
                    return fileInfo.Exists;
                default:
                    throw new NotSupportedException($"The {nameof(FileExistAttribute)} doesn't support validation of {value.GetType()}");
            }
        }
    }
}