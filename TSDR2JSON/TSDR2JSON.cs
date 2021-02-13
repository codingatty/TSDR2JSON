using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
TSDR2JSON V1.1.0: Windows command-line program to obtain trademark information from the USPTO's TSDR system and present it in an easy-to-parse JSON format

Example use:

    TSDR2JSON -k="32CharacterAPIKeyYouGotFromUSPTO" -r=1234567
or
    TSDR2JSON --key="32CharacterAPIKeyYouGotFromUSPTO" --registration=1234567

Get TSDR information on USPTO trademark registration no. 1,234,567 using the API key specified.

An API key can be obtained for free from the PTO; see https://developer.uspto.gov/api-catalog/tsdr-data-api

Allowable options:
  -r, --registration=VALUE   trademark registration number to report
  -s, --serial=VALUE         trademark application serial number to report
  -k, --key=VALUE            USPTO-provided API key
  -c, --config[=VALUE]       Use a configuration file to specify the API key
  -o, --outfile[=VALUE]      Write the JSON information to a text file instead of displaying it;
                             The default filename is tsdr2json-output.json
  -h, --help                 show help infoemation and exit
  -v, --version              show version info and exit

For more infomartion, see https://github.com/codingatty/TSDR2JSON

Copyright 2019-2021 Terry Carroll
carroll@tjc.com

License information:
This program is licensed under Apache License, version 2.0 (January 2004); see http://www.apache.org/licenses/LICENSE-2.0
SPX-License-Identifier: Apache-2.0

 */

namespace TSDR2JSON
{
    class TSDR2JSON
    {
        static void Main(string[] args)
        {
            
            Boolean registrationNumberSet = false;
            Boolean serialNumberSet = false;
            string lookupType = null;
            string lookupNumber = null;

            var bailOutEarly = false;
            var shouldShowHelp = false;
            var shouldShowVersion = false;

            string programName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            string versionNumber = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            List<string> extra;

            // API Key stuff
            string APIKeyFromCommand = null;
            string APIKeyFromConfig = null;
            var useConfigFile = false;
            string configurationFilename = null;

            // output file
            var useOutputFile = false;
            string outputFilenameFromCommand = null;
            string outputFilename = null;

            var options = new Mono.Options.OptionSet {

                { "r|registration=", "trademark registration number to report", r => {
                     if (registrationNumberSet)
                     {
                         throw new Mono.Options.OptionException("Specify only one registration number", "-r/--registration");
                     }
                     if (serialNumberSet)
                     {
                         throw new Mono.Options.OptionException("Specify registration or serial no., but not both", "-r/--registration");
                     }
                     registrationNumberSet = true;
                     lookupType = "r";
                     lookupNumber  = r;

                     var lookupNumberisvalid = validateRequestNumber(lookupType, lookupNumber);
                     if (!lookupNumberisvalid)
                     {
                          throw new Mono.Options.OptionException($"Invalid registration number {lookupNumber}; must be exactly 7 digits", "-r/--registration");
                     }

                     }
                },
                { "s|serial=", "trademark application serial number to report",  s => {
                     if (serialNumberSet)
                     {
                         throw new Mono.Options.OptionException("Specify only one serial number", "-s/--serial");
                     }
                     if (registrationNumberSet)
                     {
                         throw new Mono.Options.OptionException("Specify registration or serial no., but not both", "-s/--serial");
                     }
                     serialNumberSet = true;
                     lookupType = "s";
                     lookupNumber = s;

                     var lookupNumberisvalid = validateRequestNumber(lookupType, lookupNumber);
                     if (!lookupNumberisvalid)
                     {
                          throw new Mono.Options.OptionException($"Invalid application serial number {lookupNumber}; must be exactly 8 digits", "-s/--serial");
                     }

                     }
                 },

                 { "k|key=", "USPTO-provided API key", k => APIKeyFromCommand = k},

                 { "c|config:", "Configuration file name", c => {
                     configurationFilename = c;
                     useConfigFile = true;
                     }
                 },
                
                 { "o|outfile:", "Output file name", c => {
                     outputFilenameFromCommand = c;
                     useOutputFile = true;
                     }
                 },

                 { "h|help", "show this message and exit", h => shouldShowHelp = h != null },

                 { "v|version", "show version info and exit", v => shouldShowVersion = v != null },

            };

            try
            {
                // parse the command line
                extra = options.Parse(args);
            }
            catch (Mono.Options.OptionException e)
            {
                // output some error message
                Console.Write($"Options error ({e.OptionName}): ");
                Console.WriteLine(e.Message);
                SuggestHelp();
                bailOutEarly = true;
                return;
            }

            if (shouldShowHelp)
            {
                ShowHelp(options);
                return;
            }

            if (shouldShowVersion)
            {
                ShowVersion();
                return;
            }

            if (extra.Count > 0)
            {
                ComplainExtraOperand(extra[0]);
                return;
            }

            if (!registrationNumberSet && !serialNumberSet)
            {
                Console.WriteLine("No PTO registration or application serial no. provided.");
                SuggestHelp();
                return;
            }

            if (!bailOutEarly)
            {

                if (useConfigFile)
                {
                    string fileToUse = configurationFilename ?? $"{programName.ToLower()}-config.json";
                    string JSONConfigInfo = File.ReadAllText(fileToUse);
                    Dictionary<string, string> configDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JSONConfigInfo);
                    if (configDict.ContainsKey("TSDRAPIKey")) { APIKeyFromConfig = configDict["TSDRAPIKey"]; }
                }

                if (useOutputFile)
                {
                    outputFilename = outputFilenameFromCommand ?? $"{programName.ToLower()}-out.json";
                }


                Plumage.TSDRReq t = new Plumage.TSDRReq();

                string APIKey = APIKeyFromCommand ??  APIKeyFromConfig; // use --key if specified, else from config file (or null)
                if (APIKey != null)
                {
                    t.setAPIKey(APIKey);
                }

                t.getTSDRInfo(lookupNumber, lookupType);
                var validity_dict = new Dictionary<string, string>()
                {
                    { "Success", t.TSDRData.TSDRMapIsValid.ToString() },
                    { "ErrorCode", t.ErrorCode },
                    { "ErrorMessage", t.ErrorMessage }
                };

                var output_TSDRSingle = new Dictionary<string, string>();
                var output_TSDRMulti = new Dictionary<string, List<Dictionary<string, string>>>();

                // trim out the "MetaInfo" stuff & use null strings instead of null-value for non-existent error messages
                if (t.TSDRData.TSDRMapIsValid)
                {
                    if (validity_dict["ErrorCode"] == null) validity_dict["ErrorCode"] = "";
                    if (validity_dict["ErrorMessage"] == null) validity_dict["ErrorMessage"] = "";
                    foreach (string key in t.TSDRData.TSDRSingle.Keys)
                    {
                        if (!key.StartsWith("MetaInfo"))
                        {
                            output_TSDRSingle[key] = t.TSDRData.TSDRSingle[key];
                        };
                    }
                    output_TSDRMulti = t.TSDRData.TSDRMulti;
                }

                var output_dict = new Dictionary<string, Object>()
            {
                { "SuccessInfo", validity_dict },
                { "TSDRSingle", output_TSDRSingle },
                { "TSDRMulti", output_TSDRMulti }
            };

                string json = JsonConvert.SerializeObject(output_dict, Formatting.Indented);
                if (useOutputFile)
                {
                    using (StreamWriter sw = new StreamWriter(outputFilename))
                    {
                        sw.Write(json);
                    }
                }
                else
                {
                    Console.WriteLine(json);
                }
            }

            return;

            void ShowHelp(Mono.Options.OptionSet p)
            {
                Console.WriteLine($"Usage: {programName} options");
                Console.WriteLine("Produce a JSON representation of TSDR data");
                Console.WriteLine();
                Console.WriteLine("Options:");
                p.WriteOptionDescriptions(Console.Out);
                Console.WriteLine();
                Console.WriteLine("For further information, see https://github.com/codingatty/TSDR2JSON");
            }

            void ShowVersion()
            {
                string tsdr2jsonInfo = $"{programName} version {versionNumber}";
                var metainfo = Plumage.TSDRReq.GetMetainfo();
                string plumageInfo = $"{metainfo["MetaInfoLibraryName"]} {metainfo["MetaInfoLibraryVersion"]}";
                Console.WriteLine($"{tsdr2jsonInfo} (Plumage library: {plumageInfo})");
            }

            void SuggestHelp()
            {
                Console.WriteLine($"Try '{programName} --help' for more information.");
            }
            
            void ComplainExtraOperand(string op)
            {
                Console.WriteLine($"Error: unrecognized operand {op}");
                SuggestHelp();

            }

            Boolean validateRequestNumber(string requested_type, string requested_number)
            {
                Boolean valid = true;
                if (requested_type == "s" && requested_number.Length != 8)
                {
                    valid = false;
                }
                if (requested_type == "r" && requested_number.Length != 7)
                {
                    valid = false;
                }
                if (!requested_number.All(Char.IsDigit))
                {
                    valid = false;
                }
                return valid;
            }
        }
    }
}
