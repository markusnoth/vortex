// -------------------------------------------------------------------
// Project: Vortex TElite Protocol
//
// $HeadURL: http://svn/repos/casper/CASPer/trunk/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteErrorResponse.cs $
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
    /// TEliteUserAccessResponse
    /// </summary>
    public class TEliteErrorResponse : TEliteMessage
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
        /// Command line response message data as array
        /// </summary>
        private byte[] m_Data = null;

        /// <summary>
        /// Command Line data member variable
        /// </summary>
        private string m_CommandLine = string.Empty;

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
        /// Initializes a new instance of the TEliteErrorResponse class.
        /// </summary>
        public TEliteErrorResponse()
        {
            this.m_MessageOrigin = MessageOriginEnum.Vortex;
            this.m_Cmd = MessageTypeCode.ErrorResponse;
        }

        /// <summary>
        /// Initializes a new instance of the TEliteErrorResponse class.
        /// </summary>
        public TEliteErrorResponse(byte[] messageFrame)
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
        ~TEliteErrorResponse()
        {
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
 * $Log: /CASPer/03_SW/CASPer/VortexTEliteProtocol/VortexTEliteProtocol/TEliteErrorResponse.cs $
 * 
 * 1     2.07.09 15:35 san
 * -------------------------------------------------------------------
 */