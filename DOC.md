TSDR2JSON is a Windows command-line program to obtain data about a trademark registration or tradmark rgistraion application from the Trademark Status and Document Retrieaval (TSDR) system at the United States Patent and Trademark Office (USPTO), and provide it in an easily-parsable JSON format.
## Synopsis
```
Usage: TSDR2JSON options
Produce a JSON representation of TSDR data

Options:
  -r, --registration=VALUE   trademark registration number to report
  -s, --serial=VALUE         trademark application serial number to report
  -k, --key=VALUE            USPTO-provided API key
  -c, --config[=VALUE]       Configuration file name
  -o, --outfile[=VALUE]      Output file name
  -h, --help                 show this message and exit
  -v, --version              show version info and exit
```
## Options
### -r, --registration
For registered trademarks, you can specify the seven-digit registration number, with no punctuation,  of the trademark whose information you want. For example, to see information on registration no. 2,564,831, specify either `-r=2564831` (short form) or `--registration=2564831` (long form). The number is *required*.

A registered trademark can also be looked up by its application serial number (see **-s, --serial**, below). For example, registration no. 2,564,831 was assigned application serial no. 75/181,334 when first applied for, so it can be looked up under that serial no.
### -s, --serial
For both registered and unregistered trademarks, you can specify the eight-digit application serial number, with no punctuation,  of the trademark whose information you want. For example, to see information on application serial no. 75/181,334, specify either `-s=75181334` (short form) or `--serial=75181334` (long form). The number is *required*.

This is the only option to retrieved information on applications that have not proceeded to registration and therefore do not have a registration number.
### -k, --key
In October 2020, the USPTO  instituted a requirement that [calls to TSDR must specify an API key](https://developer.uspto.gov/api-catalog/tsdr-data-api ). Invocations of TSDR2JSON that do not specify an API key will fail. To specify the API key to be used for your TSDR call, use the `-k=` (short form) or `--key=` (long form) option.

For example, if your API key is "32CharacterAPIKeyYouGotFromUSPTO", you would using the option `--key="32CharacterAPIKeyYouGotFromUSPTO"` (with or without the quotes).

The `--key` is not *required* by TSDR2JSON, but under the PTO's current configuration, calls that don't specify an API key will fail as 401 Not Authorized. In the (admittedly unlikely) event the PTO drops the API key requirement, TSDR2JSON will work again without it, so the program does not make it mandatory.

There is no charge for the API key, but you must register with the USPTO to obtain one. 

For convenience, instead of typing an awkward 32-character string on the command line, you can put your API key in a configuration file and use that instead; see "**-c, --config**", below.
### -c, --config
You can use the  `-c` (short form) or `--config` (long form) option to specify a configuration file for TSDR2JSON. Currently, the configuration file is used only to specify the API key, as a convenient alternative to having to type it on every command line. That may change in future releases.

The configuration file is a JSON-format file consisting of key-value pairs. For example:
```
{
"TSDRAPIKey": "32CharacterAPIKeyYouGotFromUSPTO",
"TSDRAPIKeyExpirationDate": "2022-02-01"
}
```
Curently, only the `TSDRAPIKey` key-value pair is used; the `TSDRAPIKeyExpirationDate` key shown here will not generate an error, but it is ignored.

If the config option is provided with no operands (i.e. `-c` or `--config`), the default configuration file name is `tsdr2json-config.json`, in the directory where TSDR2JSON is executing. This can be overridden by specifying a filename; for example:

    --config=otherfilename.txt
or

    -c=D:\MyFiles\tsdr2json-info.json	
will respectively cause TSDR2JSON to use the file `otherfilename.txt` (in the current directory); or `D:\MyFiles\tsdr2json-info.json`, as indicated.
### -o, --outfile
By default, TSDR2JSON displays its output on the screen. To instead redirect it to a file, use the `-o` (short form) or `--outfile` (long form) option. If no filename is indicated, the output will be directed to a file named `tsdr2json-out.json`. If a filename is specified (e.g., `-o=my-tsdr-report.txt` or `--outfile=D:\MyFiles\tsdr2json-report.json`), the output will be redierected to the sppecified file.
### -h, --help
The `-h` (short form) or `--help` (long form) option will display short help text on how to use TSDR2JSON. Other options are ignored.
### -v, --version    
The `-v` (short form) or `--version` (long form) option will display the version number of TSDR2JSON and of the underlying Plumage library. Other options are ignored.
## Sample output
The following shows the output for registration no. 2,564,831:
```
{
  "SuccessInfo": {
    "Success": "True",
    "ErrorCode": "",
    "ErrorMessage": ""
  },
  "TSDRSingle": {
    "MarkCurrentStatusDate": "2009-02-07",
    "MarkCurrentStatusDateTruncated": "2009-02-07",
    "ApplicationNumber": "75181334",
    "ApplicationDate": "1996-10-15-04:00",
    "ApplicationDateTruncated": "1996-10-15",
    "RegistrationNumber": "2564831",
    "RegistrationDate": "2002-04-30",
    "RegistrationDateTruncated": "2002-04-30",
    "MarkVerbalElementText": "MONTY PYTHON'S FLYING CIRCUS",
    "MarkCurrentStatusExternalDescriptionText": "Registration cancelled because registrant did not file an acceptable declaration under Section 8.  To view all documents in this file, click on the Trademark Document Retrieval link at the top of this page.",
    "RegisterCategory": "Principal",
    "RenewalDate": "",
    "RenewalDateTruncated": "",
    "LawOfficeAssignedText": "LAW OFFICE 102",
    "CurrentLocationCode": "40S",
    "CurrentLocationText": "SCANNING ON DEMAND",
    "CurrentLocationDate": "2007-08-21-04:00",
    "CurrentLocationDateTruncated": "2007-08-21",
    "PublicationDate": "2002-02-05",
    "PublicationDateTruncated": "2002-02-05",
    "CorrespondentName": "I MORLEY DRUCKER",
    "CorrespondentOrganization": "FULWIDER PATTON LEE & UTECHT LLP",
    "CorrespondentAddressLine01": "HOWARD HUGHES CTR",
    "CorrespondentAddressLine02": "6060 CTR DR 10TH FL",
    "CorrespondentAddressCity": "LOS ANGELES",
    "CorrespondentAddressGeoRegion": "CALIFORNIA",
    "CorrespondentPostalCode": "90045",
    "CorrespondentCountryCode": "US",
    "CorrespondentCombinedAddress": "HOWARD HUGHES CTR/6060 CTR DR 10TH FL/LOS ANGELES/CALIFORNIA/90045/US",
    "CorrespondentPhoneNumber": "",
    "CorrespondentFaxNumber": "",
    "CorrespondentEmailAddress": ""
  },
  "TSDRMulti": {
    "InternationalClassDescriptionList": [
      {
        "InternationalClassNumber": "025",
        "GoodsServicesDescription": "clothing for men, women, and children, namely, shirts, jackets, sweaters, pants, footwear, belts, T-shirts, socks, coordinated shirts, jackets and slacks, tennis shoes, sweat shirts, jerseys, shorts, jogging suits, sweat pants, hats/caps, scarves, gloves, hosiery, neckties, rainwear, pajamas, robes, night shirts, thermal underwear, headbands, wristbands and Halloween costumes"
      }
    ],
    "DomesticClassDescriptionList": [
      {
        "PrimaryClassNumber": "025",
        "NiceClassNumber": "025",
        "ClassificationKindCode": "Domestic",
        "NationalClassNumber": "022"
      },
      {
        "PrimaryClassNumber": "025",
        "NiceClassNumber": "025",
        "ClassificationKindCode": "Domestic",
        "NationalClassNumber": "039"
      }
    ],
    "FirstUseDateList": [
      {
        "PrimaryClassNumber": "025",
        "NiceClassNumber": "025",
        "FirstUseDateNumber": "19761201",
        "FirstUseInCommerceDate": "19761201"
      }
    ],
    "ApplicantList": [
      {
        "ApplicantName": "Python (Monty) Pictures Ltd.",
        "ApplicantDescription": "ORIGINAL REGISTRANT",
        "ApplicantAddressLine01": "Room 537/538, The Linen Hall",
        "ApplicantAddressLine02": "162/168 Regent Street",
        "ApplicantAddressCity": "London",
        "ApplicantAddressGeoRegion": "ENGLAND",
        "ApplicantPostalCode": "W1R 5TB",
        "ApplicantCountryCode": "GB",
        "ApplicantCombinedAddress": "Room 537/538, The Linen Hall/162/168 Regent Street/London/ENGLAND/W1R 5TB/GB"
      },
      {
        "ApplicantName": "Python (Monty) Pictures Ltd.",
        "ApplicantDescription": "OWNER AT PUBLICATION",
        "ApplicantAddressLine01": "Room 537/538, The Linen Hall",
        "ApplicantAddressLine02": "162/168 Regent Street",
        "ApplicantAddressCity": "London",
        "ApplicantAddressGeoRegion": "ENGLAND",
        "ApplicantPostalCode": "W1R 5TB",
        "ApplicantCountryCode": "GB",
        "ApplicantCombinedAddress": "Room 537/538, The Linen Hall/162/168 Regent Street/London/ENGLAND/W1R 5TB/GB"
      },
      {
        "ApplicantName": "Python (Monty) Pictures Ltd.",
        "ApplicantDescription": "ORIGINAL APPLICANT",
        "ApplicantAddressLine01": "Room 537/538, The Linen Hall",
        "ApplicantAddressLine02": "162/168 Regent Street",
        "ApplicantAddressCity": "London",
        "ApplicantAddressGeoRegion": "ENGLAND",
        "ApplicantPostalCode": "W1R 5TB",
        "ApplicantCountryCode": "GB",
        "ApplicantCombinedAddress": "Room 537/538, The Linen Hall/162/168 Regent Street/London/ENGLAND/W1R 5TB/GB"
      }
    ],
    "MarkEventList": [
      {
        "MarkEventDate": "2009-02-07",
        "MarkEventDateTruncated": "2009-02-07",
        "MarkEventDescription": "CANCELLED SEC. 8 (6-YR)",
        "MarkEventEntryNumber": "22"
      },
      {
        "MarkEventDate": "2007-08-21",
        "MarkEventDateTruncated": "2007-08-21",
        "MarkEventDescription": "CASE FILE IN TICRS",
        "MarkEventEntryNumber": "21"
      },
      {
        "MarkEventDate": "2002-04-30",
        "MarkEventDateTruncated": "2002-04-30",
        "MarkEventDescription": "REGISTERED-PRINCIPAL REGISTER",
        "MarkEventEntryNumber": "20"
      },
      {
        "MarkEventDate": "2002-02-05",
        "MarkEventDateTruncated": "2002-02-05",
        "MarkEventDescription": "PUBLISHED FOR OPPOSITION",
        "MarkEventEntryNumber": "19"
      },
      {
        "MarkEventDate": "2002-01-16",
        "MarkEventDateTruncated": "2002-01-16",
        "MarkEventDescription": "NOTICE OF PUBLICATION",
        "MarkEventEntryNumber": "18"
      },
      {
        "MarkEventDate": "2001-10-03",
        "MarkEventDateTruncated": "2001-10-03",
        "MarkEventDescription": "APPROVED FOR PUB - PRINCIPAL REGISTER",
        "MarkEventEntryNumber": "17"
      },
      {
        "MarkEventDate": "2001-07-26",
        "MarkEventDateTruncated": "2001-07-26",
        "MarkEventDescription": "JURISDICTION RESTORED TO EXAMINING ATTORNEY",
        "MarkEventEntryNumber": "16"
      },
      {
        "MarkEventDate": "2001-07-26",
        "MarkEventDateTruncated": "2001-07-26",
        "MarkEventDescription": "EXPARTE APPEAL TERMINATED",
        "MarkEventEntryNumber": "15"
      },
      {
        "MarkEventDate": "2001-07-26",
        "MarkEventDateTruncated": "2001-07-26",
        "MarkEventDescription": "EXPARTE APPEAL DISMISSED AS MOOT",
        "MarkEventEntryNumber": "14"
      },
      {
        "MarkEventDate": "2000-04-28",
        "MarkEventDateTruncated": "2000-04-28",
        "MarkEventDescription": "CONTINUATION OF FINAL REFUSAL MAILED",
        "MarkEventEntryNumber": "13"
      },
      {
        "MarkEventDate": "2000-04-25",
        "MarkEventDateTruncated": "2000-04-25",
        "MarkEventDescription": "ASSIGNED TO EXAMINER",
        "MarkEventEntryNumber": "12"
      },
      {
        "MarkEventDate": "1999-01-12",
        "MarkEventDateTruncated": "1999-01-12",
        "MarkEventDescription": "CORRESPONDENCE RECEIVED IN LAW OFFICE",
        "MarkEventEntryNumber": "11"
      },
      {
        "MarkEventDate": "1998-07-08",
        "MarkEventDateTruncated": "1998-07-08",
        "MarkEventDescription": "NON-FINAL ACTION MAILED",
        "MarkEventEntryNumber": "10"
      },
      {
        "MarkEventDate": "1998-06-04",
        "MarkEventDateTruncated": "1998-06-04",
        "MarkEventDescription": "JURISDICTION RESTORED TO EXAMINING ATTORNEY",
        "MarkEventEntryNumber": "9"
      },
      {
        "MarkEventDate": "1998-06-03",
        "MarkEventDateTruncated": "1998-06-03",
        "MarkEventDescription": "EX PARTE APPEAL-INSTITUTED",
        "MarkEventEntryNumber": "8"
      },
      {
        "MarkEventDate": "1998-02-26",
        "MarkEventDateTruncated": "1998-02-26",
        "MarkEventDescription": "UNRESPONSIVE/DUPLICATE PAPER RECEIVED",
        "MarkEventEntryNumber": "7"
      },
      {
        "MarkEventDate": "1997-11-04",
        "MarkEventDateTruncated": "1997-11-04",
        "MarkEventDescription": "FINAL REFUSAL MAILED",
        "MarkEventEntryNumber": "6"
      },
      {
        "MarkEventDate": "1997-08-21",
        "MarkEventDateTruncated": "1997-08-21",
        "MarkEventDescription": "CORRESPONDENCE RECEIVED IN LAW OFFICE",
        "MarkEventEntryNumber": "5"
      },
      {
        "MarkEventDate": "1997-02-10",
        "MarkEventDateTruncated": "1997-02-10",
        "MarkEventDescription": "NON-FINAL ACTION MAILED",
        "MarkEventEntryNumber": "4"
      },
      {
        "MarkEventDate": "1997-02-07",
        "MarkEventDateTruncated": "1997-02-07",
        "MarkEventDescription": "ASSIGNED TO EXAMINER",
        "MarkEventEntryNumber": "3"
      },
      {
        "MarkEventDate": "1997-01-16",
        "MarkEventDateTruncated": "1997-01-16",
        "MarkEventDescription": "ASSIGNED TO EXAMINER",
        "MarkEventEntryNumber": "2"
      },
      {
        "MarkEventDate": "1997-01-14",
        "MarkEventDateTruncated": "1997-01-14",
        "MarkEventDescription": "ASSIGNED TO EXAMINER",
        "MarkEventEntryNumber": "1"
      }
    ]
  }
}
```
## Option variations
This program uses [Microsoft Mono.Options](https://github.com/xamarin/XamarinComponents/tree/master/XPlat/Mono.Options) to parse Posix-compliant otions. The `Mono.Options` package allows for a number of variations in specifying options. For example, a colon (`:`) may be used instead of an equal-sign (`=`); and if the operand is not optional, you may use a space (` `) or even omit any separation character entirely. In addition, for those accustomed to Windows options (which use a slash (`/`) rather than a dash (`-`)), that syntax is permitted as well. For example, all of the following (and other permutations) can be used to specify registration number 2,564,831.

- `-r=78123456`
- `-r:78123456`
- `-r 78123456`
- `-r78123456`
- `/r=78123456`
- `--registration=78123456`

However, for options where an operand is *optional*, the space cannot be used as a separator. In addition, omitting a separator when using the slash (`/`) character is not permitted. For example, the following are valid ways of an alternative configuration filename of `myfile.config`:

- `-c=myfile.config`
- `/c:myfile.config`
- `--configmyfile.config`

But the following will fail:

- `/cmyfile.config`
- `--config myfile.config`

For consistency, it is recommmended to simply always use the equal-sign (`=`).
## Support
For questions and suggestions on TSDR2JSON, please open an [issue](https://github.com/codingatty/TSDR2JSON/issues) at the TSDR2JSON repository on Github.


