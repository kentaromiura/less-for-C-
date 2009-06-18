using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace lessCompiler {
   public class lessConverter {

        public static string Convert(string less) {
            string output = replaceConstant(less);
            output = replaceMixins(output);
            return output.Trim();        
        }

        private static string prepareMixins(string input) {
            Regex rg = new Regex(@"\..+?;");
            Regex spaceBeforeSemicolon = new Regex(@"\s+?;");
            return rg.Replace(input, (match) =>
            {
                return spaceBeforeSemicolon.Replace(match.Value, ";");
            });
        }

        private static MatchEvaluator splitAndRemove(char separator, char remove, string returnFormat,IDictionary<string, string> repository) {
            return (match) =>
            {
                string[] namevalue = match.Value.Split(separator);
                string value = namevalue[1];
                repository.Add(namevalue[0].Trim(), value.Remove(value.LastIndexOf(remove)).Trim());
                return string.Format(returnFormat, match.Value);
            };
        }

        private static string replaceMixins(string input) {
            IDictionary<string, string> rules = new Dictionary<string, string>();

            Regex rg = new Regex(@"\..+?\s*?\{\s*?(.|\s)+?\}");
            rg.Replace(input, splitAndRemove('{', '}', "{0}", rules));

            string mixinPrepared = prepareMixins(input);
            return replaceMixinsWithRules(mixinPrepared, rules);
        }


        private static string replaceConstant(string input) {
            IDictionary<string, string> constants = new Dictionary<string, string>();
            Regex rg = new Regex(@"@\s*?[\w-]+?\s*?:\s*?.*?\s*?;");
            string inputWithoutConstantsDefinition = rg.Replace(input,splitAndRemove(':',';',"",constants));

            return replaceConstantWithValues(inputWithoutConstantsDefinition, constants);
        }

        private static string replaceKeysWithValues(string input, IDictionary<string, string> dictionary, string inputFormat, string outputFormat) {
            string finalString = input;
            foreach (string keys in dictionary.Keys) {
                finalString = finalString.Replace(string.Format(inputFormat, keys), string.Format(outputFormat, dictionary[keys]));
            }
            return finalString;
        }

        private static string replaceMixinsWithRules(string input, IDictionary<string, string> rules) {
            return replaceKeysWithValues(input, rules, "{0};", "{0}");
        }

        private static string replaceConstantWithValues(string inputWithoutConstantsDefinition, IDictionary<string, string> constants) {
            return replaceKeysWithValues(inputWithoutConstantsDefinition, constants, "{0};", "{0};");
        }

    }
}
