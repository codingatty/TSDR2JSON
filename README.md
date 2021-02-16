# TSDR2JSON

TSDR2JSON is a Windows command-line program to obtain trademark information from the USPTO's TSDR system and present it in an easy-to-parse JSON format

Command is used as follows:

`TSDRJSON  --key="kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk" -s=nnnnnnnn` : provide information on trademark application serial no. nnnnnnnn (8-digit number) in JSON format

or

`TSDRJSON  --key="kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk" -r=nnnnnnn` : provide information on trademark registration no. nnnnnnn (7-digit number) in JSON format

`"kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk"` is replaced by your 32-character API key, [provided by the USPTO](https://developer.uspto.gov/api-catalog/tsdr-data-api).


For detailed information and an example of sample output, please see [here](https://github.com/codingatty/TSDR2JSON/blob/master/DOC.md).
