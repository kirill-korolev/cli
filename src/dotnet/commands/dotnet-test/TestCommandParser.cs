using System.Collections.Generic;
using System.Linq;
using Microsoft.DotNet.Cli.CommandLine;
using LocalizableStrings = Microsoft.DotNet.Tools.Test.LocalizableStrings;

namespace Microsoft.DotNet.Cli
{
    internal static class TestCommandParser
    {
        public static Command Test() =>
            Create.Command(
                  "test",
                  LocalizableStrings.AppFullName,
                  Accept.ZeroOrMoreArguments()
                        .With(name: LocalizableStrings.CmdArgProject,
                              description: LocalizableStrings.CmdArgDescription),
                  CommonOptions.HelpOption(),
                  Create.Option(
                        "-s|--settings",
                        LocalizableStrings.CmdSettingsDescription,
                        Accept.ExactlyOneArgument()
                              .With(name: LocalizableStrings.CmdSettingsFile)
                              .ForwardAsSingle(o => $"/p:VSTestSetting={o.Arguments.Single()}")),
                  Create.Option(
                        "-t|--list-tests",
                        LocalizableStrings.CmdListTestsDescription,
                        Accept.NoArguments()
                              .ForwardAsSingle(o => "/p:VSTestListTests=true")),
                  Create.Option(
                        "--filter",
                        LocalizableStrings.CmdTestCaseFilterDescription,
                        Accept.ExactlyOneArgument()
                              .With(name: LocalizableStrings.CmdTestCaseFilterExpression)
                              .ForwardAsSingle(o => $"/p:VSTestTestCaseFilter={o.Arguments.Single()}")),
                  Create.Option(
                        "-a|--test-adapter-path",
                        LocalizableStrings.CmdTestAdapterPathDescription,
                        Accept.ExactlyOneArgument()
                              .With(name: LocalizableStrings.CmdTestAdapterPath)
                              .ForwardAsSingle(o => $"/p:VSTestTestAdapterPath={o.Arguments.Single()}")),
                  Create.Option(
                        "-l|--logger",
                        LocalizableStrings.CmdLoggerDescription,
                        Accept.ExactlyOneArgument()
                              .With(name: LocalizableStrings.CmdLoggerOption)
                              .ForwardAsSingle(o => 
                                    {
                                          var loggersString = string.Join(";", GetSemiColonEscapedArgs(o.Arguments)); 
                                          
                                          return $"/p:VSTestLogger={loggersString}";
                                    })),
                  CommonOptions.ConfigurationOption(),
                  CommonOptions.FrameworkOption(),
                  Create.Option(
                        "-o|--output",
                        LocalizableStrings.CmdOutputDescription,
                        Accept.ExactlyOneArgument()
                              .With(name: LocalizableStrings.CmdOutputDir)
                              .ForwardAsSingle(o => $"/p:OutputPath={o.Arguments.Single()}")),
                  Create.Option(
                        "-d|--diag",
                        LocalizableStrings.CmdPathTologFileDescription,
                        Accept.ExactlyOneArgument()
                              .With(name: LocalizableStrings.CmdPathToLogFile)
                              .ForwardAsSingle(o => $"/p:VSTestDiag={o.Arguments.Single()}")),
                  Create.Option(
                        "--no-build",
                        LocalizableStrings.CmdNoBuildDescription,
                        Accept.NoArguments()
                              .ForwardAsSingle(o => "/p:VSTestNoBuild=true")),
                  CommonOptions.VerbosityOption());

        private static string GetSemiColonEsacpedstring(string arg)
        {
            if (arg.IndexOf(";") != -1)
            {
                return arg.Replace(";", "%3b");
            }

            return arg;
        }

        private static string[] GetSemiColonEscapedArgs(IReadOnlyCollection<string> args)
        {
            int counter = 0;
            string[] array = new string[args.Count];

            foreach (string arg in args)
            {
                array[counter++] = GetSemiColonEsacpedstring(arg);
            }

            return array;
        }
    }
}