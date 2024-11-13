using System;
using System.CommandLine.Parsing;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SnapCLI.DataAnnotations
{
    /// <summary>
    /// Enables command line parameters validation with <see cref="System.ComponentModel.DataAnnotations"/> attributes, when they applied to options and arguments binded with SnapAPI library.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Extention method to validate command line parameters data annotations 
        /// </summary>
        /// <param name="parseResult">Command line parse result</param>
        ///<usage>
        ///Add the following code to your CLI program to enable validation:
        ///     
        ///[Startup]
        ///public static void Startup()
        ///{
        ///    CLI.BeforeCommand += (args) => args.ParseResult.ValidateDataAnnotations();
        ///}
        ///</usage>

        public static void ValidateDataAnnotations(this ParseResult parseResult)
        {
            CommandResult? commandResult = parseResult.CommandResult;
            
            while (commandResult != null)
            {
                foreach (var symbolResult in commandResult.Children)
                {
                    switch (symbolResult)
                    {
                        case OptionResult optionResult:
                            ValidateDataAnnotations(optionResult.Option, optionResult.GetValueOrDefault(), $"option '{optionResult.Option.Name}'");
                            break;
                        case ArgumentResult argumentResult:
                            ValidateDataAnnotations(argumentResult.Argument, argumentResult.GetValueOrDefault(), $"argument <{argumentResult.Argument.Name}>");
                            break;
                        default:
                            continue;
                    }
                }

                commandResult = commandResult.Parent as CommandResult;
            }
        }

        private static void ValidateDataAnnotations(object cmdlineObject, object? value, string displayName)
        {
            var binding = CLI.GetBinding(cmdlineObject);
            if (binding is ICustomAttributeProvider customAttributeProvider)
            {
                var validationContext = new ValidationContext(binding) { DisplayName = "{displayName}" };
                foreach (var validationAttribute in customAttributeProvider.GetCustomAttributes(typeof(ValidationAttribute), true).OfType<ValidationAttribute>())
                {
                    ValidationResult? result = validationAttribute.GetValidationResult(value, validationContext);

                    if (result != null)
                    {
                        // replace default message 'field' reference to 'value'
                        if (result.ErrorMessage != null)
                        {
                            result.ErrorMessage = result.ErrorMessage
                                .Replace("{displayName} field", "{displayName}")
                                .Replace("field {displayName}", "{displayName}")
                                .Replace("{displayName}", displayName);
                        }

                        throw new ValidationException(result, validationAttribute, value);
                    }
                }
            }
        }
    }
}