using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
TSDR2JSON: Windows command-line program to obtain trademark information from the USPTO's TSDR system and present it in an easy-to-parse JSON format

Command is used as follows:

  TSDRJSON s nnnnnnnn : provide information on trademark application serial no. nnnnnnnn (8-digit number) in JSON format

or

  TSDRJSON r nnnnnnn : provide information on trademark registration no. nnnnnnn (7-digit number) in JSON format

For example, "TSDRJSON r 2564831" will generate a JSON-formatted dump of information on registration no. 2564831

Copyright 2014-2018 Terry Carroll
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

            // TEMP
            string APIKeyFromCommand = null;
            string APIKeyFromConfig = null;

            var useConfigFile = false;
            string configurationFilename = null;

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
                Console.ReadLine();
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

            Console.WriteLine($"(outer)config file: {configurationFilename}");

            Console.WriteLine($"configurationFilename is null: {configurationFilename == null}");
            Console.WriteLine($"configurationFilename is blank: {configurationFilename == ""}");
            Console.WriteLine($"useConfigFile:  {useConfigFile}");


            if (!registrationNumberSet && !serialNumberSet)
            {
                Console.WriteLine("No PTO registration or application serial no. provided.");
                SuggestHelp();
                return;
            }

            if (!bailOutEarly)
            {


                Console.WriteLine("Here's where I do stuff, if there is stuff to be done.");
                // temporary, to confirm using new library version
                Console.WriteLine($"config file: {configurationFilename}");

                var metainfo = Plumage.TSDRReq.GetMetainfo();
                Console.WriteLine(metainfo["MetaInfoLibraryVersion"]);
                System.Diagnostics.Debug.Assert(metainfo["MetaInfoLibraryVersion"] == "1.4.0");
                //

                if (useConfigFile)
                {
                    string fileToUse = configurationFilename ?? $"{programName.ToLower()}-config.json";
                    Console.WriteLine($"fileToUse: {fileToUse}");
                    Console.WriteLine($"fileToUse is null: {fileToUse == null}");
                    Console.WriteLine($"fileToUse is blank: {fileToUse == ""}");
                    string JSONConfigInfo = File.ReadAllText(fileToUse);
                    Dictionary<string, string> configDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(JSONConfigInfo);
                    if (configDict.ContainsKey("TSDRAPIKey")) { APIKeyFromConfig = configDict["TSDRAPIKey"]; }
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

                // trim out the "Diagnostic" stuff & use null strings instead of null-value for non-existent error messages
                if (t.TSDRData.TSDRMapIsValid)
                {
                    if (validity_dict["ErrorCode"] == null) validity_dict["ErrorCode"] = "";
                    if (validity_dict["ErrorMessage"] == null) validity_dict["ErrorMessage"] = "";
                    foreach (string key in t.TSDRData.TSDRSingle.Keys)
                    {
                        if (!key.StartsWith("Diag"))
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
                Console.WriteLine(json);
            }

            Console.WriteLine("finishing");
            Console.ReadLine();
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
                Console.WriteLine($"{programName} version {versionNumber}");
            }

            void SuggestHelp()
            {
                Console.WriteLine($"Try '{programName}  --help' for more information.");
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
