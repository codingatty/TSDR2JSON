﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            Boolean parms_valid;
            string requested_type;
            string requested_number;
            string arguments_passed = "";
            string exception_msg;

            // TEMP
            string APIKEY = ""; // placeholder for API Key

            for (int i = 0; i < args.Length; i++)
            {
                arguments_passed = arguments_passed + args[i] + " ";
            }
            if (arguments_passed.Length > 0) {
                arguments_passed = arguments_passed.Substring(0, arguments_passed.Length - 1);
            }
            parms_valid = validateParameters(args);
            if (!parms_valid)
            {
                string exception_msg_template =
                    "Error; exactly two arguments are required. Either:\n" +
                    "   's' followed by eight-digit serial no. for application; or\n" +
                    "   'r' followed by seven-digit registration no.\n" +
                    "Example: TSDR2JSON r 1234567\n" +
                    "The arguments provided were: \"{0}\"";
                exception_msg = String.Format(exception_msg_template, arguments_passed);
                throw new ArgumentException(exception_msg);
            }
            requested_type = args[0];
            requested_number = args[1];

            // temporary, to confirm using new library version
            var metainfo = Plumage.TSDRReq.GetMetainfo();
            Console.WriteLine(metainfo["MetaInfoLibraryVersion"]);
            System.Diagnostics.Debug.Assert(metainfo["MetaInfoLibraryVersion"] == "1.4.0");
            //

            Plumage.TSDRReq t = new Plumage.TSDRReq();
            t.setAPIKey(APIKEY);

            t.getTSDRInfo(requested_number, requested_type);
            var validity_dict = new Dictionary<string, string>()
            {
                { "Success", t.TSDRData.TSDRMapIsValid.ToString() },
                { "ErrorCode", t.ErrorCode },
                { "ErrorMessage", t.ErrorMessage }
            };

            var output_TSDRSingle = new Dictionary<string, string>();
            var output_TSDRMulti = new Dictionary<string, List<Dictionary<string,string>>>();

            // trim out the "Diagnostic" stuff & use null strings instead of null-value for non-existent error messages
            if (t.TSDRData.TSDRMapIsValid)
            {
                if (validity_dict["ErrorCode"] == null)  validity_dict["ErrorCode"] = "";
                if (validity_dict["ErrorMessage"] == null)  validity_dict["ErrorMessage"] = "";
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

        static Boolean validateParameters(string[] args)
        {
            Boolean valid;
            string requested_type;
            string requested_number;

            valid = validateNumberOfParameters(args);
            if (valid){
                requested_type = args[0];
                valid = validateRequestType(requested_type);
            }
            if (valid)
            {
                requested_type = args[0];
                requested_number = args[1];
                valid = validateRequestNumber(requested_type, requested_number);
            }
            return valid;
        }

        static Boolean validateNumberOfParameters(string[] args)
        {
            Boolean valid = true;
            if (args.Length != 2)
            {
                valid = false;
            }
            return valid;
        }

        static Boolean validateRequestType(string requested_type)
        {
            Boolean valid = true;
            List<string> valid_types = new List<string>();
            valid_types.Add("s");
            valid_types.Add("r");
            if (!valid_types.Contains(requested_type))
                {
                valid = false;
                }
            return valid;
        }
        static Boolean validateRequestNumber(string requested_type, string requested_number)
        {
            Boolean valid = true;
            if (requested_type == "s" && requested_number.Length !=8){
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
