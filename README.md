# TSDR2JSON

TSDR2JSON is a Windows command-line program to obtain trademark information from the USPTO's TSDR system and present it in an easy-to-parse JSON format

Command is used as follows:

`TSDRJSON s nnnnnnnn` : provide information on trademark application serial no. nnnnnnnn (8-digit number) in JSON format

or

`TSDRJSON r nnnnnnn` : provide information on trademark registration no. nnnnnnn (7-digit number) in JSON format

For example, `TSDRJSON r 2564831` produces something like the following:

```
{
  "SuccessInfo": {
    "Success": "True",
    "ErrorCode": "",
    "ErrorMessage": ""
  },
  "TSDRSingle": {
    "MarkCurrentStatusDate": "2009-02-07-05:00",
    "MarkCurrentStatusDateTruncated": "2009-02-07",
    "ApplicationNumber": "75181334",
    "ApplicationDate": "1996-10-15-04:00",
    "ApplicationDateTruncated": "1996-10-15",
    "RegistrationNumber": "2564831",
    "RegistrationDate": "2002-04-30-04:00",
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
    "PublicationDate": "2002-02-05-05:00",
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
    "CorrespondentEmailAddress": "",
    "StaffName": "",
    "StaffOfficialTitle": ""
  },
  "TSDRMulti": {
    "ApplicantList": [
      {
        "ApplicantName": "Python (Monty) Pictures Ltd.",
        "ApplicantDescription": "ORIGINAL REGISTRANT",
        "ApplicantAddressLine01": "Room 537/538, The Linen Hall",
        "ApplicantAddressLine02": "",
        "ApplicantAddressCity": "London",
        "ApplicantAddressGeoRegion": "",
        "ApplicantPostalCode": "W1R 5TB",
        "ApplicantCountryCode": "GB",
        "ApplicantCombinedAddress": "Room 537/538, The Linen Hall//London//W1R 5TB/GB"
      },
      {
        "ApplicantName": "Python (Monty) Pictures Ltd.",
        "ApplicantDescription": "OWNER AT PUBLICATION",
        "ApplicantAddressLine01": "Room 537/538, The Linen Hall",
        "ApplicantAddressLine02": "",
        "ApplicantAddressCity": "London",
        "ApplicantAddressGeoRegion": "",
        "ApplicantPostalCode": "W1R 5TB",
        "ApplicantCountryCode": "GB",
        "ApplicantCombinedAddress": "Room 537/538, The Linen Hall//London//W1R 5TB/GB"
      },
      {
        "ApplicantName": "Python (Monty) Pictures Ltd.",
        "ApplicantDescription": "ORIGINAL APPLICANT",
        "ApplicantAddressLine01": "Room 537/538, The Linen Hall",
        "ApplicantAddressLine02": "",
        "ApplicantAddressCity": "London",
        "ApplicantAddressGeoRegion": "",
        "ApplicantPostalCode": "W1R 5TB",
        "ApplicantCountryCode": "GB",
        "ApplicantCombinedAddress": "Room 537/538, The Linen Hall//London//W1R 5TB/GB"
      }
    ],
    "MarkEventList": [
      {
        "MarkEventDate": "2009-02-07-05:00",
        "MarkEventDateTruncated": "2009-02-07",
        "MarkEventDescription": "CANCELLED SEC. 8 (6-YR)",
        "MarkEventEntryNumber": "22"
      },
      {
        "MarkEventDate": "2007-08-21-04:00",
        "MarkEventDateTruncated": "2007-08-21",
        "MarkEventDescription": "CASE FILE IN TICRS",
        "MarkEventEntryNumber": "21"
      },
      {
        "MarkEventDate": "2002-04-30-04:00",
        "MarkEventDateTruncated": "2002-04-30",
        "MarkEventDescription": "REGISTERED-PRINCIPAL REGISTER",
        "MarkEventEntryNumber": "20"
      },
      {
        "MarkEventDate": "2002-02-05-05:00",
        "MarkEventDateTruncated": "2002-02-05",
        "MarkEventDescription": "PUBLISHED FOR OPPOSITION",
        "MarkEventEntryNumber": "19"
      },
      {
        "MarkEventDate": "2002-01-16-05:00",
        "MarkEventDateTruncated": "2002-01-16",
        "MarkEventDescription": "NOTICE OF PUBLICATION",
        "MarkEventEntryNumber": "18"
      },
      {
        "MarkEventDate": "2001-10-03-04:00",
        "MarkEventDateTruncated": "2001-10-03",
        "MarkEventDescription": "APPROVED FOR PUB - PRINCIPAL REGISTER",
        "MarkEventEntryNumber": "17"
      },
      {
        "MarkEventDate": "2001-07-26-04:00",
        "MarkEventDateTruncated": "2001-07-26",
        "MarkEventDescription": "JURISDICTION RESTORED TO EXAMINING ATTORNEY",
        "MarkEventEntryNumber": "16"
      },
      {
        "MarkEventDate": "2001-07-26-04:00",
        "MarkEventDateTruncated": "2001-07-26",
        "MarkEventDescription": "EXPARTE APPEAL TERMINATED",
        "MarkEventEntryNumber": "15"
      },
      {
        "MarkEventDate": "2001-07-26-04:00",
        "MarkEventDateTruncated": "2001-07-26",
        "MarkEventDescription": "EXPARTE APPEAL DISMISSED AS MOOT",
        "MarkEventEntryNumber": "14"
      },
      {
        "MarkEventDate": "2000-04-28-04:00",
        "MarkEventDateTruncated": "2000-04-28",
        "MarkEventDescription": "CONTINUATION OF FINAL REFUSAL MAILED",
        "MarkEventEntryNumber": "13"
      },
      {
        "MarkEventDate": "2000-04-25-04:00",
        "MarkEventDateTruncated": "2000-04-25",
        "MarkEventDescription": "ASSIGNED TO EXAMINER",
        "MarkEventEntryNumber": "12"
      },
      {
        "MarkEventDate": "1999-01-12-05:00",
        "MarkEventDateTruncated": "1999-01-12",
        "MarkEventDescription": "CORRESPONDENCE RECEIVED IN LAW OFFICE",
        "MarkEventEntryNumber": "11"
      },
      {
        "MarkEventDate": "1998-07-08-04:00",
        "MarkEventDateTruncated": "1998-07-08",
        "MarkEventDescription": "NON-FINAL ACTION MAILED",
        "MarkEventEntryNumber": "10"
      },
      {
        "MarkEventDate": "1998-06-04-04:00",
        "MarkEventDateTruncated": "1998-06-04",
        "MarkEventDescription": "JURISDICTION RESTORED TO EXAMINING ATTORNEY",
        "MarkEventEntryNumber": "9"
      },
      {
        "MarkEventDate": "1998-06-03-04:00",
        "MarkEventDateTruncated": "1998-06-03",
        "MarkEventDescription": "EX PARTE APPEAL-INSTITUTED",
        "MarkEventEntryNumber": "8"
      },
      {
        "MarkEventDate": "1998-02-26-05:00",
        "MarkEventDateTruncated": "1998-02-26",
        "MarkEventDescription": "UNRESPONSIVE/DUPLICATE PAPER RECEIVED",
        "MarkEventEntryNumber": "7"
      },
      {
        "MarkEventDate": "1997-11-04-05:00",
        "MarkEventDateTruncated": "1997-11-04",
        "MarkEventDescription": "FINAL REFUSAL MAILED",
        "MarkEventEntryNumber": "6"
      },
      {
        "MarkEventDate": "1997-08-21-04:00",
        "MarkEventDateTruncated": "1997-08-21",
        "MarkEventDescription": "CORRESPONDENCE RECEIVED IN LAW OFFICE",
        "MarkEventEntryNumber": "5"
      },
      {
        "MarkEventDate": "1997-02-10-05:00",
        "MarkEventDateTruncated": "1997-02-10",
        "MarkEventDescription": "NON-FINAL ACTION MAILED",
        "MarkEventEntryNumber": "4"
      },
      {
        "MarkEventDate": "1997-02-07-05:00",
        "MarkEventDateTruncated": "1997-02-07",
        "MarkEventDescription": "ASSIGNED TO EXAMINER",
        "MarkEventEntryNumber": "3"
      },
      {
        "MarkEventDate": "1997-01-16-05:00",
        "MarkEventDateTruncated": "1997-01-16",
        "MarkEventDescription": "ASSIGNED TO EXAMINER",
        "MarkEventEntryNumber": "2"
      },
      {
        "MarkEventDate": "1997-01-14-05:00",
        "MarkEventDateTruncated": "1997-01-14",
        "MarkEventDescription": "ASSIGNED TO EXAMINER",
        "MarkEventEntryNumber": "1"
      }
    ]
  }
}

```
