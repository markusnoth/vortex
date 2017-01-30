// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TElitePageRequest.cs $
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
    /// TElitePageRequest
    /// </summary>
    public class TElitePageRequest : TEliteMessage
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
        public TEliteCommandResponse.ColorResultEnum ColorResult
        {
            get { return (TEliteCommandResponse.ColorResultEnum)m_ColorCode; }
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

        /// <summary>
        /// Page response message data as array
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
        /// Initializes a new instance of the TElitePageRequest class.
        /// </summary>
        public TElitePageRequest()
        {
            this.m_MessageOrigin = MessageOriginEnum.Vortex;
            this.m_Cmd = MessageTypeCode.PageRequest;
        }

        /// <summary>
        /// Initializes a new instance of the TElitePageRequest class.
        /// </summary>
        public TElitePageRequest(byte[] messageFrame)
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

        #endregion

        #region Destructor
        //**************************************************
        // Destructor
        //**************************************************

        /// <summary>
        /// default destructor of class
        /// </summary>
        ~TElitePageRequest()
        {
        }
        #endregion

        #region Public Methods
        //**************************************************
        // Public Methods
        //**************************************************

        /// <summary>
        /// Converts PageRequest message to a byte array
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

        #region Web-Methods
        //**************************************************
        // Web-Methods
        //**************************************************

        #endregion

        #endregion

    }

}
/* 
 * -------------------------------------------------------------------
 * History:
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TElitePageRequest.cs $
 * 
 * 1     22.06.09 13:35 san
 * -------------------------------------------------------------------
 */