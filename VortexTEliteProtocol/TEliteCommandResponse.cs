// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteCommandResponse.cs $
// $Author: har $
// $Date: 2011-06-14 11:17:47 +0200 (Di, 14 Jun 2011) $
// $Revision: 656 $
//
// Description:
//
// -------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace VortexTEliteProtocol
{
    /// <summary>
    /// TEliteSoftKey
    /// </summary>
    public class TEliteSoftKey
    {
        /// <summary>
        /// SoftKey label member variable
        /// </summary>
        private string m_KeyLabel = string.Empty;

        /// <summary>
        /// SoftKey definition member variable
        /// </summary>
        private string m_KeyDefinition = string.Empty;

        /// <summary>
        /// SoftKey label 
        /// </summary>
        public string KeyLabel
        {
            get { return m_KeyLabel; }
            set { m_KeyLabel = value; }
        }

        /// <summary>
        /// SoftKey definition
        /// </summary>
        public string KeyDefinition
        {
            get { return m_KeyDefinition; }
            set { m_KeyDefinition = value; }
        }
    }
    /// <summary>
    /// TEliteCommandResponse
    /// </summary>
    public class TEliteCommandResponse : TEliteMessage
    {

        #region Enumerations
        //**************************************************
        // Enumerations
        //**************************************************
        /// <summary>
        /// ColorResultEnum
        /// </summary>
        public enum ColorResultEnum
        {
            /// <summary>
            /// Success
            /// </summary>
            Success = 0x03, 
            /// <summary>
            /// Error
            /// </summary>
            Error = 0x06, 
            /// <summary>
            /// SuccessText
            /// </summary>
            SuccessText = 0x07
        };

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
        /// Command line response message data as array
        /// </summary>
        private byte[] m_Data = null;

        /// <summary>
        /// Command Line data member variable
        /// </summary>
        private string m_CommandLine = string.Empty;

        /// <summary>
        /// Color Code of the command line text member variable
        /// </summary>
        private byte m_ColorCode = 0;

        /// <summary>
        /// Soft Key list member variable
        /// </summary>
        private List<TEliteSoftKey> m_SoftKeys;

        /// <summary>
        /// Command line response file data
        /// </summary>
        private byte[][] m_FileData = null;

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
        /// Gets the command line text
        /// </summary>
        public string CommandLine
        {
            get { return m_CommandLine; }
        }

        /// <summary>
        /// Gets the command line color code
        /// </summary>
        public byte ColorCode
        {
            get { return m_ColorCode; }
        }

        /// <summary>
        /// Gets the command line color result (meaning of color code)
        /// </summary>
        public ColorResultEnum ColorResult
        {
            get { return (ColorResultEnum)m_ColorCode; }
        }

        /// <summary>
        /// Gets the command line response file data
        /// </summary>
        public byte[][] FileData
        {
            get { return m_FileData; }
            set { m_FileData = value; }
        }

        /// <summary>
        /// Gets the response file data as readable string without vortex/flair markup
        /// </summary>
        public string FileDataTextOnly
        {
            get {
                StringBuilder sbFileContent = new StringBuilder();
                for (int i = 0; i < m_FileData.Length; i++)
                {
                    for (int j = 0; j < m_FileData[i].Length; j++)
                    {
                        if (m_FileData[i][j] == 0x7E)
                        {
                            if (m_FileData[i][j + 1] == 0x42 && m_FileData[i][j + 2] == 0x04 && m_FileData[i][j + 3] == 0x1D) j++;
                            j += 2;
                        }
                        else
                        {
                            sbFileContent.Append((char)m_FileData[i][j]);
                        }
                    }
                    if (i < m_FileData.Length - 1) sbFileContent.AppendLine();
                }
                return sbFileContent.ToString();
            }
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
        /// Initializes a new instance of the TEliteCommandResponse class.
        /// </summary>
        public TEliteCommandResponse()
        {
            this.m_MessageOrigin = MessageOriginEnum.Vortex;
            this.m_Cmd = MessageTypeCode.CommandResponse;

            m_SoftKeys = new List<TEliteSoftKey>();
            m_SoftKeys.Clear();
        }

        /// <summary>
        /// Initializes a new instance of the TEliteCommandResponse class.
        /// </summary>
        public TEliteCommandResponse(byte[] messageFrame)
            : this()
        {
            this.m_Data = messageFrame;

            StringBuilder text = new StringBuilder();
            for (int i = 0; i < 80; i++)
            {
                if (messageFrame[i] >= 0x20)
                {
                    // set command line text
                    text.Append((char)messageFrame[i]);
                }
                else
                {
                    // set command line color code
                    if (messageFrame[i] == 0x03 || messageFrame[i] == 0x06 || messageFrame[i] == 0x07)
                    {
                        this.m_ColorCode = messageFrame[i];
                    }
                }
            }

            this.m_CommandLine = text.ToString().Trim();
        }

        /// <summary>
        /// Alternative Constructor for setting another MessageTypeCode (e.g. FileBrowse)
        /// </summary>
        /// <param name="messageFrame"></param>
        /// <param name="messageTypeCode"></param>
        public TEliteCommandResponse(byte[] messageFrame, MessageTypeCode messageTypeCode)
            : this(messageFrame)
        {
            this.m_Cmd = messageTypeCode;
        }

        #endregion

        #region Destructor
        //**************************************************
        // Destructor
        //**************************************************

        /// <summary>
        /// default destructor of class
        /// </summary>
        ~TEliteCommandResponse()
        {
            m_SoftKeys.Clear();
        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Converts CommandResponse message to a byte array
        /// </summary>
        /// <returns></returns>
        public override byte[] ToArray()
        {
            return this.m_Data;
        }

        #endregion

        #region Protected Methods
        //**************************************************
        // Protected Methods
        //**************************************************

        #endregion

        #region Privat Methods
        //**************************************************
        // Privat Methods
        //**************************************************

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
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteCommandResponse.cs $
 * 
 * 7     20.07.09 16:37 Not
 * file data also with cleanup available
 * 
 * 6     20.07.09 13:34 Not
 * command line responses in file format can be handled now
 * 
 * 5     19.06.09 15:01 san
 * 
 * 4     18.06.09 16:23 san
 * 
 * 3     18.06.09 14:57 san
 * 
 * 2     17.06.09 16:29 san
 * 
 * 1     17.06.09 15:06 san
 * -------------------------------------------------------------------
 */