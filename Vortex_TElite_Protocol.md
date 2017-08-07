Vortex TElite Protocol

Content
1 INTRODUCTION
2 OVERVIEW
3 INITIAL CONNECTION
4 COMMAND LINE
5 PAGES
5.1 Row 0 Format And Processing
5.2 Row 24 And Above
6 FILE TRANSFER
7 SUMMARY OF MESSAGE TYPES
7.1 To the Client From VORTEX
7.2 To VORTEX From the Client
8 MESSAGE DETAILS
8.1 COMMAND LINE
8.1.1 COMMAND LINE Message when logging onto VORTEX
8.2 COMMAND RESPONSE
8.3 PAGE REQUEST
8.4 PAGE RESPONSE
8.5 PAGE WITH COMMAND ROW
8.6 ERROR RESPONSE
8.7 TIMEDATE REQUEST
8.8 TIMEDATE RESPONSE
8.9 FILE BLOCK
8.10 FILE BROWSE
8.11 FILE END
8.12 HOST NAME REQUEST
8.13 HOST NAME RESPONSE
8.14 DISCONNECT LINK
8.15 FIELD DEFINITIONS
8.16 FIELD RESTART
8.17 ONLINE
8.18 EXECUTE DOS COMMAND
8.19 USER ATTRIBUTES REQUEST
8.20 USER ATTRIBUTES RESPONSE
8.21 USER ACCESS REQUEST
8.22 USER ACCESS RESPONSE

1 INTRODUCTION
This document describes the protocol used between a TCP/IP Client and a Vortex host Edit System using the TCP/IP protocol.
All numbers in this document are decimal unless indicated otherwise.
2 OVERVIEW
This protocol is a half duplex protcol with the Client as the master, i.e. message exchanges are initiated by the Client.
Messages are exchanged using the TCP/IP sockets. All messages start with a one byte message code and are variable length. All messages ends with a sequence of F8h 01h. The F8h is called trip character. If F8h appears in the original message, it should be replaced with F8h F8h.
The Client may disconnect the TCP/IP connection at the end of any message exchange with VORTEX.
VORTEX will disconnect the TCP/IP connection on detection of a communications error, and after responding to a connection request with a ERROR RESPONSE message.
This protocol does not use parity, and relies on TCP/IP
for error free transport.
3 INITIAL CONNECTION
The protocol is started by the Client requesting connection to port 1025 on the computer running VORTEX. The port may vary as it is possible to redefine within Vortex, for more information regarding changes of port number, please contact Softel.
VORTEX accepts the connection request and then checks that the connection request was issued from a known TCP/IP address and that communication with that node is enabled.
If either of the above checks fail then VORTEX replies with an ERROR RESPONSE message indicating that the connection was rejected (unknown node) or the terminal is disabled. VORTEX then disconnects the TCP/IP connection.
If both of the above checks succeed then VORTEX replies with a COMMAND RESPONSE message containing a welcome message.
4 COMMAND LINE
The Client sends a command line to VORTEX using a COMMAND LINE message. VORTEX will reply with one of
a) COMMAND RESPONSE
b) PAGE WITH COMMAND LINE
c) PAGE REQUEST
d) FILE BLOCK (one or more), then FILE BROWSE
e) USER ACCESS RESPONSE followed by COMMAND RESPONSE
 (after successuful LOGIN)
5 PAGES
Pages are sent to and from the Client as a set of rows in ascending numberical order. Pages rows are formatted as follows - one byte row number followed by 40 bytes of row data. Only non-blank rows are sent. Row 0 is always sent.
The row data for rows 1-24 consists of characters from the teletext character set without parity (odd parity is applied to the page within VORTEX).
Rows above 24 should be sent as specified in the WST but without hamming.
5.1 Row 0 Format And Processing
Row 0 contains the page title and some of the page attributes. Corrrectly formatted row 0 of pages sent from Client to VORTEX are processed to set these attributes. The row 0 format is
 Column 12 - If this contains a '*' then the page is to be time stamped with the current time and
 date.
 Column 14 to 30 - page title. Must be a valid VORTEX page title, i.e. start with a letter and
 contain allowed characters only. Column 35 - If this contains a teletext level 1 colour code then columns 36 to 39 are processed.

 Column 36-38 - header bits. If column 35 contains a '*' then these colums should contain
 either a space or one of the following characters :-
 
 C - no clear bit for page
 U - no update bit for page
 N - page is newsflash
 H - suppress page header on transmission
 A - page is add-on - same as C and U together
 Column 39 - one of the following characters specifing the page language :-
 space,Q,q - default language (English)
 F,f - French
 D,d - German
 S,s - Swedish/Finish
 I,i - italian
 E,e - Spanish/Portugese
 A,a - Turkish/Arabic
 R,r - reserved
5.2 Row 24 And Above
Rows above 23 are discarded if the terminal is defined as a level 1 terminal (i.e. the default). If the terminal is defined as level 2 or 3 then rows above 23 are processed as follows
1. Row 24 - treated as rows 1-23 i.e. parity added and row stored. If the system supports Fastext
 then any row 24 in the page is ignored.
2. Rows > 24 - stored as sent i.e. no parity added. When the page is transmitted then the hamming is
 added and the order of the rows altered to be the order specified by the WST.

 Transmission CRCs are added as required when either
 a) the row which carries them (row 27 designation code 0 or row 26 designation code F) is present
 in the page
 or
 b) FASTEXT or similar post processing is required
6 FILE TRANSFER
Files may be sent to the Client in response to a COMMAND LINE. The file is sent as one or more FILE BLOCK messages, followed by a FILE BROWSE message which indicates that all the file has been sent.
7 SUMMARY OF MESSAGE TYPES
The message types currently in use are as follows. All values between 0 and 255 inclusive not used below are reserved for future expansion.
7.1 To the Client From VORTEX
 1 - COMMAND RESPONSE
 2 - PAGE REQUEST
 3 - PAGE WITH COMMAND ROW
 6 - TIMEDATE RESPONSE
 8 - HOST NAME RESPONSE
 11 - ASCII PAGE (no longer used)
 29 - FILE BLOCK
 31 - EXECUTE DOS COMMAND
 32 - FILE BROWSE
 33 - END OF FILE (not yet used)
 34 - USER ACCESS RESPONSE
 48 - FIELD DEFINITIONS
 49 - FIELD RESTART
 50 - USER ATTRIBUTES RESPONSE
 255 - ERROR RESPONSE
7.2 To VORTEX From the Client
 10 - COMMAND LINE
 2 - PAGE RESPONSE
 3 - DISCONNECT LINK
 6 - TIMEDATE REQUEST
 8 - HOST NAME REQUEST
 11 - ASCII PAGE (no longer used)
 34 - USER ACCESS REQUEST
 40 - ONLINE
 50 - USER ATTRIBUTES REQUEST
8 MESSAGE DETAILS
8.1 COMMAND LINE
This message is sent from the Client to the Alpha. This message is 81 bytes, starting with a message type byte containing 10 followed by 80 bytes of command line. Thecommand may be terminated by any control character (byte containing value < 32 decimal). Leading spaces and control characters are ignored.
8.1.1 COMMAND LINE Message when logging onto VORTEX
A special case of this is when logging onto VORTEX. The terminal sends a COMMAND LINE containing the LOGON command and the username, and VORTEX responds with a prompt ending in a CONCEAL DISPLAY code (18 hex). (The idea being that the terminal will conceal anything typed on a command line after a CONCEAL DISPLAY character hence hiding the password). The terminal must respond with a COMMAND LINE message containg the password preceeded by a CONCEAL DISPLAY character i.e. the text in the COMMAND LINE message should be formatted
1. optional characters which are ignored
2. CONCEAL DISPLAY character
3. optional space characters
4. password (up to 20 alphanumberic characters)
5. space or colour code to terminate password.
Adding the password to the prompt sent by VORTEX by overwriting the characters after the CONCEAL DISPLAY will result in correctly formed text.
8.2 COMMAND RESPONSE
This message is sent from VORTEX to the Client. This message is variable length, starting with a message type byte containing 1 followed by 80 bytes of text padded tothe right with spaces, followed by the softkey definitions.
The text starts with one of the following colour codes
 3 (ALPHA YELLOW) - Command success mesage
 6 (ALPHA CYAN) - Command error message
 7 (ALPHA WHITE) - Command succeeded and text is a command.
The softkey definitions consist of several variable length entries, one per softkey. Each entry consists of the following
1. key label length (one byte, maximum value is 10)
 
A key label length of 255 is an instruction to not define the corresponding key, and in this case the other fields in the function key entry are not present. A key label length of zero terminates the key definitions.
 
2. key label characters
3. key definition length (one byte, maximum value is 80)
4. key definition characters
The first key definition corresponds to PF1, the second to
PF2, etc.
8.3 PAGE REQUEST
This message is sent from VORTEX to the Client to request the page currently displayed on the screen of the Client. This message is an 81 byte message, starting with a message type byte containing the value 2 followed by 80 bytes which are ignored by the Client.
8.4 PAGE RESPONSE
This message is sent from the Client to VORTEX in response to a PAGE REQUEST. This message is a variable length message, starting with a message type byte containing 2followed by the page rows as described above.
8.5 PAGE WITH COMMAND ROW
This message is sent from VORTEX to the Client. This message is variable length, starting with a message type byte containing 3 followed by the page rows as describedabove, then the 80 byte command row prefixed by a byte containing 255. The command row is in the same form as for a COMMAND RESPONSE.
8.6 ERROR RESPONSE
This is an 81 byte message, starting with a message type byte containing 255 followed by 80 bytes of text padded to the right with spaces.
8.7 TIMEDATE REQUEST
This message is sent from the Client to VORTEX, to request the time and date. This message is a 1 byte message consisting of a message type of 6.
8.8 TIMEDATE RESPONSE
This message is sent by VORTEX to the Client, in response to a TIMEDATE REQUEST. This message is fixed length, starting with a message type byte of containing 6,followed by (each occupying one byte) the hour, minute, second, year since 1900, month (1 is January), and day of month.
8.9 FILE BLOCK
This message is sent by VORTEX to the Client to transfer the contents of a file. This message is variable length, starting with a message type byte of 29, followed by two bytes containg the total length of the following file records (least significant byte first). This is followed by up to 1024 bytes of file records. Each record startswith a two byte record length, followed by the record contents, followed by a filler byte if required to make the record an even number of bytes. The filler byte is not included in the record length.
All FILE BLOCK messages except the last one contain 1024 bytes of file records. File records may span more than one FILE BLOCK message.
8.10 FILE BROWSE
This message is sent by VORTEX to the Client to indicate that all of a file has been sent. This is an 81 byte message, in the same format as a COMMAND RESPONSE, with a message type of 32.
8.11 FILE END
(THIS MESSAGE IS DEFINED FOR A FUTURE CHANGE TO THE FILE
TRANSFER PROTOCOL AND IS NOT USED AT PRESENT).
This message is sent by VORTEX to the Client to indicate that all of a file has been sent. This message is variable length and consists of a message type byte of 33,followed by a flag byte, optionally followed by a null terminated title. The flag byte is 0 if the top of the file is to be displayed initially, and 1 if the bottom of the file is to be displayed. The optional title is a
string of up to 26 characters to be displayed as the title of the window in which the file is displayed. If no title is used then "Host Info" is used. A null title may be sent in which case the file window has no title.
8.12 HOST NAME REQUEST
This message is sent from the Client to VORTEX, to request the name of the VORTEX host. This message is a 1 byte message containing a message type of 8.
8.13 HOST NAME RESPONSE
This message is sent from VORTEX to the Client in response to a HOST NAME REQUEST. This message is variable length, starting with a message type byte of 8, followed by the host name terminated with a null byte. The maximum length of the host name is 8 characters.
8.14 DISCONNECT LINK
This message is sent from Client to VORTEX and is a one byte message consisting of a message type of 3. This message requests VORTEX to disconnect the TCP/IP connection to the Client .
8.15 FIELD DEFINITIONS
This message is sent from VORTEX to the Client to define a set of fields which overlay a page. This message is sent before the PAGE RESPONSE or PAGE WITH COMMAND ROW message which defines the page. When the PAGE RESPONSE or PAGE WITH COMMAND ROW message is received by the Client then the Client enters field entry mode where the user can only enter data on the page in the defined fields, and the type of the data must conform to the field description.
This message has the following structure.
 
 unsigned char - message type = 48, i.e. a FIELD DEFINITIONS message
 unsigned char - number of field definitions contain in message (max 60)
 char*40 - completion message - null terminated.
 up to 60 field descriptions, where each description consists of unsigned char - start row (1 to 23)
 unsigned char - start column (0 to 39)
 unsigned char - end row (1 to 23)
 unsigned char - end column (0 to 39)
 unsigned char - field type
 1 - numeric
 2 - alpha numberic
 3 - alpha ext.
 4 - binary
 5 - octal
 6 - hex
 7 - date
 8 - time
 9 - float
 unsigned char - field attributes
 bit 0 - set if field is mandatory
 bit 1 - set if field fill enabled
 bit 2 - set if field contents are not to be echoed
 bit 3 - set if field is protected i.e. can not be changed
 char*40 - field prompt - null terminated
8.16 FIELD RESTART
This message is sent from VORTEX to the Client to indicate an error in a field value. The Client restarts field entry mode at the field indicated in the message. The message is sent before a COMMAND RESPONSE message which contains an error message. The format of the message is
 unsigned char - message type = 49, i.e. a FIELD
 RESTART message
 unsigned char - number of field containing error
 char*40 - restart message - null terminated
8.17 ONLINE
This message is sent from the Client to VORTEX when a Remote Client goes online. The message consists of the message type code (value 40) only. VORTEX responds with a COMMAND RESPONSE containing a welcome message and the sofkeys.
8.18 EXECUTE DOS COMMAND
This message is sent by VORTEX to the Client , and is a variable length message. The message consists of a message type code of 31, followed by the DOS command to be executed. The DOS command is null terminated and has a maximum length of 50 characters. The output from the DOS command is writtern to a file called dosshell.log in the RWPATH directory for the Elite.
8.19 USER ATTRIBUTES REQUEST
This message is sent by the Client to VORTEX, and is a single byte request consisting of a message type byte containing 50.
8.20 USER ATTRIBUTES RESPONSE
This message is sent by VORTEX to the Client in response to a USER ATTRIBUTES REQUEST. This message consists of the following structure.
 unsigned char - Message Type = 50, i.e. USER
 ATTRIBUTES RESPONSE.
 unsigned char - flags
 BIT 0 - If set user is logged on and rest of
 message contains valid data.
 BIT 1 - If set user is allowed to view
 batch files
 BIT 2-7 - reserved
 char * 20 - username right justified and padded
 out with spaces.
 unsigned char - user level
 0 - undefined
 1 - remote
 2 - regular
 3 - supervisor
 4 - manager
 unsigned char - user language
 0 - English
 1 - French
 2 - Reserved
 3 - Reserved
 4 - German
 5 - Reserved
 6 - Italian
 7 - Reserved
Further data may be added to the end of this message in
later versions.
8.21 USER ACCESS REQUEST
This message is sent by the Client to VORTEX, and is a single byte request consisting of a message type byte containing 34.
8.22 USER ACCESS RESPONSE
This message is sent by VORTEX to the Client in response to a USER ACCESS REQUEST, and also when a LOGIN command is successfully processed.
This message consists of the following structure.
 unsigned char - message type = 34, i.e. USER
 ACCESS RESPONSE
 char - batch allowed - set to ascii '1' if user
 can use batch menu option. Set to ascii
 '0' otherwise.
 char - spell check dictionary update - set to
 ascii '1' if user can update the spell check
 dictionary, and ascii '0' otherwise.
 char - terminator - always contains null
 characters.
At present SUPERVISOR and MANAGER Vortex users are allowed
to use the Batch menu option. Only MANAGER Vortex
users are allowed to update the spell check dictionary.
More characters may be added to this message in later
versions. The characters will always be added before the
terminating null character, and after any existing
characters.

