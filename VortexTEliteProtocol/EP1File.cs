// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/EP1File.cs $
// $Author: nothma $
// $Date: 2012-03-30 15:47:57 +0200 (Fr, 30 Mrz 2012) $
// $Revision: 1365 $
//
// Description:
// EP1File class copied from CASPer util package
// from rev: 5 / date: 24.02.09 13:54 
// -------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;

namespace VortexTEliteProtocol
{
    /// <summary>
    /// Structure for Teletext page number
    /// </summary>
    public struct PageNumber
    {
        /// <summary>
        /// Magazin
        /// </summary>
        public int Magazin;
        /// <summary>
        /// Set
        /// </summary>
        public int Set;
        /// <summary>
        /// Page
        /// </summary>
        public int Page;

    }

    /// <summary>
    /// Structure which represents an ep1-file
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto, Size = 1008)]
    public struct EP1Format
    {
        /// <summary>
        /// A COMMENT MUST BE DEFINED
        /// </summary>
        public byte FileType;
        /// <summary>
        /// A COMMENT MUST BE DEFINED
        /// </summary>
        public byte TXTLevel;
        /// <summary>
        /// A COMMENT MUST BE DEFINED
        /// </summary>
        public byte LanguageCode;
        /// <summary>
        /// A COMMENT MUST BE DEFINED
        /// </summary>
        public byte AttributeFlag;
        /// <summary>
        /// A COMMENT MUST BE DEFINED
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] AttributeLenght;
        /// <summary>
        /// A COMMENT MUST BE DEFINED
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] Header;
        /// <summary>
        /// A COMMENT MUST BE DEFINED
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 920)]
        public byte[] Content;
        /// <summary>
        /// A COMMENT MUST BE DEFINED
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] CommandRow;
        /// <summary>
        /// A COMMENT MUST BE DEFINED
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] ControlCode;
    }

    /// <summary>
    /// Implements the functionality to handle EP1-Files
    /// </summary>
    public class EP1File
    {

        #region Enumerations
        //**************************************************
        // Enumerations
        //**************************************************

        #endregion


        #region Constants
        //**************************************************
        // Constants
        //**************************************************

        #endregion


        #region Fields
        //**************************************************
        // Fields
        //**************************************************

        #region Public fields
        //**************************************************
        // Public fields
        //**************************************************

        #endregion

        #region Protected fields
        //**************************************************
        // Protected fields
        //**************************************************

        #endregion

        #region Private fields
        //**************************************************
        // Private fields
        //**************************************************

        /// <summary>
        /// Member variable of filename
        /// </summary>
        private string m_FileName = string.Empty;

        /// <summary>
        /// Member variable of EP1 Format buffer
        /// </summary>
        private EP1Format m_Ep1Format;

        #endregion

        #endregion


        #region Properties
        //**************************************************
        // Properties
        //**************************************************

        #region Public properties
        //**************************************************
        // Public properties
        //**************************************************
        /// <summary>
        /// EP1 Format buffer
        /// </summary>
        public EP1Format Ep1Format
        {
            get { return m_Ep1Format; }
            set { m_Ep1Format = value; }
        }

        /// <summary>
        /// EP1 Filename
        /// </summary>
        public string FileName
        {
            get { return this.GetFileName(); }
        }

        #endregion

        #region Protected properties
        //**************************************************
        // Protected properties
        //**************************************************

        #endregion

        #region Private properties
        //**************************************************
        // Private properties
        //**************************************************

        #endregion

        #endregion


        #region Methods
        //**************************************************
        // Methods
        //**************************************************

        #region Constructor
        //**************************************************
        // Constructor
        //**************************************************

        /// <summary>
        /// Initializes a new instance of the EP1File class.
        /// </summary>
        public EP1File()
        {
            m_Ep1Format = new EP1Format();
        }

        /// <summary>
        /// Initializes a new instance of the EP1File class with the given content
        /// </summary>
        public EP1File(byte[] content) : this()
        {
            int size = Marshal.SizeOf(typeof(EP1Format));

            IntPtr destBuffer = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.Copy(content, 0, destBuffer, size);
                m_Ep1Format = (EP1Format)Marshal.PtrToStructure(destBuffer, typeof(EP1Format));
            }
            finally
            {
                Marshal.FreeHGlobal(destBuffer);
            }
        }
        #endregion

        #region Destructor
        //**************************************************
        // Destructor
        //**************************************************

        /// <summary>
        /// default destructor of class
        /// </summary>
        ~EP1File()
        {
        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Initialize EP1 file with empty data
        /// </summary>
        public void Initialize()
        {
            int size = Marshal.SizeOf(typeof(EP1Format));

            byte[] sourceBuffer = Enumerable.Repeat((byte)0x20, size).ToArray();

            IntPtr destBuffer = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.Copy(sourceBuffer, 0, destBuffer, size);
                m_Ep1Format = (EP1Format)Marshal.PtrToStructure(destBuffer, typeof(EP1Format));
            }
            finally
            {
                Marshal.FreeHGlobal(destBuffer);
            }
        }

        /// <summary>
        /// this method reads an ep1-file and writes the content to
        /// the member m_Ep1Format
        /// </summary>
        /// <param name="reader">StreamReader of the file</param>
        public void ReadFile(StreamReader reader)
        {
            int size = Marshal.SizeOf(typeof(EP1Format));

            byte[] sourceBuffer = new byte[size];
            reader.BaseStream.Read(sourceBuffer, 0, size);

            IntPtr destBuffer = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.Copy(sourceBuffer, 0, destBuffer, size);
                m_Ep1Format = (EP1Format)Marshal.PtrToStructure(destBuffer, typeof(EP1Format));
            }
            finally
            {
                Marshal.FreeHGlobal(destBuffer);
            }
        }

        /// <summary>
        /// this method reads an ep1-file and writes the content to
        /// the member m_Ep1Format
        /// </summary>
        /// <param name="filename">filename</param>
        public void ReadFile(string filename)
        {
            m_FileName = filename;

            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            try
            {
                StreamReader streamReader = new StreamReader(fileStream);
                try
                {
                    int size = Marshal.SizeOf(typeof(EP1Format));

                    byte[] sourceBuffer = new byte[size];
                    streamReader.BaseStream.Read(sourceBuffer, 0, size);

                    IntPtr destBuffer = Marshal.AllocHGlobal(size);

                    try
                    {
                        Marshal.Copy(sourceBuffer, 0, destBuffer, size);
                        m_Ep1Format = (EP1Format)Marshal.PtrToStructure(destBuffer, typeof(EP1Format));
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(destBuffer);
                    }
                }
                finally
                {
                    streamReader.Close();
                }
            }
            finally
            {
                fileStream.Close();
            }
        }

        /// <summary>
        /// writes an ep1-file with the content of the member m_Ep1Format.
        /// </summary>
        /// <param name="filename">Filename of the new file</param>
        public void WriteFile(string filename)
        {
            FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);

            try
            {
                StreamWriter file = new StreamWriter(fileStream);
                try
                {
                    var bytes = this.ToArray();

                    file.BaseStream.Write(bytes, 0, bytes.Length);
                    file.Flush();
                }
                finally
                {
                    file.Close();
                }
            }
            finally
            {
                fileStream.Close();
            }
        }

        /// <summary>
        /// Extracts the EP1Format as Byte-Array
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            int size = Marshal.SizeOf(typeof (EP1Format));
            byte[] destBuffer = new byte[size];

            IntPtr sourceBuffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(m_Ep1Format, sourceBuffer, false);
                for (int i = 0; i < size; i++)
                {
                    destBuffer[i] = Marshal.ReadByte(sourceBuffer, i);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(sourceBuffer);
            }
            return destBuffer;
        }

        /// <summary>
        /// this method sets a new HeaderRow
        /// </summary>
        /// <param name="header">header as byte array</param>
        public void SetByteHeaderRow(byte[] header)
        {
            m_Ep1Format.Header = header;
        }

        /// <summary>
        /// this method sets a new HeaderRow
        /// </summary>
        /// <param name="Text">new text displayed in HeaderRow</param>
        public void SetHeaderRow(string Text)
        {
            byte[] line = new byte[40];

            ASCIIEncoding encoding = new ASCIIEncoding();
            line = encoding.GetBytes(Text);

            for (int i = 0; i < line.Length; i++)
            {
                this.m_Ep1Format.Header[i] = line[i];
            }
        }

        /// <summary>
        /// this method sets a new CommandRow
        /// </summary>
        /// <param name="command">command as byte array</param>
        public void SetByteCommandRow(byte[] command)
        {
            this.m_Ep1Format.CommandRow = command;
        }

        /// <summary>
        /// this method sets a new CommandRow
        /// </summary>
        /// <param name="Text">new text displayed in CommandRow</param>
        public void SetCommandRow(string Text)
        {
            byte[] line = new byte[40];

            ASCIIEncoding encoding = new ASCIIEncoding();
            line = encoding.GetBytes(Text);

            for (int i = 0; i < line.Length; i++)
            {
                this.m_Ep1Format.CommandRow[i] = line[i];
            }
        }

        /// <summary>
        /// returns the command row line as a byte array
        /// </summary>
        /// <returns>command row byte array</returns>
        public byte[] GetByteCommandRow()
        {
            return m_Ep1Format.CommandRow;
        }

        /// <summary>
        /// returns the command row line as a string
        /// </summary>
        /// <returns>command row line</returns>
        public string GetCommandRow()
        {
            StringBuilder retstring = new StringBuilder();

            for (int i = 0; i < 40; i++)
            {
                string byteValue = Int32.Parse(m_Ep1Format.CommandRow[i].ToString()).ToString("X");
                if (!((byteValue.Length < 2) || (Int32.Parse(byteValue.Substring(0, 1)) < 2)))
                {
                    retstring.Append(Convert.ToString(Convert.ToChar(Int32.Parse(m_Ep1Format.CommandRow[i].ToString()))));
                }
            }
            return retstring.ToString().Trim();
        }

        /// <summary>
        /// returns the command row line as a string
        /// </summary>
        /// <param name="doTrim">Define if the answer will be trimmed</param>
        /// <returns>command row line</returns>
        public string GetCommandRow(bool doTrim)
        {
            StringBuilder retstring = new StringBuilder();

            for (int i = 0; i < 40; i++)
            {
                string byteValue = Int32.Parse(m_Ep1Format.CommandRow[i].ToString()).ToString("X");
                if (!((byteValue.Length < 2) || (Int32.Parse(byteValue.Substring(0, 1)) < 2)))
                {
                    retstring.Append(Convert.ToString(Convert.ToChar(Int32.Parse(m_Ep1Format.CommandRow[i].ToString()))));
                }
            }
            if (doTrim)
            {
                return retstring.ToString().Trim();
            }
            else
            {
                return retstring.ToString();
            }
        }

        /// <summary>
        /// Gets the Page-Number of EP1 file from header row.
        /// </summary>
        /// <returns></returns>
        public PageNumber GetPageNumber()
        {
            PageNumber pageNumner = new PageNumber();

            if (m_Ep1Format.Header != null)
            {
                string headerStr = this.GetValueString(m_Ep1Format.Header, 0, m_Ep1Format.Header.Length);
                if (headerStr.Trim().Length > 0)
                {
                    // Teletext Page
                    pageNumner.Magazin = int.Parse(this.GetValueString(m_Ep1Format.Header, 0, 3));
                    pageNumner.Set = int.Parse(this.GetValueString(m_Ep1Format.Header, 4, 6));
                    pageNumner.Page = int.Parse(this.GetValueString(m_Ep1Format.Header, 7, 9));
                }
                else
                {
                    string pageStr = this.GetLine(1, false);
                    if (pageStr.Contains("Attributes for Page"))
                    {
                        // Attributes Page
                        pageStr = pageStr.Trim().Split(':')[1];
                        pageNumner.Magazin = int.Parse(pageStr.Trim().Split(' ')[0]);
                        pageStr = pageStr.Split(' ')[1];
                        pageNumner.Set = int.Parse(pageStr.Trim().Split('.')[0]);
                        pageNumner.Page = int.Parse(pageStr.Trim().Split('.')[1]);
                    }
                    if (pageStr.Contains("Attributes for Set"))
                    {
                        // Attributes Set
                        pageStr = pageStr.Trim().Split(':')[1];
                        pageNumner.Magazin = int.Parse(pageStr.Trim().Split(' ')[0]);
                        pageNumner.Set = int.Parse(pageStr.Trim().Split(' ')[1]);
                    }
                    if (pageStr.Contains("Attributes for Magazine"))
                    {
                        // Attributes Magazine
                        pageStr = pageStr.Trim().Split(':')[1];
                        pageNumner.Magazin = int.Parse(pageStr.Trim().Split(' ')[0]);
                    }

                }
            }

            return pageNumner;
        }

        /// <summary>
        /// returns the header row line as a byte array
        /// </summary>
        /// <returns>header row byte array</returns>
        public byte[] GetByteHeaderRow()
        {
            return m_Ep1Format.Header;
        }

        /// <summary>
        /// returns the header row line as a string
        /// </summary>
        /// <returns>header row line</returns>
        public string GetHeaderRow()
        {
            StringBuilder retstring = new StringBuilder();

            for (int i = 0; i < 40; i++)
            {
                string byteValue = Int32.Parse(m_Ep1Format.Header[i].ToString()).ToString("X");
                if (!((byteValue.Length < 2) || (Int32.Parse(byteValue.Substring(0, 1)) < 2)))
                {
                    retstring.Append(Convert.ToString(Convert.ToChar(Int32.Parse(m_Ep1Format.Header[i].ToString()))));
                }
            }
            return retstring.ToString().Trim();
        }

        /// <summary>
        /// returns the header row line as a string
        /// </summary>
        /// <param name="doTrim">Define if the answer will be trimmed</param>
        /// <returns>header row line</returns>
        public string GetHeaderRow(bool doTrim)
        {
            StringBuilder retstring = new StringBuilder();

            for (int i = 0; i < 40; i++)
            {
                string byteValue = Int32.Parse(m_Ep1Format.Header[i].ToString()).ToString("X");
                if (!((byteValue.Length < 2) || (Int32.Parse(byteValue.Substring(0, 1)) < 2)))
                {
                    retstring.Append(Convert.ToString(Convert.ToChar(Int32.Parse(m_Ep1Format.Header[i].ToString()))));
                }
            }
            if (doTrim)
            {
                return retstring.ToString().Trim();
            }
            else
            {
                return retstring.ToString();
            }
        }

        /// <summary>
        /// sets a new line of the content area
        /// </summary>
        /// <param name="Text">text to set</param>
        /// <param name="LineNumber">on this line number the text will be set</param>
        public void SetLine(string Text, int LineNumber)
        {
            if (LineNumber > 0 && LineNumber < 24)
            {
                byte[] line = new byte[40];

                ASCIIEncoding encoding = new ASCIIEncoding();
                line = encoding.GetBytes(Text);

                int startPos = (LineNumber - 1) * 40;

                int bytepointer = 0;
                for (int i = startPos; i < startPos + line.Length; i++)
                {
                    this.m_Ep1Format.Content[i] = line[bytepointer];
                    bytepointer++;
                }
            }
        }

        /// <summary>
        /// sets a new line of the content area
        /// </summary>
        /// <param name="Text">text to set</param>
        /// <param name="PageNumber">the pagenumber (displayed right-aligned)</param>
        /// <param name="LineNumber">on this line number the text will be set</param>
        public void SetLine(string Text, string PageNumber, int LineNumber)
        {
            if (LineNumber > 0 && LineNumber < 24)
            {
                byte[] line = new byte[40];
                string space = new string(' ', 40);
                int spacebetween = 39 - Text.Length - PageNumber.Length;


                ASCIIEncoding encoding = new ASCIIEncoding();
                line = encoding.GetBytes(" " + Text + space.Substring(0, spacebetween) + PageNumber);

                int startPos = (LineNumber - 1) * 40;

                int bytepointer = 0;
                for (int i = startPos; i < startPos + line.Length; i++)
                {
                    this.m_Ep1Format.Content[i] = line[bytepointer];
                    bytepointer++;
                }
            }
        }

        /// <summary>
        /// sets a new line of the content area
        /// </summary>
        /// <param name="Text">text to set</param>
        /// <param name="PageNumber">the pagenumber (displayed right-aligned)</param>
        /// <param name="FontColor">Color-code of the text</param>
        /// <param name="LineNumber">on this line number the text will be set</param>
        public void SetLine(string Text, string PageNumber, byte FontColor, int LineNumber)
        {
            if (LineNumber > 0 && LineNumber < 24)
            {
                byte[] line = new byte[40];
                string color;
                if (FontColor > 7 || FontColor < 1)
                {
                    byte newColor = 7;
                    color = Convert.ToString(Convert.ToChar(Int32.Parse(newColor.ToString())));
                }
                else
                {
                    color = Convert.ToString(Convert.ToChar(Int32.Parse(FontColor.ToString())));
                }

                string space = "                                       ";
                int spacebetween = 40 - color.Length - Text.Length - PageNumber.Length;


                ASCIIEncoding encoding = new ASCIIEncoding();
                line = encoding.GetBytes(color + Text + space.Substring(0, spacebetween) + PageNumber);

                int startPos = (LineNumber - 1) * 40;

                int bytepointer = 0;
                for (int i = startPos; i < startPos + line.Length; i++)
                {
                    this.m_Ep1Format.Content[i] = line[bytepointer];
                    bytepointer++;
                }
            }
        }

        /// <summary>
        /// This method inserts a byte array at a specific line
        /// </summary>
        /// <param name="line">byte array with the content to insert</param>
        /// <param name="lineNumber">number of the line where to insert</param>
        /// <param name="beginPosition">first char where to insert</param>
        public void SetByteLine(byte[] line, int lineNumber, int beginPosition)
        {
            int length = line.Length;

            if (lineNumber > 0 && lineNumber < 24 && (length + beginPosition) <= 41)
            {
                int startPos = ((lineNumber - 1) * 40) + beginPosition;

                int bytepointer = 0;
                for (int i = startPos; i < startPos + length; i++)
                {
                    this.m_Ep1Format.Content[i] = line[bytepointer];
                    bytepointer++;
                }
            }
        }

        /// <summary>
        /// This method inserts a byte array at a specific line
        /// </summary>
        /// <param name="line">byte array with the content to insert</param>
        /// <param name="lineNumber">number of the line where to insert</param>
        public void SetByteLine(byte[] line, int lineNumber)
        {
            this.SetByteLine(line, lineNumber, 1);
        }

        /// <summary>
        /// Returns a line of the content part as a string
        /// </summary>
        /// <param name="lineNumber">Line to return</param>
        /// <param name="controlChars">if true all bytes are return, if false the controlbytes are ignored</param>
        /// <returns>content of the line</returns>
        public string GetLine(int lineNumber, bool controlChars)
        {
            if (lineNumber > 0 && lineNumber < 24)
            {
                int startPos = (lineNumber - 1) * 40;

                StringBuilder retstring = new StringBuilder();

                for (int i = startPos; i < startPos + 40; i++)
                {
                    if (!controlChars)
                    {
                        string byteValue = Int32.Parse(m_Ep1Format.Content[i].ToString()).ToString("X");
                        if (!((byteValue.Length < 2) || (Int32.Parse(byteValue.Substring(0, 1)) < 2)))
                        {
                            retstring.Append(Convert.ToString(Convert.ToChar(Int32.Parse(m_Ep1Format.Content[i].ToString()))));
                        }
                    }
                    else
                    {
                        retstring.Append(Convert.ToString(Convert.ToChar(Int32.Parse(m_Ep1Format.Content[i].ToString()))));
                    }
                }

                string result = retstring.ToString();

                return result.Trim();
            }
            return string.Empty;
        }

        /// <summary>
        /// This method returns a byte-array with the content of a specific part of a line
        /// </summary>
        /// <param name="lineNumber">Number of the line to return</param>
        /// <param name="beginPosition">First char of the line to return</param>
        /// <param name="length">length of the part to return</param>
        /// <returns>byte array with the content</returns>
        public byte[] GetByteLine(int lineNumber, int beginPosition, int length)
        {
            byte[] byteLine = new byte[length];

            if (lineNumber > 0 && lineNumber < 24 && (beginPosition + length) <= 41)
            {
                int startPos = ((lineNumber - 1) * 40) + (beginPosition - 1);

                int byteIndex = 0;
                for (int i = startPos; i < startPos + length; i++)
                {
                    byteLine[byteIndex] = m_Ep1Format.Content[i];
                    byteIndex++;
                }
            }
            return byteLine;
        }

        /// <summary>
        /// This method returns a byte-array with the content of a specific line
        /// </summary>
        /// <param name="lineNumber">Number of the line to return</param>
        /// <param name="length">length of the part to return</param>
        /// <returns>byte array with the content</returns>
        public byte[] GetByteLine(int lineNumber, int length)
        {
            return this.GetByteLine(lineNumber, 1, length);
        }

        /// <summary>
        /// Copies a part of an ep1-File in an ArrayList
        /// </summary>
        /// <param name="topRow">First line number of the part (from the top)</param>
        /// <param name="leftCol">First column of the part</param>
        /// <param name="bottomRow">Last line number of the part (from the top)</param>
        /// <param name="rightCol">Last column of the part</param>
        /// <returns>An ArrayList which contains byte-Arrays of each line</returns>
        public ArrayList CopyPartOfFile(int topRow, int leftCol, int bottomRow, int rightCol)
        {
            ArrayList result = new ArrayList();

            if (topRow < 24 && bottomRow < 24 && leftCol < 41 && rightCol < 41 && (topRow * leftCol * bottomRow * rightCol) > 0)
            {
                int numberOfLines = bottomRow - topRow + 1;
                int numberOfBytes = rightCol - leftCol + 1;

                for (int i = 0; i < numberOfLines; i++)
                {
                    byte[] line = this.GetByteLine(topRow + i, leftCol, numberOfBytes);
                    result.Add(line);
                }
            }
            return result;
        }

        /// <summary>
        /// Copies a part of an ep1-File in an ArrayList
        /// </summary>
        /// <param name="config">Array which contains the top, left bottom and right regions</param>
        /// <returns>An ArrayList which contains byte-Arrays of each line</returns>
        public ArrayList CopyPartOfFile(int[] config)
        {
            return this.CopyPartOfFile(config[0], config[1], config[2], config[3]);
        }

        /// <summary>
        /// Inserts a content part, which is stored in an ArrayList, at a specific place in the ep1-File
        /// </summary>
        /// <param name="listToPaste">ArrayList with content to insert</param>
        /// <param name="topRow">First line</param>
        /// <param name="leftCol">First column</param>
        public void InsertPartOfFile(ArrayList listToPaste, int topRow, int leftCol)
        {
            if ((topRow + listToPaste.Count) < 24)
            {
                int i = 0;
                foreach (byte[] line in listToPaste)
                {
                    if ((leftCol + line.Length - 1) < 41)
                    {
                        this.SetByteLine(line, topRow + i, leftCol - 1);
                        i++;
                    }
                }
            }
        }

        #endregion

        #region Protected Methods
        //**************************************************
        // Protected Methods
        //**************************************************

        #endregion

        #region Private Methods
        //**************************************************
        // Private Methods
        //**************************************************

        /// <summary>
        /// Gets the filename of the EP1 file
        /// </summary>
        /// <returns></returns>
        private string GetFileName()
        {
            if ((m_FileName == string.Empty) && (m_Ep1Format.Header != null))
            {
                PageNumber pageNumber = this.GetPageNumber();
                m_FileName = String.Format("{0}.{1}.{2}.ep1", pageNumber.Magazin, pageNumber.Set, pageNumber.Page);
            }

            return m_FileName;
        }

        /// <summary>
        /// Gets a value string from array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <returns></returns>
        private string GetValueString(byte[] array, int startPos, int endPos)
        {
            string valueString = string.Empty;

            // read magazin from header row
            for (int i = startPos; i < endPos; i++)
            {
                valueString += ((char)array[i]).ToString();
            }

            return valueString.Trim();
        }

        #endregion

        #region Eventhandlers
        //**************************************************
        // Eventhandlers
        //**************************************************


        #endregion

        #endregion

    }

}
/* 
 * -------------------------------------------------------------------
 * History:
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/EP1File.cs $
 * 
 * 8     16.07.09 11:40 san
 * 
 * 7     15.07.09 14:15 san
 * Add new function: Get Page-Attribute EP1 from Vortex.
 * 
 * 6     2.07.09 9:27 san
 * 
 * 5     1.07.09 14:10 san
 * 
 * 4     1.07.09 14:03 san
 * 
 * 3     30.06.09 11:15 san
 * 
 * 2     23.06.09 14:45 san
 * 
 * 1     22.06.09 13:35 san
 * -------------------------------------------------------------------
 */