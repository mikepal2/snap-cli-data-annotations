using SnapCLI;
using SnapCLI.DataAnnotations;
using System.CommandLine;
using System.CommandLine.Builder;
using System.ComponentModel.DataAnnotations;

namespace Tests
{

    [TestClass]
    public class UnitTest1 : SnapCliUnitTest
    {
        [TestMethod]
        [DataRow("ann1", "The option 'opt1' must be between 1 and 10")]
        [DataRow("ann1 --opt1 1", "[ann1(1,test)]")]
        [DataRow("ann1 --opt1 10", "[ann1(10,test)]")]
        [DataRow("ann1 --opt1 11", "The option 'opt1' must be between 1 and 10")]
        [DataRow("ann1 --opt1 5 --opt2 aa", "minimum length of '3' and maximum length of '10'")]
        [DataRow("ann1 --opt1 5 --opt2 test!", "[ann1(5,test!)]")]
        public void Test(string commandLine, string pattern, UseExceptionHandler useExceptionHandler = UseExceptionHandler.Default)
        {
            TestCLI(commandLine, pattern, useExceptionHandler);
        }

        [Startup]
        public static void Startup()
        {
            CLI.BeforeCommand += (args) => args.ParseResult.ValidateDataAnnotations();
        }


        [Command(Name = "ann1")]
        public static void TestAnnotations1(
            [Range(1, 10)]
            int opt1 = 0,

            [Length(3, 10)]
            string opt2 = "test"
            )
        {
            TraceCommand(opt1, opt2);
        }
    }
}
