using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SnapCLI.DataAnnotations
{
    /// <summary>
    /// Validates directory not exist
    /// </summary>
    public sealed class DirectoryNotExistAttribute : ValidationAttribute
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
                    return !Directory.Exists(str);
                case DirectoryInfo DirectoryInfo:
                    return !DirectoryInfo.Exists;
                default:
                    throw new NotSupportedException($"The {nameof(DirectoryNotExistAttribute)} doesn't support validation of {value.GetType()}");
            }
        }
    }
}